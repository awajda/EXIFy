namespace Exify.Contracts
{
    public interface IValueParser<InType, OutType>
    {
        OutType Parse(InType value);
    }
}
