using UnityEngine;

public class CardFlip : MonoBehaviour
{
    public Card cardData; // Assigned during initialization

    // Reference to sprites for each card type
    public Sprite artifactSprite;
    public Sprite trapSprite;
    public Sprite healthSprite;
    public Sprite backSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (!cardData.IsFaceUp && !cardData.IsMatched)
        {
            OnCardClicked();
        }
    }

    public void OnCardClicked()
    {
        cardData.IsFaceUp = true;
        UpdateCardVisual();
        GameManager.Instance.CardRevealed(this);
    }

    public void UpdateCardVisual()
    {
        spriteRenderer.sprite = cardData.IsFaceUp ? GetFrontSprite(cardData.Type) : backSprite;
    }

    private Sprite GetFrontSprite(Card.CardType type)
    {
        switch (type)
        {
            case Card.CardType.Artifact:
                return artifactSprite;
            case Card.CardType.Trap:
                return trapSprite;
            case Card.CardType.Health:
                return healthSprite;
            default:
                return null;
        }
    }
}