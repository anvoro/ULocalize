using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal interface ITranslationImport
    {
        Translation Import(TranslationInfo translationInfo);
    }
}
