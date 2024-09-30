namespace MyApp.Domain.Factories
{
    public interface ICardFactory
    {
        Card[] GetAllCards();
        Pile GetPile();
    }
}
