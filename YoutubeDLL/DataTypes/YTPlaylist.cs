using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YoutubeDLL.DataTypes
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class YTPlaylist
    {
        private string playlistTitle;
        private string playlistId;
        private int totalVideos;
        private YTVidList internalVideos;

        [JsonIgnore]
        public int VideoCount
        {
            get
            {
                if (internalVideos != null)
                {
                    totalVideos = internalVideos.Count;
                    return internalVideos.Count;
                }
                else
                {
                    return totalVideos;
                }
            }
        }

        [JsonIgnore]
        public string Title
        {
            get { return playlistTitle; }
            set { playlistTitle = value; }
        }

        [JsonIgnore]
        public string Id
        {
            get { return playlistId; }
            set { playlistId = value; }
        }

        [JsonIgnore]
        public YTVidList items
        {
            get { return internalVideos; }
            set { internalVideos = value; }
        }

        public YTPlaylist(string Title, string Id, int count)
        {
            playlistTitle = Title;
            playlistId = Id;
            totalVideos = count;
            internalVideos = null;
        }

        public YTPlaylist(string Title, string Id, int count, YTVidList Videos)
        {
            playlistTitle = Title;
            playlistId = Id;
            totalVideos = count;
            internalVideos = Videos;
        }

    }
}
