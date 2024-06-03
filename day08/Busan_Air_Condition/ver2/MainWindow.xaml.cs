using MahApps.Metro.Controls;
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
using ver2.Views;

namespace ver2
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

        private void BtnAllAir_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new AllAir();
            LblStatus.Content = "전체 대기 상태";
            StsResult.Content = "전체 대기 상태";
        }

        private void BtnRegionAir_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new RegionAir();
            StsResult.Content = "지역별 대기 상태";
            StsResult.Content = "지역별 대기 상태";
        }

        private void BtnDataInquiry_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new DataInquiry();
            StsResult.Content = "데이터 조회";
            StsResult.Content = "데이터 조회";
        }
    }
}