using System;
using System.Collections.Generic;

namespace UnityLocalize.Editor
{
    [Serializable]
    internal class InlineStringsSet
    {
        public const string INLINE_STRINGS_STOGARE = "~InlineStringsSet.json";

        public List<InlineString> InlineStrings = new List<InlineString>();
    }
}
