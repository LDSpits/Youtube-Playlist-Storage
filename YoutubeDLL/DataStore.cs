using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;


namespace YoutubeDLL
{
    public static class DataStore
    {

        static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YTPlayListStorage");

        public static void StoreData(YTPlaylist Playlist)
        {
            if(Playlist.items != null)
            {
                using (StreamWriter Writer = File.CreateText(BuildFilePath(Playlist.Id)))
                {
                    Writer.Write(JsonConvert.SerializeObject(Playlist.items, Formatting.Indented));
                }
            }
            else
            {
                throw new ArgumentNullException("the playlist provided does not have a list of videos");
            }

            
        }

        public static YTVidList LoadData(string playlistId)
        {
            YTVidList itemList = new YTVidList();

            string file = BuildFilePath(playlistId);

            if (File.Exists(file))
            {
                using(StreamReader reader = new StreamReader(file))
                {
                    itemList = JsonConvert.DeserializeObject<YTVidList>(reader.ReadToEnd());
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
            return itemList;
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
