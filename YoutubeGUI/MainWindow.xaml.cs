using System.Windows;
using System.Windows.Input;
using YoutubeDLL;
using System.Windows.Controls;
using System.ComponentModel;
using System;
using System.Windows.Threading;
using System.Linq;
using System.Collections;

namespace YoutubeGUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ControlTemplate isloading;
        ProgressBar prgDownload;

        public MainWindow()
        {
            InitializeComponent();
            setup();
            isloading = (ControlTemplate)FindResource("DataLoading");
        }

        private async void setup()
        {
            YTplaylistCollection playCollection = await YoutubeApi.GetPlaylists();
            PlaylistView.ItemsSource = playCollection.GetCollection();
            PlaylistView.Focus();
        }

        private void UpdateListView_Loaded(object sender, RoutedEventArgs e)
        {
            if(UpdateListView.Template == isloading)
            {
                prgDownload = isloading.FindName("LoadingBar", UpdateListView) as ProgressBar;
            }
        }

        private async void PlayListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            YTPlaylist selected = (YTPlaylist)PlaylistView.SelectedItem;
            LocalListView.ClearValue(ItemsControl.ItemsSourceProperty);
            UpdateListView.ClearValue(ItemsControl.ItemsSourceProperty);

            YTVidList list = await YoutubeApi.GetVideos(selected.Id);
            UpdateListView.ItemsSource = list.GetCollection();

            try
            {
                YTVidList data = await DataStore.LoadDataAsync(selected.Id);
                LocalListView.ItemsSource = data.GetCollection();
            }
            catch
            {
                LocalListView.Template = (ControlTemplate)TryFindResource("FileNotFound");
            }
        }

        private void CreateLocal_Click(object sender, MouseButtonEventArgs e)
        {
            DataStore.StoreDataAsync((YTPlaylist)PlaylistView.SelectedItem);
        }

        private void Videoworker_onProgress(int total, int current)
        {
            prgDownload.Dispatcher.Invoke(new Action(() => {
                prgDownload.IsIndeterminate = false;
                prgDownload.Maximum = total;
                prgDownload.Value += current;
            })); 
        }

    }
}
