using System.Windows;
using System.Windows.Input;
using YoutubeDLL;
using System.Windows.Controls;
using System.ComponentModel;
using System;

namespace YoutubeGUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AsyncWorker.DoWork(Worker_DoWork,Worker_onComplete);
        }
        #region workermethods
        private void Dataworker_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = e.Argument as string;

            e.Result = DataStore.LoadData(id); 
        }

        private void Dataworker_onComplete(object sender, RunWorkerCompletedEventArgs e)
        {

            if (LocalListView.HasItems)
            {
                LocalListView.ClearValue(ItemsControl.ItemsSourceProperty);
            }

            BackgroundWorker worker = sender as BackgroundWorker;

            if (e.Error == null)
            {
                YTVidList Stored = e.Result as YTVidList;
                LocalListView.ItemsSource = Stored.GetCollection();
            }
            else
            {
                //give the user a warning that no playlists with that name exists
            }

            worker.DoWork -= Dataworker_DoWork;
            worker.RunWorkerCompleted -= Dataworker_onComplete;
            worker.Dispose();
        }

        private void Videoworker_onComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            YTVidList downloaded = e.Result as YTVidList;
            BackgroundWorker worker = sender as BackgroundWorker;

            if (UpdateListView.HasItems)
            {
                UpdateListView.ClearValue(ItemsControl.ItemsSourceProperty);
            }

            UpdateListView.ItemsSource = downloaded.GetCollection();

            prgDownload.Value = 0;

            worker.RunWorkerCompleted -= Videoworker_onComplete;
            worker.DoWork -= Videoworker_DoWork;
            worker.Dispose();
        }

        private void Videoworker_DoWork(object sender, DoWorkEventArgs e)
        {
            YTVidList list = YoutubeApi.GetVideos((string)e.Argument, Videoworker_onProgress);
            e.Result = list;
        }

        private void Videoworker_onProgress(int total, int current)
        {
            prgDownload.Dispatcher.Invoke(new Action(() => {
                prgDownload.Maximum = total;
                prgDownload.Value += current;
            })); 
        }

        private void Worker_onComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            YTplaylistCollection collection = e.Result as YTplaylistCollection;
            BackgroundWorker worker = sender as BackgroundWorker;

            prgDownload.IsIndeterminate = false;
            PlaylistView.ItemsSource = collection.GetCollection();

            worker.RunWorkerCompleted -= Worker_onComplete;
            worker.DoWork -= Worker_DoWork;
            worker.Dispose();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            YTplaylistCollection coll = YoutubeApi.GetPlaylists();
            e.Result = coll;
        }
        #endregion

        private void PlayListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            YTPlaylist selected = (YTPlaylist)PlaylistView.SelectedItem;

            AsyncWorker.DoWork(Videoworker_DoWork, Videoworker_onComplete, selected.Id);
            AsyncWorker.DoWork(Dataworker_DoWork, Dataworker_onComplete, selected.Id);
        }
    }
}
