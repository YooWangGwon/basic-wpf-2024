using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ver1.Model;

namespace ver1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 1초마다 업데이트
            timer.Tick += Timer_Tick;
            timer.Start(); // 타이머 시작
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TxbCurrDateTime.Text = DateTime.Now.ToString(); // 현재 시간 업데이트
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetAirInfo();
            GrdStatusChange(0);
            MapDegreeChange(0);
        }

        private async void GetAirInfo()
        {
            string encodingKey = "HOkDfo8ke5g4hoc9JTcZh2xTTnBZ2a1woU250HmKXH1kEKsDJrPU0kG8pj2sJA9%2Bo%2B8mMCdnk%2BRExaRf%2FAC%2BeA%3D%3D";

            //string decodingKey = "HOkDfo8ke5g4hoc9JTcZh2xTTnBZ2a1woU250HmKXH1kEKsDJrPU0kG8pj2sJA9+o+8mMCdnk+RExaRf/AC+eA==";

            string openApiUri = "http://apis.data.go.kr/6260000/AirQualityInfoService/" +
                                $"getAirQualityInfoClassifiedByStation?serviceKey={encodingKey}&pageNo=1" +
                                "&numOfRows=31&resultType=json";
            string result = string.Empty;

            // WebRequest, WebResponse 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUri);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();

                //await this.ShowMessageAsync("결과", result);
                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", ex.Message);
            }

            var jsonResult = JObject.Parse(result);
            //var status = Convert.ToInt32(jsonResult["header"]);
            //await this.ShowMessageAsync("결과", $"{status}");

            var status = Convert.ToInt32(jsonResult["getAirQualityInfoClassifiedByStation"]["header"]["resultCode"]);

            // 데이터 출력
            try
            {
                if (status == 00)
                {
                    var data1 = jsonResult["getAirQualityInfoClassifiedByStation"]["body"]["items"]["item"];

                    var jsonArray = data1 as JArray;

                    var airSensor = new List<AirSensor>();
                    foreach (var item in jsonArray)
                    {
                        airSensor.Add(new AirSensor()
                        {
                            Site = Convert.ToString(item["site"]),
                            //Controlnumber = Convert.ToDateTime(item["controlnumber"]),
                            No2 = Convert.ToDouble(item["no2"]),
                            No2Cai = Convert.ToString(item["no2Cai"]),
                            O3 = Convert.ToDouble(item["o3"]),
                            O3Cai = Convert.ToString(item["o3Cai"]),
                            So2 = Convert.ToDouble(item["so2"]),
                            So2Cai = Convert.ToString(item["so2Cai"]),
                            Pm25 = Convert.ToDouble(item["pm25"]),
                            Pm25Cai = Convert.ToString(item["pm25Cai"]),
                            Pm10 = Convert.ToDouble(item["pm10"]),
                            Pm10Cai = Convert.ToString(item["pm10Cai"]),
                            Co = Convert.ToDouble(item["co"]),
                            CoCai = Convert.ToString(item["coCai"]),
                        });
                    }

                    //var sensorData = new ListCollectionView(airSensor);
                    //sensorData.SortDescriptions.Add(new SortDescription("Site", ListSortDirection.Ascending));
                    //this.DataContext = sensorData;
                    this.DataContext = airSensor;

                    StsResult.Content = $"OpenAIP {airSensor.Count}건 조회완료!";
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", ex.Message);
            }
        }

        private void MapDegreeChange(int indexNum)
        {
            var siteList = new List<TextBlock>
            {
                Gwangbok,
                Noksan,
                Daeyeon,
                Daejeo,
                Deokcheon,
                Myeongjang,
                Choryang,
                Taejong,
                Jeonpo,
                Oncheon,
                Hakjang,
                Cheongryong,
                Jwadong,
                Jangnim,
                Yeonsan,
                Gijang,
                Yongsu,
                Sujeong,
                Boogok,
                Gwangan,
                Daeshin,
                HwaMeong,
                Jaesong,
                Myeongji,
                CheongHack,
                Hoedong,
                Deokpo,
                Gaegeum,
                Dangni
            };

            foreach (AirSensor item in GrdResult.Items)
            {
                foreach(TextBlock site in siteList)
                {
                    if(string.Equals(item.Site, site.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        switch(indexNum)
                        {
                            case 0:
                                site.Text = item.Pm10.ToString();
                                continue;
                            case 1:
                                site.Text = item.Pm25.ToString();
                                continue;
                            case 2:
                                site.Text = item.No2.ToString();
                                continue;
                            case 3:
                                site.Text = item.Co.ToString();
                                continue;
                            case 4:
                                site.Text = item.So2.ToString();
                                continue;
                            case 5:
                                site.Text = item.O3.ToString();
                                continue;
                        }
                    }
                }
            }
        }

        private void CboIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetSite();
            switch (CboIndex.SelectedIndex)
            {
                case 0:
                    Figure.Header = "미세먼지((PM-10))";
                    Index.Header = "미세먼지 지수";
                    var binding1 = new Binding("Pm10");
                    var binding2 = new Binding("Pm10Cai");
                    Figure.Binding = binding1;
                    Index.Binding = binding2;
                    Type.Content = "미세먼지";
                    MapDegreeChange(0);
                    GrdStatusChange(0);
                    return;

                case 1:
                    Figure.Header = "초미세먼지(PM-2.5)";
                    Index.Header = "초미세먼지 지수";
                    var binding3 = new Binding("Pm25");
                    var binding4 = new Binding("Pm25Cai");
                    Figure.Binding = binding3;
                    Index.Binding = binding4;
                    Type.Content = "초미세먼지";
                    MapDegreeChange(1);
                    GrdStatusChange(1);
                    return;

                case 2:
                    Figure.Header = "이산화질소(NO2)";
                    Index.Header = "이산화질소 지수";
                    var binding5 = new Binding("No2");
                    var binding6 = new Binding("No2Cai");
                    Figure.Binding = binding5;
                    Index.Binding = binding6;
                    Type.Content = "이산화질소";
                    MapDegreeChange(2);
                    GrdStatusChange(2);
                    return;

                case 3:
                    Figure.Header = "일산화탄소(CO)";
                    Index.Header = "일산화탄소 지수";
                    var binding7 = new Binding("Co");
                    var binding8 = new Binding("CoCai");
                    Figure.Binding = binding7;
                    Index.Binding = binding8;
                    Type.Content = "일산화탄소";
                    MapDegreeChange(3);
                    GrdStatusChange(3);
                    return;

                case 4:
                    Figure.Header = "이황산가스(SO2)";
                    Index.Header = "이황산가스 지수";
                    var binding9 = new Binding("So2");
                    var binding10 = new Binding("So2Cai");
                    Figure.Binding = binding9;
                    Index.Binding = binding10;
                    Type.Content = "이황산가스";
                    MapDegreeChange(4);
                    GrdStatusChange(4);
                    return;

                case 5:
                    Figure.Header = "오존(O3)";
                    Index.Header = "오존 지수";
                    var binding11 = new Binding("O3");
                    var binding12 = new Binding("O3Cai");
                    Figure.Binding = binding11;
                    Index.Binding = binding12;
                    Type.Content = "오존";
                    MapDegreeChange(5);
                    GrdStatusChange(5);
                    return;
            }
        }

        private void ResetSite()
        {
            Gwangbok.Text = "광복동";
            Noksan.Text = "녹산동";
            Daeyeon.Text = "대연동";
            Daejeo.Text = "대저동";
            Deokcheon.Text = "덕천동";
            Myeongjang.Text = "명장동";
            Choryang.Text = "초량동";
            Taejong.Text = "태종대";
            Jeonpo.Text = "전포동";
            Oncheon.Text = "온천동";
            Hakjang.Text = "학장동";
            Cheongryong.Text = "청룡동";
            Jwadong.Text = "좌동";
            Jangnim.Text = "장림동";
            Yeonsan.Text = "연산동";
            Gijang.Text = "기장읍";
            Yongsu.Text = "용수리";
            Sujeong.Text = "수정동";
            Boogok.Text = "부곡동";
            Gwangan.Text = "광안동";
            Daeshin.Text = "대신동";
            HwaMeong.Text = "화명동";
            Jaesong.Text = "재송동";
            Myeongji.Text = "명지동";
            CheongHack.Text = "청학동";
            Hoedong.Text = "회동동";
            Deokpo.Text = "덕포동";
            Gaegeum.Text = "개금동";
            Dangni.Text = "당리동";
        }

        private void GrdStatusChange(int indexNum)
        {
            switch(indexNum)
            {
                case 0:
                    var pm10Status = new List<AirStatus>();
                    pm10Status.Add(new AirStatus() { GoodDegree = "0~30", NormalDegree = "31 ~ 80", BadDegree = "81 ~ 150", VeryBadDegree = "151 이상" });
                    GrdStatus.ItemsSource = pm10Status;
                    return;

                case 1:
                    var pm25Status = new List<AirStatus>();
                    pm25Status.Add(new AirStatus() { GoodDegree = "0~15", NormalDegree = "16 ~ 35", BadDegree = "36 ~ 75", VeryBadDegree = "76 이상" });
                    GrdStatus.ItemsSource = pm25Status;
                    return;

                case 2:
                    var no2Status = new List<AirStatus>();
                    no2Status.Add(new AirStatus() { GoodDegree = "0~0.030", NormalDegree = "0.031 ~ 0.060", BadDegree = "0.061 ~ 0.200", VeryBadDegree = "0.201 이상" });
                    GrdStatus.ItemsSource = no2Status;
                    return;

                case 3:
                    var coStatus = new List<AirStatus>();
                    coStatus.Add(new AirStatus() { GoodDegree = "0~2.00", NormalDegree = "2.01 ~ 9.00", BadDegree = "9.01 ~ 15.00", VeryBadDegree = "15.01 이상" });
                    GrdStatus.ItemsSource = coStatus;
                    return;
                case 4:
                    var so2Status = new List<AirStatus>();
                    so2Status.Add(new AirStatus() { GoodDegree = "0~0.020", NormalDegree = "0.021 ~ 0.050", BadDegree = "0.051 ~ 0.150", VeryBadDegree = "0.151 이상 이상" });
                    GrdStatus.ItemsSource = so2Status;
                    return;
                case 5:
                    var o3Status = new List<AirStatus>();
                    o3Status.Add(new AirStatus() { GoodDegree = "0~0.030", NormalDegree = "0.031 ~ 0.090", BadDegree = "0.091 ~ 0.150", VeryBadDegree = "0.151 이상" });
                    GrdStatus.ItemsSource = o3Status;
                    return;
            }
        }

        private void BtnF5_Click(object sender, RoutedEventArgs e)
        {
            GetAirInfo();
        }
    }
}