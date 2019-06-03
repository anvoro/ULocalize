using System;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{ 
    internal class InlineString : StoredString
    {
        public string GUID;

        public InlineString(string id, string[] strings) : base(id, strings)
        {
            this.GUID = Guid.NewGuid().ToString();
        }

        public InlineString()
        {
            this.GUID = Guid.NewGuid().ToString();
        }

        public bool Equals(InlineString other)
        {
            return string.Equals(this.GUID, other.GUID);
        }
    }
}
