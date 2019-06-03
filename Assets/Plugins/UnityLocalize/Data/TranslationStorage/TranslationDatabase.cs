using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityLocalize.DataStorage
{
    internal class TranslationDatabase
    {
        private readonly TranslationRepository _translationRepository;

        public TranslationDatabase(TranslationRepository translationRepository)
        {
            if (translationRepository == null)
                throw new NullReferenceException(nameof(TranslationRepository));

            this._translationRepository = translationRepository;
        }

        public Dictionary<string, string[]> GetTranslationDictionary(TranslationInfo translationInfo)
        {
            Translation translatedSet = this._translationRepository.Get(translationInfo);

            return translatedSet.Strings.ToDictionary(k => k.Id, v => v.Strings);
        }
    }
}
