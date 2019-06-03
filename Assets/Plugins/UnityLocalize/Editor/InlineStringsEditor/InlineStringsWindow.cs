using System;
using System.Linq;
using EditorDraw;
using UnityEditor;
using UnityEngine;
using UnityLocalize.Exceptions;

namespace UnityLocalize.Editor
{
    internal class InlineStringsWindow : EditorWindow
    {
        private readonly InlineStringsListView _inlineStringsListView = new InlineStringsListView();

        private InlineStringsPresenter _inlineStringsPresenter;

        private Vector2 _inlineStringsListViewScrollPos;

        private string _inlineStringAliasToAdd;
        private string _inlineStringTextToAdd;

        private bool _isInit;

        public string InlineStringAliasToAdd
        {
            get => this._inlineStringAliasToAdd;
            set => this._inlineStringAliasToAdd = value;
        }

        public string InlineStringTextToAdd
        {
            get => this._inlineStringTextToAdd;
            set => this._inlineStringTextToAdd = value;
        }

        public InlineStringsListView InlineStringsListView => this._inlineStringsListView;

        public void Init(InlineStringsPresenter inlineStringsPresenter)
        {
            if(inlineStringsPresenter == null)
                throw new NullReferenceException(nameof(InlineStringsPresenter));

            this._inlineStringsPresenter = inlineStringsPresenter;

            this._isInit = true;
        }

        private void OnDestroy()
        {
            this._inlineStringsPresenter.OnClose();
        }

        private void OnGUI()
        {
            if(!this._isInit)
                throw new NotInitializedException(typeof(InlineStringsWindow));

            EDraw.BeginDraw()
                .S_BeginVertical()

                .DrawSpace()
                .S_BeginVertical(GUI.skin.box)
                .S_DrawLabel("Alias:", style: GUIEx.LabelStyle(FontStyle.Bold)).S_TextField(ref this._inlineStringAliasToAdd)
                .DrawSpace()
                .S_DrawLabel("String:", style: GUIEx.LabelStyle(FontStyle.Bold)).S_TextField(ref this._inlineStringTextToAdd)
                .EndVertical()

                .DrawSpace()
                .S_Button("Add Inline string", this._inlineStringsPresenter.AddInlineString,
                    color: Color.gray, lossFocus: true)

                .S_Button("Generate InlineStrings.cs file", this._inlineStringsPresenter.GenerateFile,
                    color: Color.green)

                .Invoke(this.DrawStrings)

                .EndVertical();
        }

        private void DrawStrings()
        {
            if (this._inlineStringsListView.InlineStringEditors.Any())
            {
                EDraw.BeginDraw()
                    .DrawSpace(6)
                    .S_DrawLabel("Inline strings", EditorStyles.boldLabel)
                    .DrawSpace(6)
                    .S_BeginVertical(EditorStyles.helpBox)
                    .Invoke(() =>
                    {
                        using (var scrollView = new EditorGUILayout.ScrollViewScope(this._inlineStringsListViewScrollPos))
                        {
                            this._inlineStringsListViewScrollPos = scrollView.scrollPosition;
                            foreach (InlineStringEditor editor in this._inlineStringsListView.InlineStringEditors)
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