using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YoutubeDLL.DataTypes
{
    public interface IVideo
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

        bool WasDeleted
        {
            get;
            set;
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class Video
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
        public string ID
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

        public Video(string VideoTitle, string VideoID, bool Deleted)
        {
            videoTitle = VideoTitle;
            videoID = VideoID;
            wasDeleted = Deleted;
        }

    }
}
