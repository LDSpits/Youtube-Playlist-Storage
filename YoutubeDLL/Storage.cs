using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YoutubeDLL.DataTypes;
using System.Linq;

namespace YoutubeDLL
{

    public static class Storage 
    {
        static internal readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Lucas Spits", "Youtube Playlist Storage");

        public static void Save(Playlist playlist)
        {
            if(playlist.Count > 0)
            {
                using (StreamWriter Writer = File.CreateText(BuildFilePath(playlist.ID)))
                {
                    Writer.Write(JsonConvert.SerializeObject(playlist, Formatting.Indented));
                }
            }
            else
            {
                throw new ArgumentNullException("the playlist provided does not have a list of videos");
            }
        }

        public static Playlist Load(string Id)
        {
            Playlist itemList;
            string file = BuildFilePath(Id);

            if (File.Exists(file))
            {
                using(StreamReader reader = new StreamReader(file))
                {
                    itemList = JsonConvert.DeserializeObject<Playlist>(reader.ReadToEnd());
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
            return itemList;
        }

        public static async Task<Playlist> LoadAsync(string Id)
        {
            Playlist itemlist;

            string file = BuildFilePath(Id);

            if (File.Exists(file))
            {
                using(StreamReader reader = new StreamReader(file))
                {
                    itemlist = JsonConvert.DeserializeObject<Playlist>(await reader.ReadToEndAsync());
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
            return itemlist;
        }

        public static PlaylistCollection GetStoredPlaylists()
        {
            foreach (string str in Directory.GetFiles(DataPath))
                Console.WriteLine(str);

            List<string> ids = Directory.GetFiles(DataPath)
            .Select(path => Path.GetFileNameWithoutExtension(path))
            .ToList<string>();

            PlaylistCollection coll = new PlaylistCollection();

            foreach (string str in ids)
                coll.Add(YoutubeApi.getPlaylist(str));

            return coll;
        }

        private static string BuildFilePath(string PlayListId)
        {
            if(Directory.Exists(DataPath) == false)
            {
                Directory.CreateDirectory(DataPath);
            }
            return Path.Combine(DataPath, PlayListId + ".lst");    
        }
    }
}
