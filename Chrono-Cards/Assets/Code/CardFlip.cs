using System.Diagnostics;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    public Card cardData; // Card data assigned during initialization

    // References to sprites for each card type
    public Sprite artifactSprite;
    public Sprite trapSprite;
    public Sprite healthSprite;
    public Sprite backSprite;

    /// <summary>
    /// Called when the card is clicked.
    /// </summary>
    public void OnCardClicked()
    {
        if (!cardData.IsFaceUp && !cardData.IsMatched)
        {
            cardData.IsFaceUp = true; // Flip the card face-up
            UpdateCardVisual();      // Update the sprite to match its type
            GameManager.Instance.CardRevealed(this); // Notify GameManager
        }
    }

    /// <summary>
    /// Updates the visual representation of the card based on its state.
    /// </summary>
    public void UpdateCardVisual()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = cardData.IsFaceUp ? GetFrontSprite(cardData.Type) : backSprite;
    }

    /// <summary>
    /// Gets the front-facing sprite based on the card type.
    /// </summary>
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
                UnityEngine.Debug.LogWarning("Unexpected CardType: " + type);
                return null; // Safeguard in case of unexpected card type
        }
    }
}
