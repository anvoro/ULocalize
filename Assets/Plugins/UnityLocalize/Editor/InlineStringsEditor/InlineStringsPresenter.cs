using System;
using System.Linq;
using PluginsEditorEnvironment;
using UnityEditor;
using UnityLocalize.DataStorage;
using UnityLocalize.Editor.Validation;

namespace UnityLocalize.Editor
{
    internal class InlineStringsPresenter : IPresenter
    {
        private readonly InlineStringValidator _inlineStringValidator = new InlineStringValidator();

        private readonly InlineStringsRepository _inlineStringsRepository;

        private InlineStringsWindow _inlineStringsWindow;

        public InlineStringsPresenter(InlineStringsRepository inlineStringsRepository)
        {
            if (inlineStringsRepository == null)
                throw new ArgumentNullException(nameof(InlineStringsRepository));

            this._inlineStringsRepository = inlineStringsRepository;
        }

        public void Run()
        {
            this._inlineStringsWindow = EditorWindow.GetWindow<InlineStringsWindow>("Inline strings editor");
            this._inlineStringsWindow.Init(this);

            foreach (InlineString inlineString in this._inlineStringsRepository.Get().InlineStrings)
            {
                this._inlineStringsWindow.InlineStringsListView.AddInlineString(inlineString, this);
            }
        }

        public void OnClose()
        {
            this._inlineStringsRepository.Save();
        }

        public void GenerateFile()
        {
            InlineStringsCodeGen.Generate(this._inlineStringsRepository.Get());
        }

        public void AddInlineString()
        {
            var newString = new InlineString(this._inlineStringsWindow.InlineStringAliasToAdd, new[] { this._inlineStringsWindow.InlineStringTextToAdd });

            if (!this._inlineStringValidator.Validate(newString, out string error))
            {
                EditorUtility.DisplayDialog("Inline string adding error", error, "OK");

                return;
            }

            if (this._inlineStringsRepository.IsContain(this._inlineStringsWindow.InlineStringAliasToAdd))
            {
                EditorUtility.DisplayDialog("Inline string adding error",
                    $"Inline string with Alias: '{this._inlineStringsWindow.InlineStringAliasToAdd}' already stored.", "OK");

                return;
            }

            this._inlineStringsRepository.AddInlineString(newString);

            this._inlineStringsWindow.InlineStringsListView.AddInlineString(newString, this);

            this._inlineStringsWindow.InlineStringAliasToAdd = string.Empty;
            this._inlineStringsWindow.InlineStringTextToAdd = string.Empty;
        }

        public void DeleteInlineString(InlineStringEditor editor)
        {
            if (!EditorUtility.DisplayDialog("Delete Inline string",
                $"Delete Inline string with Alias: '{editor.InlineString.Id}'?", "OK", "Cancel"))
                return;

            this._inlineStringsRepository.RemoveInlineString(editor.InlineString);
            this._inlineStringsWindow.InlineStringsListView.RemoveInlineString(editor.InlineString);
        }

        public void UpdateInlineString(InlineStringEditor editor)
        {
            if (editor.InlineString.Id == editor.NewInlineStringAlias && editor.InlineString.Strings.SequenceEqual(editor.NewInlineStringStrings))
            {
                EditorUtility.DisplayDialog("Inline string update error.", "Inline string must be changed, before save.", "OK");

                return;
            }

            string oldAlias = editor.InlineString.Id;
            string[] oldStrings = editor.InlineString.Strings;

            editor.InlineString.Id = editor.NewInlineStringAlias;
            editor.InlineString.Strings = editor.NewInlineStringStrings;

            if (!this._inlineStringValidator.Validate(editor.InlineString, out string error))
            {
                EditorUtility.DisplayDialog("Inline string update error.", error, "OK");

                editor.InlineString.Id = oldAlias;
                editor.InlineString.Strings = oldStrings;

                return;
            }

            this._inlineStringsRepository.UpdateInlineString(editor.InlineString);
        }

        public void AddPluralForm(InlineStringEditor editor)
        {
            string[] strings = editor.NewInlineStringStrings;

            editor.NewInlineStringStrings = new string[strings.Length + 1];
            Array.Copy(strings, editor.NewInlineStringStrings, strings.Length);
        }

        public void RemovePluralForm(InlineStringEditor editor)
        {
            string[] strings = editor.NewInlineStringStrings;

            if (strings.Length == 1)
                return;

            editor.NewInlineStringStrings = new string[strings.Length - 1];
            Array.Copy(strings, editor.NewInlineStringStrings, editor.NewInlineStringStrings.Length);
        }
    }
}
