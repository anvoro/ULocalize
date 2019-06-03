using System;
using System.Linq;

namespace UnityLocalize.DataStorage
{
    [Serializable]
    public class StoredString
    {
        public string Id;
        public string[] Strings;

        public StoredString(string id, string[] strings)
        {
            this.Strings = strings;
            this.Id = id;
        }

        public StoredString() {}

        public bool Equals(StoredString other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(this.Id, other.Id) && this.Strings.SequenceEqual(other.Strings);
        }
    }
}
