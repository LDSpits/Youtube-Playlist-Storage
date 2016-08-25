﻿using System.Windows;
using System.Windows.Input;
using YoutubeDLL;
using System.Windows.Controls;
using System;
using System.Threading.Tasks;
using YoutubeDLL.DataTypes;

namespace YoutubeGUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ControlTemplate isloading;
        ProgressBar prgDownload;

        //fires on start
        public MainWindow()
        {
            InitializeComponent(); 
            setup();// setup the back code
            
        }

        private async void setup()
        {
            isloading = (ControlTemplate)FindResource("DataLoading"); 
            YTplaylistColl playCollection = await YoutubeApi.GetPlaylists(); 
            PlaylistView.ItemsSource = playCollection.GetCollection();
            PlaylistView.Focus();
        }

        private void UpdateListView_Loaded(object sender, RoutedEventArgs e)
        {
            prgDownload = isloading.FindName("LoadingBar", UpdateListView) as ProgressBar;
        }

        
        private async void PlayListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            YTPlaylist selected = (YTPlaylist)PlaylistView.SelectedItem;
            LocalListView.ClearValue(ItemsControl.ItemsSourceProperty);
            UpdateListView.ClearValue(ItemsControl.ItemsSourceProperty);

            YTVidList VideoList = await YoutubeApi.GetVideos(selected);
            UpdateListView.ItemsSource = VideoList.GetCollection();

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

        private async void CreateLocal_Click(object sender, MouseButtonEventArgs e)
        {
            await Task.Run(() => { DataStore.StoreData((YTPlaylist)PlaylistView.SelectedItem); });
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
