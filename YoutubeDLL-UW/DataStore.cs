﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YoutubeDLL.DataTypes;

namespace YoutubeDLL
{
    public static class DataStore
    {
        static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lucas Spits", "Youtube Playlist Storage");

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
            YTVidList itemList;
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

        public static async Task<YTVidList> LoadDataAsync(string playlistId)
        {
            YTVidList itemlist;

            string file = BuildFilePath(playlistId);

            if (File.Exists(file))
            {
                using(StreamReader reader = new StreamReader(file))
                {
                    itemlist = JsonConvert.DeserializeObject<YTVidList>(await reader.ReadToEndAsync());
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
            return itemlist;
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