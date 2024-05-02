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

namespace ex05_wpf_bikeshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 객체 생성시 프로퍼티(변수) 값을 설정하는 방법 1  -> 객체 생성시 속성 초기화
            Bike mybike = new Bike();
            mybike.Speed = 60;
            mybike.Color = Colors.Aqua; //Color.FromArgb(255, 255, 0, 0);

            // 값을 설정하는 방법 2 -> 객체 생성시 프로퍼티(변수) 값을 설정
            Bike yourbike = new Bike() { Speed = 50, Color = Colors.Aqua };

            StsBike.DataContext = yourbike; // WPF 방식

            TxtMyBikeSpeed.Text = mybike.Speed.ToString(); // 전통적인 윈폼 방식
        }

        private void TxtMyBikeSpeed_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
            TxtCopySpeed.Text = TxtMyBikeSpeed.Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("버튼 클릭!!");
        }
    }
}