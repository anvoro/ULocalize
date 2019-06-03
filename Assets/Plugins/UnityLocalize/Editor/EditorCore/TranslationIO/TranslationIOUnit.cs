using System;
using System.Linq;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class TranslationIOUnit
    {
        private readonly DefaultLocalizationRepository _defaultLocalizationRepository;
        private readonly EditorTranslationIORepository _editorTranslationRepository;

        private readonly ITranslationExport _translationExport;
        private readonly ITranslationImport _translationImport;
        
        public TranslationIOUnit(EditorInternalConfig config, DefaultLocalizationRepository defaultLocalizationRepository, EditorTranslationIORepository editorTranslationRepository)
        {
            if (defaultLocalizationRepository == null)
                throw new ArgumentNullException(nameof(DefaultLocalizationRepository));

            if (editorTranslationRepository == null)
                throw new ArgumentNullException(nameof(EditorTranslationIORepository));

            if (config == null)
                throw new ArgumentNullException(nameof(EditorInternalConfig));

            this._defaultLocalizationRepository = defaultLocalizationRepository;
            this._editorTranslationRepository = editorTranslationRepository;

            var exporterType = ReflectionHelper
                .GetAllSubtypes(typeof(ITranslationExport))
                .First(e => e.Name == config.TranslationExportProvider);
            this._translationExport = (ITranslationExport)Activator.CreateInstance(exporterType);

            var importerType = ReflectionHelper
                .GetAllSubtypes(typeof(ITranslationImport))
                .First(e => e.Name == config.TranslationImportProvider);
            this._translationImport = (ITranslationImport)Activator.CreateInstance(importerType);
        }

        public void DoExport(TranslationInfo translationInfo)
        {
            DefaultLocalizationSet defaultSet = this._defaultLocalizationRepository.Get();

            this._translationExport.Export(translationInfo, defaultSet);
        }

        public void DoImport(TranslationInfo translationInfo)
        {
            Translation translation = this._translationImport.Import(translationInfo);

            this._editorTranslationRepository.Save(translationInfo, translation);
        }
    }
}
