using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeDLL.DataTypes;

namespace YoutubeDLL
{
    internal interface Iinterface 
    {
        VideoList GetVideos(Playlist playlist);
        Video GetVideo(string id);
        Playlist GetPlaylist(string id);
        PlaylistCollection GetPlaylists();
    }

    public class GoogleConnection : Iinterface
    {
        public Playlist GetPlaylist(string id)
        {
            return YoutubeApi.getPlaylist(id);
        }

        public PlaylistCollection GetPlaylists()
        {
            return YoutubeApi.GetPlaylists().Result;
        }

        public async Task<PlaylistCollection> GetPlaylistsAsync()
        {
            return await YoutubeApi.GetPlaylists();
        }

        public Video GetVideo(string id)
        {
            return YoutubeApi.GetVideo(id);
        }

        public VideoList GetVideos(Playlist playlist)
        {
            return YoutubeApi.GetVideos(playlist).Result;
        }
    }

}
