using System;
using System.Collections.Generic;
using System.IO;
using UnityLocalize.DataStorage;
using UnityLocalize.Plural;

namespace UnityLocalize.Editor
{
    internal class GettextTranslationImport : ITranslationImport
    {
        private readonly string _comment = "#.";
        private readonly string _msgstr = "msgstr";

        private int _pluralsCount;

        public Translation Import(TranslationInfo translationInfo)
        {
            string translationDirectory = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, translationInfo.TranslationName);
            string[] files = Directory.GetFiles(translationDirectory, "*.po");

            if(files.Length > 1)
                throw new Exception($"More then one PO file in directory {translationDirectory}.");

            this._pluralsCount = PluralFormGenerator.CreateForm(translationInfo.CultureInfo).PluralsCount;

            var result = new Translation();

            using (var fileStream = File.OpenRead(files[0]))
            using (var streamReader = new StreamReader(fileStream))
            {
                string line;
                while (!string.IsNullOrEmpty(streamReader.ReadLine())){}

                var unit = new List<string>();
                
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.StartsWith(this._comment))
                    {
                        unit.Clear();
                    }

                    if (line.StartsWith(this._msgstr) || line.StartsWith(this._comment))
                    {
                        unit.Add(line.Replace("\"", string.Empty));
                    }

                    if (string.IsNullOrEmpty(line))
                    {
                        this.ProcessUnit(unit, ref result);
                    }
                }

                //TODO: из за этого иногда дублируются строки в сете
                if (unit.Count > 0)
                {
                    this.ProcessUnit(unit, ref result);
                }
            }

            return result;
        }

        private void ProcessUnit(List<string> unit, ref Translation translation)
        {
            var storedString = new StoredString
            {
                Id = unit[0].Substring(3)
            };

            if (unit.Count > 2)
            {
                storedString.Strings = new string[this._pluralsCount];

                for (int i = 0; i < this._pluralsCount; i++)
                {
                    storedString.Strings[i] = unit[i + 1].Substring(10);
                }
            }
            else
            {
                storedString.Strings = new string[1];
                storedString.Strings[0] = unit[1].Substring(6);
            }

            translation.Strings.Add(storedString);
        }
    }
}
