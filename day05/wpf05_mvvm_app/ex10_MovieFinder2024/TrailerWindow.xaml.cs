using ex10_MovieFinder2024.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ex10_MovieFinder2024
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {
        List<YoutubeItem> youtubeItems = null; // 유튜브 API 검색결과를 담을 객체리스트 

        public TrailerWindow()
        {
            InitializeComponent();
        }

        // MainWindow 그리드에서 선택된 영화 제목을 넘기면서 생성
        // 재정의 생성자(기본 생성자인 TrailerWindow()먼저 실행하고 movieName을 받아 실행)
        public TrailerWindow(string movieName) : this() 
        {
            // this() => TrailerWindow() 기본생성자를 먼저 실행한 뒤 아래를 실행
            LblMovieName.Content = $"{movieName} 예고편";
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            youtubeItems = new List<YoutubeItem>(); // 초기화
            SearchYoutubeApi(); // 핵심메서드 실행
        }

        private async void SearchYoutubeApi()
        {
            await LoadDataCollection(); // 비동기로 실제 유튜브 API를 실행
            LsvResult.ItemsSource = youtubeItems;

            LsvResult.SelectedIndex = 0;
        }

        private async Task LoadDataCollection()
        {
            // Youtube서비스용 패키지 다운로드 : Google.Apis.YouTube.v3
            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey= "AIzaSyCtVYVSu1vOL6ipwQszmpnO_jjZlozDu_8",
                ApplicationName = this.GetType().ToString()
            });

            var req = service.Search.List("snippet");
            req.Q = LblMovieName.Content.ToString(); // 어벤져스 인피니티워 예고편
            req.MaxResults = 10; // 검색결과 수를 10개로 제한

            var res = await req.ExecuteAsync(); // Youtube 서버에서 요청된 값을 실행하고 결과를 리턴(비동기)

            //await this.ShowMessageAsync("검색결과", res.Items.Count.ToString());
            
            foreach(var item in res.Items)
            {
                if(item.Id.Kind.Equals("youtube#video")) // 동영상 플레이가 아이템만
                {
                    var youtube = new YoutubeItem()
                    {
                        Title = item.Snippet.Title,
                        ChannelTitle = item.Snippet.ChannelTitle,
                        URL = $"https://www.youtube.com/watch?v={item.Id.VideoId}", // 유튜브 플레이 링크
                        Author = item.Snippet.ChannelId,
                    };

                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url, UriKind.RelativeOrAbsolute));
                    youtubeItems.Add(youtube);
                }
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 한번씩 ChefSharp 브라우저에서 메모리 누수가 발생
            BrsYouTube.Address = string.Empty;
            BrsYouTube.Dispose(); // 객체 해제, 종종 웹 어플리케이션 만들 때 앱 종료시 객체를 완전 해제
        }

        private void LsvResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LsvResult.SelectedItem is YoutubeItem) // is의 결과는 true/false
            {
                var video = LsvResult.SelectedItem as YoutubeItem;  // as => casting 실패하면 null
                BrsYouTube.Address = video.URL;
            }
        }
    }
}
