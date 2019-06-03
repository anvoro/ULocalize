using System;
using System.Globalization;

namespace UnityLocalize.Plural
{
    public static class PluralFormGenerator
	{
        private class PluralForm : IPluralForm
        {
            private readonly Func<int, int> _evaluator;
            private int _numPlurals;

            public int PluralsCount
            {
                get => this._numPlurals;
                private set => this._numPlurals = value;
            }

            public PluralForm(int numPlurals, Func<int, int> evaluator)
            {
                if (numPlurals <= 0)
                    throw new ArgumentOutOfRangeException(nameof(numPlurals));

                if (evaluator == null)
                    throw new ArgumentNullException(nameof(evaluator));

                this.PluralsCount = numPlurals;
                this._evaluator = evaluator;
            }

            public int EvaluatePlural(int number)
            {
                return this._evaluator.Invoke(number);
            }
        }

        public static IPluralForm CreateForm(CultureInfo cultureInfo)
		{
			var langCode = cultureInfo.TwoLetterISOLanguageName;

			switch (langCode)
			{
				case "de":
				case "en":
					return new PluralForm(2, number => (number == 1) ? 0 : 1);

				case "fr":
					return new PluralForm(2, number => ((number == 0) || (number == 1)) ? 0 : 1);

				case "ru":
					return new PluralForm(3, number => ((number % 10 == 1) && (number % 100 != 11)) ? 0 : (((number % 10 >= 2) && (number % 10 <= 4) && ((number % 100 < 10) || (number % 100 >= 20))) ? 1 : 2));

                default:
                    throw new ArgumentOutOfRangeException(nameof(cultureInfo), cultureInfo, null);
			}
		}
	}
}