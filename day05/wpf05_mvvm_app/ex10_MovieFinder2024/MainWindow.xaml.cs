using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Web;
using System.Windows;
using System.Text;
using System.Windows.Controls;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace ex10_MovieFinder2024
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

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //await this.ShowMessageAsync("검색", "검색을 시작합니다!!");
            if(string.IsNullOrEmpty(TxtMovieName.Text))
            {
                await this.ShowMessageAsync("검색", "검색할 영화명을 입력하세요.");
                return;
            }

            SearchMovie(TxtMovieName.Text);
        }

        private async void SearchMovie(string movieName)
        {
            string tmdb_apiKey = "0d917c87f0ae3e49eb4e7a4616ee593c"; // TMDB 사이트에서 제공받음 API 키
            string encoding_movieName = HttpUtility.UrlEncode(movieName, Encoding.UTF8);
            string openApiUri = $"https://api.themoviedb.org/3/search/movie?api_key={tmdb_apiKey}" +
                                $"&language=ko-KR&page=1&include_adult=false&query={encoding_movieName}";
            //Debug.WriteLine(openApiUri);

            string result = string.Empty; // 결과값을 담을 곳
            WebRequest req = null;
            WebResponse res = null;
            // Web은 Request와 Response 모두 필요
            StreamReader reader = null;

            try
            {
                // tmdb api 요청
                req = WebRequest.Create(openApiUri);    // URL을 넣어서 객체를 생성
                res = await req.GetResponseAsync(); // 요청한 URL의 결과를 비동기 응답으로 받기
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd(); // json 결과를 문자열로 저장

                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
                //TODO : 메시지 박스 출력하기
            }
            finally 
            {
                reader.Close();
                res.Close();
                //req는 안닫아도 됨
            }
        }

        private void TxtMovieName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private async void GrdResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            await this.ShowMessageAsync("포스터", "포스터를 처리합니다!!");
        }

        private async void BtnAddFavorite_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("즐겨찾기", "즐겨찾기를 추가합니다!!");
        }

        private async void BtnViewFavorite_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("즐겨찾기", "즐겨찾기를 확인합니다!!");
        }

        private async void BtnDelFavorite_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("즐겨찾기", "즐겨찾기를 삭제합니다!!");
        }

        private async void BtnWatchTrailer_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowMessageAsync("예고편", "유튜브 예고편을 확인합니다!!");
        }
    }
}