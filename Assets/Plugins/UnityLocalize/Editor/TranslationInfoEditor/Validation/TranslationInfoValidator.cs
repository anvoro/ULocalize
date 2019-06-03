using System.Text;
using System.Text.RegularExpressions;
using PluginsEditorEnvironment;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class TranslationInfoValidator : IValidator<TranslationInfo>
    {
        public bool Validate(TranslationInfo validatableObject, out string error)
        {
            error = null;

            bool result = true;

            StringBuilder sb = new StringBuilder();

            if (!Regex.IsMatch(validatableObject.TranslationName, "^[^<>:\"\\|?*/]+$"))
            {
                sb.AppendLine("- Name contains forbidden characters.");

                result = false;
            }

            if (!Regex.IsMatch(validatableObject.TranslationName, "^[0-9A-Za-z]+$"))
            {
                sb.AppendLine("- Only english letters and numbers are allowed.");

                result = false;
            }

            if (!result)
                error = sb.ToString();

            return result;
        }
    }
}
