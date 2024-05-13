using Busan_Air_Condition.Model;
using ControlzEx.Standard;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
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

namespace Busan_Air_Condition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetAirInfo();
        }

        private async void GetAirInfo()
        {
            string encodingKey = "HOkDfo8ke5g4hoc9JTcZh2xTTnBZ2a1woU250HmKXH1kEKsDJrPU0kG8pj2sJA9%2Bo%2B8mMCdnk%2BRExaRf%2FAC%2BeA%3D%3D";

            //string decodingKey = "HOkDfo8ke5g4hoc9JTcZh2xTTnBZ2a1woU250HmKXH1kEKsDJrPU0kG8pj2sJA9+o+8mMCdnk+RExaRf/AC+eA==";

            string openApiUri = "http://apis.data.go.kr/6260000/AirQualityInfoService/" +
                                $"getAirQualityInfoClassifiedByStation?serviceKey={encodingKey}&pageNo=1" +
                                "&numOfRows=21&resultType=json";
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

                    var sensorData = new ListCollectionView(airSensor);
                    sensorData.SortDescriptions.Add(new SortDescription("Site", ListSortDirection.Ascending));
                    this.DataContext = sensorData;

                    StsResult.Content = $"OpenAIP {airSensor.Count}건 조회완료!";
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", ex.Message);
            }
        }

        private async void MapManipulation()
        {

            Gwangbok.Text = $"{GrdResult.SelectedItem}";
        }

        private void CboIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(CboIndex.SelectedIndex)
            {
                case 0:
                    Figure.Header = "미세먼지((PM-10))";
                    Index.Header = "미세먼지 지수";
                    var binding1 = new Binding("Pm10");
                    var binding2 = new Binding("Pm10Cai");
                    Figure.Binding = binding1;
                    Index.Binding = binding2;
                    Type.Content = "미세먼지";
                    return;

                case 1:
                    Figure.Header = "초미세먼지(PM-2.5)";
                    Index.Header = "초미세먼지 지수";
                    var binding3 = new Binding("Pm25");
                    var binding4 = new Binding("Pm25Cai");
                    Figure.Binding = binding3;
                    Index.Binding = binding4;
                    Type.Content = "초미세먼지";
                    MapManipulation();
                    return;

                case 2:
                    Figure.Header = "이산화질소(NO2)";
                    Index.Header = "이산화질소 지수";
                    var binding5 = new Binding("No2");
                    var binding6 = new Binding("No2Cai");
                    Figure.Binding = binding5;
                    Index.Binding = binding6;
                    Type.Content = "이산화질소";
                    return;

                case 3:
                    Figure.Header = "일산화탄소(CO)";
                    Index.Header = "일산화탄소 지수";
                    var binding7 = new Binding("Co");
                    var binding8 = new Binding("CoCai");
                    Figure.Binding = binding7;
                    Index.Binding = binding8;
                    Type.Content = "일산화탄소";
                    return;

                case 4:
                    Figure.Header = "이황산가스(SO2)";
                    Index.Header = "이황산가스 지수";
                    var binding9 = new Binding("So2");
                    var binding10 = new Binding("So2Cai");
                    Figure.Binding = binding9;
                    Index.Binding = binding10;
                    Type.Content = "이황산가스";
                    return;

                case 5:
                    Figure.Header = "오존(O3)";
                    Index.Header = "오존 지수";
                    var binding11 = new Binding("O3");
                    var binding12 = new Binding("O3Cai");
                    Figure.Binding = binding11;
                    Index.Binding = binding12;
                    Type.Content = "오존";
                    return;
            }


        }
    }
}