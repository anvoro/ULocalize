using System;
using System.Globalization;

namespace UnityLocalize.DataStorage
{
    internal static class CultureMap
    {
        public static CultureInfo Get(Culture culture)
        {
            switch (culture)
            {
                case Culture.en_GB:
                    return new CultureInfo("en-GB");

                case Culture.en_US:
                    return new CultureInfo("en-US");

                case Culture.ru_RU:
                    return new CultureInfo("ru-RU");

                case Culture.fr_FR:
                    return new CultureInfo("fr-FR");

                case Culture.de_DE:
                    return new CultureInfo("de-DE");

                default:
                    throw new ArgumentOutOfRangeException(nameof(culture), culture, null);
            }
        }
    }
}
