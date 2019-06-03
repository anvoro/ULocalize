using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PluginsEditorEnvironment;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor.Validation
{
    internal class InlineStringValidator : IValidator<StoredString>
    {
        public bool Validate(StoredString validatableObject, out string error)
        {
            bool result = true;
            error = null;

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(validatableObject.Id))
            {
                result = false;

                sb.AppendLine("- Alias can't be empty.");
            }
            else
            {
                if (!Regex.IsMatch(validatableObject.Id, "^[a-zA-Z_]+$"))
                {
                    result = false;

                    sb.AppendLine("- Alias must contain only letters and underscore");
                }
            }

            if (validatableObject.Strings == null || validatableObject.Strings.Any(string.IsNullOrEmpty))
            {
                result = false;

                sb.AppendLine("- Strings can't be empty.");
            }

            if (!result)
                error = sb.ToString();

            return result;
        }
    }
}
