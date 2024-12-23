
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject cardPrefab;
    public Transform cardParent;
    public int gridRows = 5;
    public int gridCols = 5;
    public int lives = 3;

    private List<Card> cards = new List<Card>();
    private List<CardFlip> revealedCards = new List<CardFlip>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateCards();
        PlaceCards();
    }

    void GenerateCards()
    {
        int totalCards = gridRows * gridCols;
        int artifactCount = 12;
        int trapCount = 7;
        int healthCount = 6;

        AddCards(Card.CardType.Artifact, artifactCount);
        AddCards(Card.CardType.Trap, trapCount);
        AddCards(Card.CardType.Health, healthCount);

        cards = cards.OrderBy(x => UnityEngine.Random.value).ToList(); // Shuffle
    }

    void AddCards(Card.CardType type, int count)
    {
        for (int i = 0; i < count; i++)
        {
            cards.Add(new Card(type));
        }
    }

    void PlaceCards()
    {
        foreach (Card card in cards)
        {
            GameObject cardGO = Instantiate(cardPrefab, cardParent);
            CardFlip cardFlip = cardGO.GetComponent<CardFlip>();
            cardFlip.cardData = card;
            cardFlip.UpdateCardVisual();
        }
    }

    public void CardRevealed(CardFlip cardFlip)
    {
        revealedCards.Add(cardFlip);

        if (revealedCards.Count == 2)
        {
            CheckForMatch();
        }
    }

    void CheckForMatch()
    {
        CardFlip card1 = revealedCards[0];
        CardFlip card2 = revealedCards[1];

        if (card1.cardData.Type == card2.cardData.Type)
        {
            switch (card1.cardData.Type)
            {
                case Card.CardType.Trap:
                    LoseLife();
                    break;
                case Card.CardType.Health:
                    GainHealth();
                    MarkCardsMatched(card1, card2);
                    break;
                default:
                    MarkCardsMatched(card1, card2);
                    break;
            }
        }
        else
        {
            StartCoroutine(FlipBackCards(card1, card2));
        }

        revealedCards.Clear();
    }

    IEnumerator FlipBackCards(CardFlip card1, CardFlip card2)
    {
        yield return new WaitForSeconds(1f);
        card1.cardData.IsFaceUp = false;
        card2.cardData.IsFaceUp = false;
        card1.UpdateCardVisual();
        card2.UpdateCardVisual();
    }

    void MarkCardsMatched(CardFlip card1, CardFlip card2)
    {
        card1.cardData.IsMatched = true;
        card2.cardData.IsMatched = true;
    }

    void GainHealth()
    {
        lives++;
        Debug.Log("Health increased! Lives: " + lives);
    }

    void LoseLife()
    {
        lives--;
        Debug.Log("Life lost! Lives: " + lives);

        if (lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over! The robot is stranded forever.");
        // Implement game-over logic here
    }
}