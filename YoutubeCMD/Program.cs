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

            //YTVidList master = DataStore.LoadData();

            //Console.WriteLine("stored data: ");
            //foreach (YTVideo video in master)
            //{
            //    Console.WriteLine("master, video: " + video.Title);
            //}

            //YTVidList update = YoutubeApi.GetVideos("[EDM] music");

            //foreach (YTVideo video in update)
            //{
            //    Console.WriteLine("update, video: " + video.Title);
            //}

            //YTplaylistCollection collection = YoutubeApi.GetPlaylists();

            //foreach (YTPlaylist playlist in collection)
            //{
            //    Console.WriteLine("playlist: " + playlist.Title);
            //}

            //DataStore.StoreData(update);

            //YoutubeApi.logOut();
            Console.Read();
        }
    }
}
