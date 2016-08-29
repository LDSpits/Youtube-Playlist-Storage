using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDLL.DataTypes;
using Windows.Security.Authentication.Web;
using System.Text;

namespace YoutubeDLL
{

    public delegate void DownloadDelegate(int totalItems, int currentItems);

    /// <summary>
    /// class for handling all the youtube request's, auto-authorizes the user on call
    /// </summary>
    public class YoutubeApi
    {
        

        private static UserCredential YoutubeCreds = auth();

        private static YouTubeService ytService = LaunchService(YoutubeCreds);

        private static UserCredential auth()
        {
            UserCredential credentials;

            credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
            new Uri("ms-appx:YoutubeGUI/Assets/client_secrets.json"),
            new[] { YouTubeService.Scope.YoutubeReadonly },
            "user",
            CancellationToken.None
            ).Result;

            

            return credentials;
        }

        private static async void Authenticate()
        {
            StringBuilder addressbuilder = new StringBuilder();
            addressbuilder.Append("https://accounts.google.com/o/oauth2/auth?client_id=");
            addressbuilder.Append("165704796803-92u17jru4pe62f821srjet338ap9bp5i.apps.googleusercontent.com");
            addressbuilder.Append("");


            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, )
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

        /// <summary>
        /// gets all the playlist's of the currently logged-in user
        /// </summary>
        public async static Task<YTplaylistColl> GetPlaylists()
        {
            YTplaylistColl collection = new YTplaylistColl();

            var request = ytService.Playlists.List("ContentDetails, snippet");
            var nextPageToken = "";
            request.Mine = true;

            while (nextPageToken != null)
            {
                request.PageToken = nextPageToken;
                var response = await request.ExecuteAsync();

                foreach (var playlist in response.Items)
                {
                    YTPlaylist local = new YTPlaylist(playlist.Snippet.Title, playlist.Id, (int)playlist.ContentDetails.ItemCount);
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
        /// <returns>a video collection with all the videos of the playlist in <see cref="YTVideo"/> format</returns>
        public static async Task<YTVidList> GetVideos(YTPlaylist playlist)
        {
            YTVidList videos = new YTVidList();

                var request = ytService.PlaylistItems.List("snippet");
                request.PlaylistId = playlist.Id;
                string nextPageToken = "";

                while (nextPageToken != null)
                {
                    request.PageToken = nextPageToken;

                    var response = await request.ExecuteAsync();

                    foreach (var video in response.Items)
                    {
                        YTVideo realvideo = new YTVideo(video.Snippet.Title, video.Id, false);
                        videos.Add(realvideo);
                    }
                    nextPageToken = response.NextPageToken;
                }

            return videos;
        }

        public static YTVidList GetVideos(YTPlaylist playlist, IProgress<int> progress)
        {
            YTVidList videos = new YTVidList();
            
            var request = ytService.PlaylistItems.List("snippet");
            request.PlaylistId = playlist.Id;
            request.MaxResults = (int)Math.Ceiling(playlist.items.Count / 10f);
            string nextPageToken = "";
            int progresspercentage = 0;

            while (nextPageToken != null)
            {
                request.PageToken = nextPageToken;

                var response = request.Execute();

                foreach (var video in response.Items)
                {
                    YTVideo realvideo = new YTVideo(video.Snippet.Title, video.Id, false);
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

    }

}
