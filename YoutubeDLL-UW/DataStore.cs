using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeDLL.DataTypes;
using Windows.Storage;
using Windows.Storage.Streams;

namespace YoutubeDLL
{
    public class DataStore
    {
        static readonly string DataPath = Path.Combine(ApplicationData.Current.LocalFolder.ToString(), "Lucas Spits", "Youtube Playlist Storage");       

        public static async void StoreData(YTPlaylist Playlist)
        {
            if(Playlist.items != null)
            {
                using (StreamWriter Writer = new StreamWriter(await CreateFile(Playlist.Id).Result.OpenStreamForWriteAsync()))
                {
                    Writer.Write(JsonConvert.SerializeObject(Playlist.items, Formatting.Indented));
                }
            }
            else
            {
                throw new ArgumentNullException("The playlist provided does not have a list of videos");
            }
        }

        public async static Task<YTVidList> LoadData(string playlistId)
        {
            YTVidList itemList;
            StorageFile file = await getFile(playlistId);

            if (file != null)
            {
                using(StreamReader reader = new StreamReader(await file.OpenStreamForReadAsync()))
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

        private static async Task<StorageFile> getFile(string PlayListId)
        {
            StorageFolder dataFolder = await StorageFolder.GetFolderFromPathAsync(DataPath);
            return (StorageFile)await dataFolder.TryGetItemAsync(PlayListId);
        }

        private static async Task<StorageFile> CreateFile(string PlayListId)
        {
            StorageFolder DataFolder = await StorageFolder.GetFolderFromPathAsync(DataPath);
            return await DataFolder.CreateFileAsync(PlayListId, CreationCollisionOption.OpenIfExists);
        }
    }
}
