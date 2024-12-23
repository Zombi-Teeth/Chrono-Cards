public class Card
{
    public enum CardType { Artifact, Trap, Health }

    public CardType Type { get; private set; }
    public bool IsFaceUp { get; set; }
    public bool IsMatched { get; set; }

    public Card(CardType type)
    {
        Type = type;
        IsFaceUp = false;
        IsMatched = false;
    }
}