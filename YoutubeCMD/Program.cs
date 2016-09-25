using System;
using System.Linq;
using YoutubeDLL;
using YoutubeDLL.DataTypes;

namespace YoutubeCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            GoogleConnection connection = new GoogleConnection();
            PlaylistCollection onlineColl = connection.GetPlaylists();

            if(args[0] == "init" && args.Length == 1)
            {
                string choice = Methods.choose(onlineColl, Storage.GetStoredPlaylists());

                for (int i = 0; i < onlineColl.Count; i++)
                {
                    if(choice == i.ToString())
                    {
                        Playlist list = onlineColl[i];
                        list.Items = connection.GetVideos(list);

                        Storage.Save(list);
                        break;
                    }
                }
            }

            else if(args[0] == "update" && args.Length == 1)
            {
                foreach (Playlist offline in Storage.GetStoredPlaylists())
                {
                    foreach (Playlist online in onlineColl)
                    {
                        if (offline.ID == online.ID)//the playlist is stored locally and online
                        {
                            Console.WriteLine("matching local and online playlist found: {0}\nprocessing...", online.Title);

                            //load the videos of the online list
                            online.Items = connection.GetVideos(online);

                            Playlist local = offline;
                            local.Items = Storage.Load(local.ID);

                            //update the list
                            local.UpdateWith(online);

                            //write the changes to disk
                            Storage.Save(local);
                            Console.WriteLine("done processing!\n");

                        }
                    }
                }
            }

            Console.WriteLine("done!");
            Console.ReadKey();
        }

        
    }

    static class Methods
    {
        internal static string choose(PlaylistCollection onlineCollection, PlaylistCollection offlineCollection)
        {
            //PlaylistCollection possible = new PlaylistCollection();

            //foreach (PlayList offline in offlineCollection)
            //{
            //    if (!onlineCollection.Contains(offline))
            //        possible.Add(online);
            //}

            Console.WriteLine("choose which playlist to store locally (press the appropriate numkey):");
            for (int i = 0; i < onlineCollection.Count; i++)
            {
                Console.WriteLine(i + ": " + onlineCollection[i].Title);
            }
            string pressed = RemoveWhitespace(Console.ReadLine());

            return pressed;
        }

        private static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
