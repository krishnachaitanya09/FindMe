using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSTracker.Helpers
{
    public static class StorageHelper
    {
        public static async Task Save<T>(this T obj, string file)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        //if (!store.FileExists(file))
                        //{
                        //    store.CreateFile(file);
                        //}
                        using (var fileStream = new IsolatedStorageFileStream(file, FileMode.Create, store))
                        {
                            using (var stream = new StreamWriter(fileStream))
                            {
                                stream.Write(JsonConvert.SerializeObject(obj));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        public static T Load<T>(string file)
        {

            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            T obj = Activator.CreateInstance<T>();
            if (store.FileExists(file))
            {
                try
                {
                    using (var fileStream = new IsolatedStorageFileStream(file, FileMode.Open, store))
                    {
                        using (var stream = new StreamReader(fileStream))
                        {
                            obj = JsonConvert.DeserializeObject<T>(stream.ReadToEnd());
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return obj;
        }
    }
}
