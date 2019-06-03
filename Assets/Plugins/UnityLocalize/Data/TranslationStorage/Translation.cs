using System;
using System.Collections.Generic;

namespace UnityLocalize.DataStorage
{
    [Serializable]
    public class Translation
    {
        public List<StoredString> Strings = new List<StoredString>();
    }
}
