using System;
using EditorDraw;
using UnityEditor;
using UnityEngine;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class TranslationInfoEditor
    {
        private readonly TranslationInfoPresenter _translationInfoPresenter;

        private string _newTranslationName;
        private Culture _newCulture;

        private TranslationInfo _translationInfo;

        public TranslationInfoEditor(TranslationInfo translationInfo, TranslationInfoPresenter translationInfoPresenter)
        {
            if (translationInfoPresenter == null)
                throw new NullReferenceException(nameof(translationInfoPresenter));

            if (translationInfo == null)
                throw new NullReferenceException(nameof(translationInfo));

            this._translationInfoPresenter = translationInfoPresenter;
            this._translationInfo = translationInfo;

            this._newTranslationName = translationInfo.TranslationName;
            this._newCulture = translationInfo.Culture;
        }

        public string NewTranslationName
        {
            get => this._newTranslationName;
            set => this._newTranslationName = value;
        }

        public Culture NewCulture
        {
            get => this._newCulture;
            set => this._newCulture = value;
        }

        public TranslationInfo TranslationInfo
        {
            get => this._translationInfo;
            set
            {
                this._translationInfo = value;
                this.Repaint();
            }
        }

        public void Draw()
        {
            EDraw.BeginDraw()
                .S_BeginHorizontal(EditorStyles.helpBox)

                .S_BeginVertical()
                .S_DrawLabel("Translation name:", style: GUIEx.LabelStyle(FontStyle.Bold)).S_TextField(ref this._newTranslationName)
                .S_DrawLabel("Culture:", style: GUIEx.LabelStyle(FontStyle.Bold)).S_DrawEnumPopup(ref this._newCulture)
                .EndVertical()

                .BeginVertical(width: 80)
                .S_BeginHorizontal()
                .Button("Delete", this.Delete, color: Color.red, height: 32, width: 72)
                .EndHorizontal()
                .S_BeginHorizontal()
                .Button("Export", this.Export, height: 32, width: 50)
                .Button("Import", this.Import, height: 32, width: 50)
                .EndHorizontal()
                .EndVertical()

                .EndHorizontal();
        }

        private void Delete()
        {
            this._translationInfoPresenter.DeleteTranslation(this);
        }

        private void Import()
        {
            this._translationInfoPresenter.DoImport(this._translationInfo);
        }

        private void Export()
        {
            this._translationInfoPresenter.DoExport(this._translationInfo);
        }

        private void Repaint()
        {
            this._newTranslationName = this._translationInfo?.TranslationName;

            if (this._translationInfo != null)
            {
                this._newCulture = this._translationInfo.Culture;
            }
        }
    }
}
