using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDLL.DataTypes;

namespace YoutubeDLL
{
    /// <summary>
    /// class for handling all the youtube request's, auto-authorizes the user on call
    /// </summary>
    internal class YoutubeApi
    {

        private static UserCredential YoutubeCreds = auth();

        private static YouTubeService ytService = LaunchService(YoutubeCreds);

        private static UserCredential auth()
        {
            UserCredential credentials;

            ClientSecrets secrets = new ClientSecrets();
            secrets.ClientId = "165704796803-92u17jru4pe62f821srjet338ap9bp5i.apps.googleusercontent.com";
            secrets.ClientSecret = "N1RAbhCg_AhPq7J3gZnHMZu3";

                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                new[] { YouTubeService.Scope.YoutubeReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore("youtubeApi")
                ).Result;

            return credentials;
        }

        private static YouTubeService LaunchService(UserCredential credentials)
        {
            YouTubeService service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Youtube Playlist Storage"
            });
           
            return service;
        }

        /// <summary>
        /// logs the current user out by revoking the access token used and destroying the youtube service
        /// </summary>
        public static void logOut()
        {
            ytService.Dispose();
            YoutubeCreds.RevokeTokenAsync(CancellationToken.None);
        }

        public static Playlist getPlaylist(string id)
        {
            var request = ytService.Playlists.List("snippet");
            request.Id = id;

            var response = request.Execute();

            if (response.Items.Count == 1)
            {
                var item = response.Items[0];
                return new Playlist(item.Snippet.Title, item.Id, false);
            }
            else
                return null;
        }

        /// <summary>
        /// gets all the playlist's of the currently logged-in user
        /// </summary>
        public async static Task<PlaylistCollection> GetPlaylists()
        {
            PlaylistCollection collection = new PlaylistCollection();

            var request = ytService.Playlists.List("ContentDetails, snippet");
            var nextPageToken = "";
            request.Mine = true;

            while (nextPageToken != null)
            {
                request.PageToken = nextPageToken;
                var response = await request.ExecuteAsync();

                foreach (var playlist in response.Items)
                {
                    Playlist local = new Playlist(playlist.Snippet.Title, playlist.Id, false);
                    collection.Add(local);
                }

                nextPageToken = response.NextPageToken;
            }
            return collection;
        }

        /// <summary>
        /// searches for a playlist with the provided name, get's all the videos and return's them in a list
        /// </summary>
        /// <param name="playlistname">the name of the playlist to search for</param>
        /// <returns>a video collection with all the videos of the playlist in <see cref="Video"/> format</returns>
        public static async Task<Playlist> GetVideos(Playlist playlist)
        {
            playlist.Items = null;

            var request = ytService.PlaylistItems.List("snippet");
            request.PlaylistId = playlist.ID;
            string nextPageToken = "";

            while (nextPageToken != null)
            {
                request.PageToken = nextPageToken;
                var response = await request.ExecuteAsync();

                foreach (var video in response.Items)
                {
                DataTypes.Video realvideo = new DataTypes.Video(video.Snippet.Title, video.Id, false);
                    playlist.Add(realvideo);
                }
                nextPageToken = response.NextPageToken;
            }
            return playlist;
        }

        public static async Task<VideoList> GetVideos(string id)
        {
            var request = ytService.PlaylistItems.List("snippet");
            request.PlaylistId = id;
            string nextPageToken = "";

            VideoList list = new VideoList();

            while (nextPageToken != null)
            {
                request.PageToken = nextPageToken;
                var response = await request.ExecuteAsync();

                

                foreach (var video in response.Items)
                {
                    DataTypes.Video realvideo = new DataTypes.Video(video.Snippet.Title, video.Id, false);
                    list.Add(realvideo);
                }
                nextPageToken = response.NextPageToken;
            }
            return list; 
        }

        public static VideoList GetVideos(Playlist playlist, IProgress<int> progress)
        {
            VideoList videos = new VideoList();
            
            var request = ytService.PlaylistItems.List("snippet");
            request.PlaylistId = playlist.ID;
            request.MaxResults = (int)Math.Ceiling(playlist.Count / 10f);
            string nextPageToken = "";
            int progresspercentage = 0;

            while (nextPageToken != null)
            {
                request.PageToken = nextPageToken;
                var response = request.Execute();

                foreach (var video in response.Items)
                {
                    DataTypes.Video realvideo = new DataTypes.Video(video.Snippet.Title, video.Id, false);
                    videos.Add(realvideo);
                }
                nextPageToken = response.NextPageToken;

                if(progress != null)
                {
                    progress.Report(progresspercentage += 10);
                } 
            }

            return videos;
        }

        public static YoutubeDLL.DataTypes.Video GetVideo(string id)
        {
            var request = ytService.Videos.List("snippet");
            request.Id = id;

            var response = request.Execute();

            if (response.Items.Count == 1)
            {
                return new DataTypes.Video(response.Items[0].Snippet.Title, response.Items[0].Id, false);
            }
            else
                return null;
        }
    }
}
