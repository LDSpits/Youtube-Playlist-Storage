using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDLL.DataTypes
{
    public class YTplaylistColl
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
