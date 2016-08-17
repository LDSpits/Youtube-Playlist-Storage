using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;

namespace YoutubeDLL
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

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class YTPlaylist
    {
        private string playlistTitle;
        private string playlistId;
        private YTVidList internalVideos;
        
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

        public YTVidList items
        {
            get { return internalVideos; }
            set { internalVideos = value; }
        }

        public YTPlaylist(string Title, string Id)
        {
            playlistTitle = Title;
            playlistId = Id;
            internalVideos = null;
        }

        public YTPlaylist(string Title, string Id, YTVidList Videos)
        {
            playlistTitle = Title;
            playlistId = Id;
            internalVideos = Videos;
        }

    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class YTVidList
    {
        private List<YTVideo> _list = new List<YTVideo>();

        public List<YTVideo> GetCollection()
        {
            return _list;
        }

        public void Add(YTVideo video)
        {
            _list.Add(video);
        }

        public bool Contains(YTVideo video)
        {
            bool found = false;
            foreach (YTVideo v in _list)
            {
                if (v.Id == video.Id)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public YTVideo this[int i]
        {
            get { return _list[i]; }
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void UpdateWith(YTVidList updates)
        {
            foreach (YTVideo video in updates)
            {
                if (Contains(video) == false)
                {
                    _list.Insert(0, video);
                    Console.WriteLine("video added: " + video.Title);
                }
            }

            foreach (YTVideo video in _list)
            {
                if (updates.Contains(video) == false && video.WasDeleted == false)
                {
                    video.WasDeleted = true;
                    Console.WriteLine("video marked as deleted: " + video.Title);
                }
            }
        }

    }

    public class YTplaylistCollection
    {
        private List<YTPlaylist> _list = new List<YTPlaylist>();

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Add(YTPlaylist playlist)
        {
            _list.Add(playlist);
        }

        public List<YTPlaylist> GetCollection()
        {
            return _list;
        }

        public YTPlaylist this[int i]
        {
            get { return _list[i]; }
        }

    }
}

