using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace YoutubeDLL.DataTypes
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class YTVidList
    {
        private List<YTVideo> _list = new List<YTVideo>();

        public static implicit operator List<YTVideo>(YTVidList list)
        {
            return list.GetCollection();
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

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
}
