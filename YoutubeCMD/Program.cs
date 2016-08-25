using System;
using YoutubeDLL;
using YoutubeDLL.DataTypes;

namespace YoutubeCMD
{
    class Program
    {
        static void Main(string[] args)
        {

            doWork();
            
            Console.WriteLine("program complete. press enter to exit...");
            Console.Read();
        }

        private static async void doWork()
        {
            YTplaylistColl collection = await YoutubeApi.GetPlaylists();
            collection[0].items = await YoutubeApi.GetVideos(collection[0].Id);
            DataStore.StoreData(collection[0]);
        }

    }
}
