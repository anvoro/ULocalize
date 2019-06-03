using System;
using System.Collections.Generic;

namespace UnityLocalize.DataStorage
{
    [Serializable]
    public class TranslationInfoSet
    {
        public const string TRANSLATION_INFO_SET = "TRANSLATION_INFO_SET";

        public List<TranslationInfo> Translations = new List<TranslationInfo>();
    }
}
