using System.Collections.Generic;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal interface ITranslationExport
    {
        void Export(TranslationInfo translationInfo, DefaultLocalizationSet defaultLocalizationSet);
    }
}