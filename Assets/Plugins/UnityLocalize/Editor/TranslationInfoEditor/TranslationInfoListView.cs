using System.Collections.Generic;
using System.Linq;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class TranslationInfoListView
    {
        private readonly List<TranslationInfoEditor> _translationInfoEditorsPool = new List<TranslationInfoEditor>();

        public IEnumerable<TranslationInfoEditor> TranslationInfoEditors
        {
            get { return this._translationInfoEditorsPool.Where(e => e.TranslationInfo != null); }
        }

        public void AddTranslation(TranslationInfo translationInfo, TranslationInfoPresenter translationInfoPresenter)
        {
            TranslationInfoEditor freeEditor =
                this._translationInfoEditorsPool.FirstOrDefault(e => e.TranslationInfo == null);

            if (freeEditor == null)
            {
                this._translationInfoEditorsPool.Add(new TranslationInfoEditor(translationInfo,
                    translationInfoPresenter));
            }
            else
            {
                freeEditor.TranslationInfo = translationInfo;
            }
        }

        public void RemoveTranslation(TranslationInfo translationInfo)
        {
            TranslationInfoEditor editor =
                this._translationInfoEditorsPool.First(e => e.TranslationInfo.Equals(translationInfo));

            editor.TranslationInfo = null;
        }
    }
}