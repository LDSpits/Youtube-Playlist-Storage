using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YoutubeDLL.DataTypes
{

    public interface IPlayList
    {
        string Title
        {
            get;
            set;
        }

        string ID
        {
            get;
            set;
        }
    }

    public class Playlist : VideoList, IPlayList
    {
        private string playlistTitle;
        private string playlistId;
        private int totalVideos;

        public static implicit operator List<Video>(Playlist i)// van List<videos> naar PlayList
        {
            return i._list;
        }
        
        [JsonIgnore]
        public string Title
        {
            get { return playlistTitle; }
            set { playlistTitle = value; }
        }

        [JsonIgnore]
        public string ID
        {
            get { return playlistId; }
            set { playlistId = value; }
        }

        public Playlist(string Title, string Id, bool populate)
        {
            playlistTitle = Title;
            playlistId = Id;

            if (populate)
                Items = YoutubeApi.GetVideos(ID).Result;
            else
                _list = null;
        }

        public Playlist(string Title, string Id, List<Video> Videos)
        {
            playlistTitle = Title;
            playlistId = Id;
            _list = Videos;
        }

        public Playlist(string Title, string Id, VideoList Videos)
        {
            playlistTitle = Title;
            playlistId = Id;
            _list = Videos;
        }

    }
}
