namespace UnityLocalize.Plural
{
    public interface IPluralForm
    {
        int PluralsCount { get; }

        int EvaluatePlural(int number);
    }
}