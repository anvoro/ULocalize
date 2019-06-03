using System;
using System.Linq;
using EditorDraw;
using UnityEditor;
using UnityEngine;
using UnityLocalize.DataStorage;
using UnityLocalize.Exceptions;

namespace UnityLocalize.Editor
{
    internal class TranslationInfoWindow : EditorWindow
    {
        private readonly TranslationInfoListView _translationInfoListView = new TranslationInfoListView();

        private TranslationInfoPresenter _translationInfoPresenter;

        private Vector2 _translationInfoListViewScrollPos;

        private string _translationNameToAdd;
        private Culture _cultureToAdd;

        private bool _isInit;

        public string TranslationNameToAdd
        {
            get => this._translationNameToAdd;
            set => this._translationNameToAdd = value;
        }

        public Culture CultureToAdd
        {
            get => this._cultureToAdd;
            set => this._cultureToAdd = value;
        }

        public TranslationInfoListView InfoListView => this._translationInfoListView;

        public void Init(TranslationInfoPresenter presenter)
        {
            if (presenter == null)
                throw new ArgumentNullException(nameof(TranslationInfoPresenter));

            this._translationInfoPresenter = presenter;

            this._isInit = true;
        }

        private void OnGUI()
        {
            if (!this._isInit)
                throw new NotInitializedException(typeof(TranslationInfoWindow));

            EDraw.BeginDraw()
                .DrawSpace()
                .S_TextField(ref this._translationNameToAdd, "Translation name: ")
                .S_DrawEnumPopup(ref this._cultureToAdd, "Culture: ");

            EDraw.BeginDraw()
                .DrawSpace(14)
                .S_Button("Add translation", this._translationInfoPresenter.AddTranslation,
                    color: Color.green, lossFocus: true)
                .Invoke(this.DrawTranslations);
        }

        private void DrawTranslations()
        {
            if (this._translationInfoListView.TranslationInfoEditors.Any())
            {
                EDraw.BeginDraw()
                    .DrawSpace(6)
                    .S_DrawLabel("Translations", EditorStyles.boldLabel)
                    .DrawSpace(6)
                    .S_BeginVertical(EditorStyles.helpBox)
                    .Invoke(() =>
                    {
                        using (var scrollView = new EditorGUILayout.ScrollViewScope(this._translationInfoListViewScrollPos))
                        {
                            this._translationInfoListViewScrollPos = scrollView.scrollPosition;
                            foreach (TranslationInfoEditor editor in this._translationInfoListView.TranslationInfoEditors)
                            {
                                editor.Draw();
                            }
                        }
                    })
                    .EndVertical();
            }
        }
    }
}
