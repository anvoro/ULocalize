using System;
using System.Collections.Generic;
using System.Linq;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class InlineStringsLocalizationProvider : LocalizationProviderBase
    {
        private readonly InlineStringsRepository _inlineStringsRepository;

        public InlineStringsLocalizationProvider(InlineStringsRepository inlineStringsRepository)
        {
            if(inlineStringsRepository == null)
                throw new NullReferenceException(nameof(InlineStringsRepository));

            this._inlineStringsRepository = inlineStringsRepository;
        }

        public override List<StoredString> GetLocalization()
        {
            InlineStringsSet inlineStringsSet = this._inlineStringsRepository.Get();

            return inlineStringsSet.InlineStrings
                .Cast<StoredString>()
                .ToList();
        }
    }
}
