using System;
using System.Linq;
using EditorDraw;
using UnityEditor;
using UnityEngine;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class InlineStringEditor
    {
        private readonly InlineStringsPresenter _inlineStringsPresenter;

        private string _newInlineStringAlias;
        private string[] _newInlineStringStrings;

        private InlineString _inlineString;

        public InlineStringEditor(InlineString inlineString, InlineStringsPresenter inlineStringsPresenter)
        {
            if (inlineStringsPresenter == null)
                throw new NullReferenceException(nameof(inlineStringsPresenter));

            if (inlineString == null)
                throw new NullReferenceException(nameof(inlineString));

            this._inlineStringsPresenter = inlineStringsPresenter;
            this._inlineString = inlineString;
            
            this._newInlineStringAlias = inlineString.Id;
            this._newInlineStringStrings = inlineString.Strings.ToArray();
        }

        public string NewInlineStringAlias
        {
            get => this._newInlineStringAlias;
            set => this._newInlineStringAlias = value;
        }

        public string[] NewInlineStringStrings
        {
            get => this._newInlineStringStrings;
            set => this._newInlineStringStrings = value;
        }

        public InlineString InlineString
        {
            get => this._inlineString;
            set
            {
                this._inlineString = value;
                this.Repaint();
            }
        }

        public void Draw()
        {
            EDraw.BeginDraw()
                .S_BeginHorizontal(EditorStyles.helpBox)

                .S_BeginVertical()
                .S_DrawLabel("Alias:", style: GUIEx.LabelStyle(FontStyle.Bold)).S_TextField(ref this._newInlineStringAlias)
                .DrawSpace()
                .Invoke(this.DrawStrings)
                .EndVertical()

                .BeginVertical(width: 80)
                .S_BeginHorizontal()
                .Button("Apply", this.Apply, color: Color.green, height: 34, width: 48)
                .Button("Delete", this.Delete, color: Color.red, height: 34, width: 48)
                .EndHorizontal()
                .S_BeginHorizontal()
                .Button("+PF", this.AddPlural, height: 34, width: 48)
                .Button("-PF", this.RemovePlural, height: 34, width: 48)
                .EndHorizontal()
                .EndVertical()

                .EndHorizontal();
        }

        private void DrawStrings()
        {
            for (int i = 0; i < this._newInlineStringStrings.Length; i++)
            {
                EDraw.BeginDraw()
                    .S_DrawLabel(i == 0 ? "Single:" : $"Plural {i}:", style: GUIEx.LabelStyle(FontStyle.Bold))
                    .S_TextField(ref this._newInlineStringStrings[i]);
            }
        }

        private void Repaint()
        {
            this._newInlineStringStrings = this._inlineString?.Strings;
            this._newInlineStringAlias = this._inlineString?.Id;
        }

        private void Apply()
        {
            this._inlineStringsPresenter.UpdateInlineString(this);
        }

        private void Delete()
        {
            this._inlineStringsPresenter.DeleteInlineString(this);
        }

        private void AddPlural()
        {
            this._inlineStringsPresenter.AddPluralForm(this);
        }

        private void RemovePlural()
        {
            this._inlineStringsPresenter.RemovePluralForm(this);
        }
    }
}
