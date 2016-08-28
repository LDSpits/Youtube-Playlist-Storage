using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YoutubeDLL.DataTypes
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class YTVideo
    {

        private string videoTitle;
        private string videoID;
        private bool wasDeleted;

        [JsonIgnore]
        public string Title
        {
            get { return videoTitle; }
            set { videoTitle = value; }
        }

        [JsonIgnore]
        public string Id
        {
            get { return videoID; }
            set { videoID = value; }
        }

        [JsonIgnore]
        public bool WasDeleted
        {
            get { return wasDeleted; }
            set { wasDeleted = value; }
        }

        public YTVideo(string VideoTitle, string VideoID, bool Deleted)
        {
            videoTitle = VideoTitle;
            videoID = VideoID;
            wasDeleted = Deleted;
        }

    }
}
