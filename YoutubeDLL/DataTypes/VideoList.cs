using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YoutubeDLL.DataTypes
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class VideoList
    {
        protected List<Video> _list = new List<Video>();

        public List<Video> Items
        {
            get { return _list; }
            set { _list = value; }
        }

        public static implicit operator VideoList(List<Video> i) //van List<videos> naar videoList
        {
            return new VideoList(i);
        }

        public static implicit operator List<Video>(VideoList i)
        {
            return i._list;
        }

        public VideoList(List<Video> videos)
        {
            _list = videos;
        }

        public VideoList()
        {
            _list = null;
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public void Add(Video video)
        {
            if(_list != null)
                _list.Add(video);
            else
            {
                _list = new List<Video>();
                _list.Add(video);
            }
        }

        public bool Contains(Video video)
        {
            bool found = false;
            foreach (Video v in _list)
            {
                if (v.ID == video.ID)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public Video this[int i]
        {
            get { return _list[i]; }
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void UpdateWith(Playlist updates)
        {
            if(_list != null && updates != null)
            {
                foreach (Video video in updates)
                {
                    if (Contains(video) == false)
                    {
                        _list.Insert(0, video);
                        Console.WriteLine("video added: " + video.Title);
                    }
                }

                foreach (Video video in _list)
                {
                    if (updates.Contains(video) == false && video.WasDeleted == false)
                    {
                        video.WasDeleted = true;
                        Console.WriteLine("video marked as deleted: " + video.Title);
                    }
                }
            }else
            {
                throw new NullReferenceException("one of the given lists is empty!");
            }
            
        }

    }
}
