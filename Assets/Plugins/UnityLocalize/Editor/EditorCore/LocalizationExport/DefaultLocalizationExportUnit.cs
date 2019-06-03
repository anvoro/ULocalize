using System;
using System.Collections.Generic;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class DefaultLocalizationExportUnit
    {
        private readonly LocalizationProviderCache _localizationProviderCache = new LocalizationProviderCache();

        private readonly DefaultLocalizationRepository _defaultLocalizationRepository;

        private readonly ScenesLocalizationProvider _scenesLocalizationProvider;
        private readonly AssetsLocalizationProvider _assetsLocalizationProvider;
        private readonly InlineStringsLocalizationProvider _inlineStringsLocalizationProvider;

        private DefaultLocalizationSet _defaultLocalizationSet;
        private List<Type> _typesToLocalization;

        public DefaultLocalizationExportUnit(DefaultLocalizationRepository defaultLocalizationRepository, EditorConfigRepository editorConfigRepository, InlineStringsRepository inlineStringsRepository)
        {
            if(defaultLocalizationRepository == null)
                throw new NullReferenceException(nameof(DefaultLocalizationRepository));

            if (editorConfigRepository == null)
                throw new NullReferenceException(nameof(EditorConfigRepository));

            if (inlineStringsRepository == null)
                throw new NullReferenceException(nameof(InlineStringsRepository));

            this._defaultLocalizationRepository = defaultLocalizationRepository;

            this._scenesLocalizationProvider = new ScenesLocalizationProvider(editorConfigRepository.GetInternalConfig());
            this._assetsLocalizationProvider = new AssetsLocalizationProvider();
            this._inlineStringsLocalizationProvider = new InlineStringsLocalizationProvider(inlineStringsRepository);
        }

        public void SetTypesToLocalization()
        {
            this._typesToLocalization = ReflectionHelper
                .GetMarkedTypesFromAssembly(typeof(LocalizableAttribute), typeof(UnityEngine.Object));
        }

        public void GetScenesLocalization()
        {
            this.BeforeGetSetUp(this._scenesLocalizationProvider);

            List<StoredString> strings = this._scenesLocalizationProvider.GetLocalization();

            this.SaveDefaultLocalizationSet(strings);
        }

        public void GetAssetsLocalization()
        {
            this.BeforeGetSetUp(this._assetsLocalizationProvider);

            List<StoredString> strings = this._assetsLocalizationProvider.GetLocalization();

            this.SaveDefaultLocalizationSet(strings);
        }

        public void GetInlineStringsLocalization()
        {
            this.BeforeGetSetUp(this._inlineStringsLocalizationProvider);

            List<StoredString> strings = this._inlineStringsLocalizationProvider.GetLocalization();

            this.SaveDefaultLocalizationSet(strings);
        }

        private void SaveDefaultLocalizationSet(List<StoredString> strings)
        {
            foreach (StoredString s in strings)
            {
                this._defaultLocalizationSet.Add(s);
            }

            this._defaultLocalizationRepository.Save(this._defaultLocalizationSet);
        }

        private void BeforeGetSetUp(LocalizationProviderBase localizationProvider)
        {
            if (this._defaultLocalizationSet == null)
                this._defaultLocalizationSet = this._defaultLocalizationRepository.Get();

            if (this._typesToLocalization == null)
                this.SetTypesToLocalization();

            localizationProvider.TypesToLocalization = this._typesToLocalization;
            localizationProvider.LocalizationProviderCache = this._localizationProviderCache;
        }
    }
}
