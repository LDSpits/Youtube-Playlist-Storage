using System;
using System.Collections.Generic;
using YoutubeDLL;

namespace YoutubeCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            YTplaylistCollection collection = YoutubeApi.GetPlaylists();

            collection[0].items = YoutubeApi.GetVideos(collection[0].Id);

            DataStore.StoreData(collection[0]);

            //YoutubeApi.logOut();
            Console.WriteLine("program complete. press enter to exit...");
            Console.Read();
        }
    }
}
