using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.IO;
using System.Threading;
using YoutubeDLL;
using System.Collections;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YoutubeDLL
{

    public delegate void DownloadDelegate(int totalItems, int currentItems);

    /// <summary>
    /// class for handling all the youtube request's, auto-authorizes the user on call
    /// </summary>
    public class YoutubeApi
    {

        private static UserCredential YoutubeCreds = auth();

        private static YouTubeService ytService = (YouTubeService)LaunchService(YoutubeCreds);

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

        /// <summary>
        /// gets all the playlist's of the currently logged-in user
        /// </summary>
        public async static Task<YTplaylistCollection> GetPlaylists()
        {
            YTplaylistCollection collection = new YTplaylistCollection();

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
        public static async Task<YTVidList> GetVideos(string playlistId)
        {
            YTVidList videos = new YTVidList();

            if (playlistId != null)
            {
                var request = ytService.PlaylistItems.List("snippet");
                request.PlaylistId = playlistId;
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
            }
            else
            {
                throw new ArgumentException(playlistId, "the playlist searched for does not exist");
            }

            return videos;
        }

        public static YTVidList GetVideos(string playlistId, DownloadDelegate Callback)
        {
            YTVidList videos = new YTVidList();

            if (playlistId != null)
            {
                var request = ytService.PlaylistItems.List("snippet");
                request.PlaylistId = playlistId;
                request.MaxResults = 10;
                string nextPageToken = "";

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
                    Callback.Invoke((int)response.PageInfo.TotalResults, (int)response.Items.Count);
                }
            }
            else
            {
                throw new ArgumentException(playlistId, "the playlist searched for does not exist");
            }

            return videos;
        }

    }

}
