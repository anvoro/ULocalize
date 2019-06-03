using System;
using System.Globalization;

namespace UnityLocalize.DataStorage
{
    [Serializable]
    public class TranslationInfo
    {
        public string GUID;
        public string TranslationName;
        public Culture Culture;

        public TranslationInfo(string translationName, Culture culture)
        {
            this.GUID = Guid.NewGuid().ToString();
            this.TranslationName = translationName;
            this.Culture = culture;
        }

        public TranslationInfo()
        {
            this.GUID = Guid.NewGuid().ToString();
        }

        public CultureInfo CultureInfo => CultureMap.Get(this.Culture);

        public bool Equals(TranslationInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(this.GUID, other.GUID);
        }
    }
}
