using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamer : NPC
{
    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        talks.Add("Hmmm...");
        talks.Add("Wait, Wait. Don't Get in the Way");
        talks.Add("It's not Bad");
        talks.Add("Actually Good Hands");
        talks.Add("Come on...Come on...!");
    }

    public override void Talk(Transform talker)
    {
        StopAllCoroutines();
        talkClouds.SetText(talks[Random.Range(0, talks.Count)]);
        talkClouds.gameObject.SetActive(true);
        StartCoroutine(TalkRoutine());
    }

    public override void Interact()
    {
        base.Interact();
    }

    enum CardPattern { Spade, Diamond, Heart, Clover }
    Queue<Card> deck, usedDeck;
    
    struct Card 
    { 
        public CardPattern cardPattern; 
        public int cardNum; 

        public Card(CardPattern _cardPattern, int _cardNum)
        {
            cardPattern = _cardPattern;
            cardNum = _cardNum;
        }
    }

    void CreateDeck()
    {
        deck = new();
        usedDeck = new();

        List<Card> cardList = new();
        for(int i = 1; i <- 13; i++)
        {
            cardList.Add(new Card(CardPattern.Spade, i));
            cardList.Add(new Card(CardPattern.Diamond, i));
            cardList.Add(new Card(CardPattern.Heart, i));
            cardList.Add(new Card(CardPattern.Clover, i));
        }

        while(cardList.Count > 0)
        {
            int cardNum = Random.Range(0, cardList.Count);
            deck.Enqueue(cardList[cardNum]);
            cardList.RemoveAt(cardNum);
        }
    }

    Card DrawCard()
    {
        return deck.Dequeue();
    }

    void UseCard(Card card)
    {
        usedDeck.Enqueue(card);
    }

    void RecycleUsedDeck()
    {
        List<Card> cardList = new();
        while(usedDeck.Count > 0)
        {
            cardList.Add((Card)usedDeck.Dequeue());
        }

        while (cardList.Count > 0)
        {
            int cardNum = Random.Range(0, cardList.Count);
            deck.Enqueue(cardList[cardNum]);
            cardList.RemoveAt(cardNum);
        }
    }

    List<int> CalculateHand(List<Card> hand)
    {
        List<int> result = new() { 0 };

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].cardNum == 1)
            {
                int resultCount = result.Count;
                for (int j = 0; j < resultCount; j++)
                {
                    result[j] += 1;
                    result.Add(result[j] + 10);
                }
            }
            else
            {
                for(int j = 0; j < result.Count; j++)
                {
                    result[j] += hand[i].cardNum;
                }
            }
        }

        return result;
    }

    int GetResult(List<int> calculateResult)
    {
        int diePair = 0;
        int high = 0;

        for(int i = 0; i < calculateResult.Count; ++i)
        {
            if (calculateResult[i] == 21)
            {
                return 100;
            }
            else if (calculateResult[i] > 21)
            {
                diePair++;
            }
            else
            {
                if (calculateResult[i] > high)
                    high = calculateResult[i];
            }
        }

        if (diePair == calculateResult.Count)
            return -1;

        return high;
    }
}
