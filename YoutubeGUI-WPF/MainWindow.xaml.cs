using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YoutubeDLL;
using YoutubeDLL.DataTypes;

namespace YoutubeGUI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GoogleConnection connection = new GoogleConnection();
        PlaylistCollection online = null;

        public MainWindow()
        {
            init();

            InitializeComponent();

            DataView.ItemsSource = (IList<IPlayList>)online;
        }

        private async void init()
        {
            online = await connection.GetPlaylistsAsync();
        }
    }

    
}
