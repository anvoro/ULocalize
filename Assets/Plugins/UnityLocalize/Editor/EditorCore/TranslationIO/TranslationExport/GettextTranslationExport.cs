using System.IO;
using System.Text;
using PluginsEnvironment;
using UnityEngine;
using UnityLocalize.DataStorage;
using UnityLocalize.Plural;

namespace UnityLocalize.Editor
{
    internal class GettextTranslationExport : ITranslationExport
    {
        private readonly string _comment = "#. {0}";

        private readonly string _msgid = "msgid \"{0}\"";
        private readonly string _msgid_plural = "msgid_plural \"{0}\"";

        private readonly string _msgstr = "msgstr \"\"";
        private readonly string _msgstr_plural = "msgstr[{0}] \"\"";

        public void Export(TranslationInfo translationInfo, DefaultLocalizationSet defaultLocalizationSet)
        {
            string translationDirectory = Path.Combine(LocalizationEditorEnvironmentManager.TempFolder, translationInfo.TranslationName);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this._msgid.F(string.Empty));
            sb.AppendLine(this._msgstr);
            sb.AppendLine("\"Language: " + translationInfo.CultureInfo.TwoLetterISOLanguageName + "\\n\"");
            sb.AppendLine();

            int pluralsCount = PluralFormGenerator.CreateForm(translationInfo.CultureInfo).PluralsCount;

            foreach (StoredString s in defaultLocalizationSet.Strings)
            {
                sb.AppendLine(this._comment.F(s.Id));

                if (s.Strings.Length == 1)
                {
                    sb.AppendLine(this._msgid.F(s.Strings[0]));

                    sb.AppendLine(this._msgstr);
                }
                else
                {
                    sb.AppendLine(this._msgid.F(s.Strings[0]));
                    sb.AppendLine(this._msgid_plural.F(s.Strings[1]));

                    for (int i = 0; i < pluralsCount; i++)
                    {
                        sb.AppendLine(this._msgstr_plural.F(i));
                    }
                }

                sb.AppendLine();
            }

            File.WriteAllText(Path.Combine(translationDirectory, translationInfo.TranslationName + ".po"), sb.ToString());

            Debug.Log("Gettext translation export - Complete.");
        }
    }
}
