using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Card Setup")]
    public GameObject cardPrefab;
    public Transform cardParent;
    public Sprite artifactSprite;
    public Sprite trapSprite;
    public Sprite healthSprite;
    public Sprite backSprite;

    [Header("Grid Settings")]
    public int rows = 5;
    public int cols = 5;

    private List<Card> cards = new List<Card>();
    private CardFlip firstRevealedCard;
    private CardFlip secondRevealedCard;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GenerateCardData();
        GenerateCards();
    }

    private void GenerateCardData()
    {
        int totalCards = rows * cols;

        int artifactCount = 12;
        int trapCount = 10;
        int healthCount = 3;

        if (artifactCount + trapCount + healthCount != totalCards)
        {
            UnityEngine.Debug.LogError("Card type counts do not add up to total grid size.");
            return;
        }

        AddCards(Card.CardType.Artifact, artifactCount);
        AddCards(Card.CardType.Trap, trapCount);
        AddCards(Card.CardType.Health, healthCount);

        // Shuffle cards
        cards = cards.OrderBy(c => UnityEngine.Random.value).ToList();
    }

    private void AddCards(Card.CardType type, int count)
    {
        for (int i = 0; i < count; i++)
        {
            cards.Add(new Card(type));
        }
    }

    private void GenerateCards()
    {
        float spacing = 1.5f;

        if (cards.Count != rows * cols)
        {
            UnityEngine.Debug.LogError($"Mismatch between cards ({cards.Count}) and grid size ({rows * cols}).");
            return;
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // Instantiate card prefab
                GameObject cardGameObject = Instantiate(cardPrefab);
                cardGameObject.transform.SetParent(cardParent, false);

                // Position card in grid
                cardGameObject.transform.localPosition = new Vector3(x * spacing, y * spacing, 0);

                // Assign card data
                int index = x + y * cols;
                Card cardData = cards[index];
                CardFlip cardFlip = cardGameObject.GetComponent<CardFlip>();
                cardFlip.cardData = cardData;

                // Assign sprites
                cardFlip.artifactSprite = artifactSprite;
                cardFlip.trapSprite = trapSprite;
                cardFlip.healthSprite = healthSprite;
                cardFlip.backSprite = backSprite;

                // Update visuals to start with back face
                cardFlip.UpdateCardVisual();
            }
        }
    }

    public void CardRevealed(CardFlip card)
    {
        if (firstRevealedCard == null)
        {
            firstRevealedCard = card;
        }
        else if (secondRevealedCard == null)
        {
            secondRevealedCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private System.Collections.IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstRevealedCard.cardData.Type == secondRevealedCard.cardData.Type)
        {
            firstRevealedCard.cardData.IsMatched = true;
            secondRevealedCard.cardData.IsMatched = true;
        }
        else
        {
            firstRevealedCard.cardData.IsFaceUp = false;
            secondRevealedCard.cardData.IsFaceUp = false;
            firstRevealedCard.UpdateCardVisual();
            secondRevealedCard.UpdateCardVisual();
        }

        firstRevealedCard = null;
        secondRevealedCard = null;
    }
}