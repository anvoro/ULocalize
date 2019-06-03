using System.IO;
using UnityEngine;

namespace UnityLocalize.DataStorage
{
    public class TranslationStorageProvider
    {
        public string GetStoragePath()
        {
            string streamingAssetsDir = Application.streamingAssetsPath;
            string dataStorageDir = Path.Combine(streamingAssetsDir, LocalizationConfig.DATA_STORAGE_DIRECTORY);

            return dataStorageDir;
        }
    }
}
