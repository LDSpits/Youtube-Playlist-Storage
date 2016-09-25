using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDLL.DataTypes
{
    public interface IPlaylistCollection
    {
        IEnumerator GetEnumerator();

        void Add(Playlist playlist);

        Playlist this[int index]
        {
            get;
        }
    }

    public class PlaylistCollection : IPlaylistCollection
    {
        private List<Playlist> _list = new List<Playlist>();

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Add(Playlist playlist)
        {
            _list.Add(playlist);
        }

        public Playlist this[int index]
        {
            get { return _list[index]; }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool Contains(Playlist playlist)
        {
            foreach (Playlist play in _list)
            {
                if (play.ID == playlist.ID)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
