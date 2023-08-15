using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTable : NPC
{
    [SerializeField] PlayerDataManager playerDataManager;
    [SerializeField] CinemachineVirtualCamera tableCamera;
    [SerializeField] Transform sitDownTransform, lookAtTransform;
    [SerializeField] Vector3 prevPosition;

    [SerializeField] GameObject infoUI, mainUI;
    [SerializeField] TMP_InputField inputCoinfield;
    [SerializeField] TMP_Text coinField;
    [SerializeField] Button hitButton, stayButton;

    [SerializeField] int battingMoney;
    [SerializeField] int comAHighLine, comBHighLine;

    enum CardPattern { S, D, H, C }
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

    public override void Initialize(PlayerController _player) { }

    public override void Talk(Transform talker) { }

    public override void Interact()
    {
        base.Interact();
        prevPosition = playerController.transform.position;
        playerController.ControllOut();
        playerController.HideUI();
        playerController.transform.LookAt(lookAtTransform);
        playerController.transform.localEulerAngles = new Vector3(0f, playerController.transform.localEulerAngles.y, 0f);
        playerController.MoveCharacterController(sitDownTransform.position);
        playerController.SitAnimation(true);
        interactUI.SetActive(true);
        infoUI.SetActive(true);
        mainUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;

        comAHighLine = 14 + Random.Range(-2, 3);
        comBHighLine = 14 + Random.Range(-2, 3);
        CreateDeck();
        StartNewGame();

        tableCamera.Priority = 2;
    }

    public void StartNewGame()
    {
        coinField.text = $"$ {playerDataManager.Money}";
    }

    public void OnBattingButtonClicked()
    {
        if (inputCoinfield.text.Length == 0)
            return;
        battingMoney = int.Parse(inputCoinfield.text);
        if (battingMoney <= 0)
            return;
        playerDataManager.Money -= battingMoney;
        inputCoinfield.text = "";

        infoUI.SetActive(false);
        mainUI.SetActive(true);

        StartGame();
    }

    public void OnSitUpButtonClicked()
    {
        playerController.SitAnimation(false);
        playerController.MoveCharacterController(prevPosition);
        EndInteract();
        tableCamera.Priority = 0;
    }

    [SerializeField] bool playing, game, hit;
    [SerializeField] int playerResult, dealerResult;
    [SerializeField] int dealerHandCount, playerHandCount, comAHandCount, comBHandCount;
    [SerializeField] bool dealerDie, playerDie, comADie, comBDie;

    [SerializeField] TMP_Text deckCount, usedCount, dealerCount, playerCount, comACount, comBCount;
    [SerializeField] List<Card> dealerHands, playerHands, comAHands, comBHands;
    [SerializeField] List<TMP_Text> dealerHandsText, playerHandsText, comAHandsText, comBHandsText;

    void StartGame()
    {
        playing = true;
        game = true;
        hit = false;

        playerResult = 0;
        dealerResult = 0;

        dealerHandCount = 0;
        playerHandCount = 0;
        comAHandCount = 0;
        comBHandCount = 0;

        deckCount.text = $"{deck.Count}";
        usedCount.text = $"{usedDeck.Count}";
        dealerCount.text = "";
        playerCount.text = "";
        comACount.text = "";
        comBCount.text = "";

        dealerDie = false;
        playerDie = false;
        comADie = false;
        comBDie = false;

        dealerHands = new();
        playerHands = new();
        comAHands = new();
        comBHands = new();

        for(int i = 0; i < dealerHandsText.Count; i++)
        {
            dealerHandsText[i].transform.parent.gameObject.SetActive(false);
            playerHandsText[i].transform.parent.gameObject.SetActive(false);
            comAHandsText[i].transform.parent.gameObject.SetActive(false);
            comBHandsText[i].transform.parent.gameObject.SetActive(false);
        }

        StartCoroutine(Playing());
    }

    IEnumerator Playing()
    {
        while (game)
        {
            yield return new WaitForSeconds(0.5f);
            ComBTurn();

            yield return new WaitForSeconds(0.5f);
            ComATurn();

            hitButton.interactable = true;
            stayButton.interactable = true;
            yield return new WaitUntil(() => { return (hit || playerDie || !playing) || (playerHandCount == 6); });
            hitButton.interactable = (hit || playerDie || !playing) || (playerHandCount == 6);
            stayButton.interactable = (hit || playerDie || !playing) || (playerHandCount == 6);
            PlayerTurn();

            yield return new WaitForSeconds(0.5f);
            DealerTurn();

            yield return new WaitForSeconds(0.5f);

            game = (!playerDie && !comADie && !comBDie && !dealerDie);
        }
        GameResult();
    }

    void PlayerTurn()
    {
        hit = false;
        if (!playerDie && playing && playerHandCount < 6)
        {
            List<int> playerSums = CalculateHand(playerHands);
            int result = GetResult(playerSums);

            playerCount.text = "";

            if (result == -1)
            {
                playerDie = true;
                playerResult = 1;
                playerCount.text = "Burst!";
            }
            else if (result == 100)
            {
                playerDie = true;
                playerResult = 3;
                playerCount.text = "Black Jack!";
            }
            else
            {
                foreach (int sum in playerSums)
                {
                    if(sum < 21)
                        playerCount.text += $"{sum} ";
                }
                playerResult = 2;
            }
        }
    }

    void ComATurn()
    {
        if (!comADie && comAHandCount < 6)
        {
            List<int> sums = CalculateHand(comAHands);
            int result = GetResult(sums);

            if (result <= comAHighLine)
            {
                Card card = DrawCard();
                comAHands.Add(card);
                comAHandsText[comAHandCount].transform.parent.gameObject.SetActive(true);
                switch (card.cardPattern)
                {
                    case CardPattern.S:
                        comAHandsText[comAHandCount].text = $"S{card.cardNum}";
                        break;
                    case CardPattern.D:
                        comAHandsText[comAHandCount].text = $"D{card.cardNum}";
                        break;
                    case CardPattern.H:
                        comAHandsText[comAHandCount].text = $"H{card.cardNum}";
                        break;
                    case CardPattern.C:
                        comAHandsText[comAHandCount].text = $"C{card.cardNum}";
                        break;
                }
                comAHandCount++;

                sums = CalculateHand(comAHands);
                result = GetResult(sums);

                comACount.text = "";

                if (result == -1)
                {
                    comADie = true;
                    comACount.text = "Burst!";
                }
                else if (result == 100)
                {
                    comADie = true;
                    comACount.text = "Black Jack!";
                }
                else
                {
                    foreach (int sum in sums)
                    {
                        if (sum < 21)
                            comACount.text += $"{sum} ";
                    }
                }
            }
        }
    }

    void ComBTurn()
    {
        if (!comBDie && comBHandCount < 6)
        {
            List<int> sums = CalculateHand(comBHands);
            int result = GetResult(sums);

            if (result <= comBHighLine)
            {
                Card card = DrawCard();
                comBHands.Add(card);
                comBHandsText[comBHandCount].transform.parent.gameObject.SetActive(true);
                switch (card.cardPattern)
                {
                    case CardPattern.S:
                        comBHandsText[comBHandCount].text = $"S{card.cardNum}";
                        break;
                    case CardPattern.D:
                        comBHandsText[comBHandCount].text = $"D{card.cardNum}";
                        break;
                    case CardPattern.H:
                        comBHandsText[comBHandCount].text = $"H{card.cardNum}";
                        break;
                    case CardPattern.C:
                        comBHandsText[comBHandCount].text = $"C{card.cardNum}";
                        break;
                }
                comBHandCount++;

                sums = CalculateHand(comBHands);
                result = GetResult(sums);

                comBCount.text = "";

                if (result == -1)
                {
                    comBDie = true;
                    comBCount.text = "Burst!";
                }
                else if (result == 100)
                {
                    comBDie = true;
                    comBCount.text = "Black Jack!";
                }
                else
                {
                    foreach (int sum in sums)
                    {
                        if (sum < 21)
                            comBCount.text += $"{sum} ";
                    }
                }
            }
        }
    }

    void DealerTurn()
    {
        if (!dealerDie && dealerHandCount < 6)
        {
            List<int> sums = CalculateHand(dealerHands);
            int result = GetResult(sums);

            if (result <= 16)
            {
                Card card = DrawCard();
                dealerHands.Add(card);
                dealerHandsText[dealerHandCount].transform.parent.gameObject.SetActive(true);
                switch (card.cardPattern)
                {
                    case CardPattern.S:
                        dealerHandsText[dealerHandCount].text = $"S{card.cardNum}";
                        break;
                    case CardPattern.D:
                        dealerHandsText[dealerHandCount].text = $"D{card.cardNum}";
                        break;
                    case CardPattern.H:
                        dealerHandsText[dealerHandCount].text = $"H{card.cardNum}";
                        break;
                    case CardPattern.C:
                        dealerHandsText[dealerHandCount].text = $"C{card.cardNum}";
                        break;
                }
                dealerHandCount++;

                sums = CalculateHand(dealerHands);
                result = GetResult(sums);

                dealerCount.text = "";

                if (result == -1)
                {
                    dealerDie = true;
                    dealerResult = 1;
                    dealerCount.text = "Burst!";
                }
                else if (result == 100)
                {
                    dealerDie = true;
                    dealerResult = 3;
                    dealerCount.text = "Black Jack!";
                }
                else
                {
                    foreach (int sum in sums)
                    {
                        if (sum < 21)
                            dealerCount.text += $"{sum} ";
                    }
                    dealerResult = 2;
                }
            }
        }
    }

    void GameResult()
    {
        if (playerResult == 1)
        {
            if(dealerResult == 1)
            {
                playerDataManager.Money += battingMoney;
            }
        }
        else if (playerResult == 2)
        {
            if (dealerResult == 1)
            {
                playerDataManager.Money += battingMoney * 2;
            }
            else if (dealerResult == 2)
            {
                int playerHigh = GetResult(CalculateHand(playerHands));
                int dealerHigh = GetResult(CalculateHand(dealerHands));
                if (playerHigh > dealerHigh)
                {
                    playerDataManager.Money += battingMoney * 2;
                }
                else if(playerHigh == dealerHigh)
                {
                    if (playerHandCount <= dealerHandCount)
                    {
                        playerDataManager.Money += battingMoney * 2;
                    }
                }
            }
        }
        else if (playerResult == 3)
        {
            if (dealerResult == 1)
            {
                playerDataManager.Money += (int)(battingMoney * 2.5f);
            }
            else if (dealerResult == 2)
            {
                playerDataManager.Money += (int)(battingMoney * 2.5f);
            }
            else if (dealerResult == 3)
            {
                if(playerHandCount <= dealerHandCount)
                {
                    playerDataManager.Money += (int)(battingMoney * 2.5f);
                }
            }
        }

        StartNewGame();
        infoUI.SetActive(true);
        mainUI.SetActive(false);
    }

    public void OnHitButtonClicked()
    {
        if (playerDie || playerHandCount == 6)
            return;

        Card card = DrawCard();
        playerHands.Add(card);
        playerHandsText[playerHandCount].transform.parent.gameObject.SetActive(true);
        switch (card.cardPattern)
        {
            case CardPattern.S:
                playerHandsText[playerHandCount].text = $"S{card.cardNum}";
                break;
            case CardPattern.D:
                playerHandsText[playerHandCount].text = $"D{card.cardNum}";
                break;
            case CardPattern.H:
                playerHandsText[playerHandCount].text = $"H{card.cardNum}";
                break;
            case CardPattern.C:
                playerHandsText[playerHandCount].text = $"C{card.cardNum}";
                break;
        }
        playerHandCount++;

        hit = true;
    }

    public void OnStayButtonclicked()
    {
        playing = false;
    }

    void CreateDeck()
    {
        deck = new();
        usedDeck = new();

        List<Card> cardList = new();
        for (int i = 1; i <= 13; i++)
        {
            cardList.Add(new Card(CardPattern.S, i));
            cardList.Add(new Card(CardPattern.D, i));
            cardList.Add(new Card(CardPattern.H, i));
            cardList.Add(new Card(CardPattern.C, i));
        }

        while (cardList.Count > 0)
        {
            int cardNum = Random.Range(0, cardList.Count);
            deck.Enqueue(cardList[cardNum]);
            cardList.RemoveAt(cardNum);
        }
    }

    Card DrawCard()
    {
        if(deck.Count == 0)
            RecycleUsedDeck();

        Card card = deck.Dequeue();
        usedDeck.Enqueue(card);

        deckCount.text = $"{deck.Count}";
        usedCount.text = $"{usedDeck.Count}";
        return card;
    }

    void RecycleUsedDeck()
    {
        List<Card> cardList = new();
        while (usedDeck.Count > 0)
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
                    result.Add(result[j] + 11);
                    result[j] += 1;
                }
            }
            else if (hand[i].cardNum < 10)
            {
                for (int j = 0; j < result.Count; j++)
                {
                    result[j] += hand[i].cardNum;
                }
            }
            else
            {
                for (int j = 0; j < result.Count; j++)
                {
                    result[j] += 10;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="calculateResult"></param>
    /// <returns></returns>
    int GetResult(List<int> calculateResult)
    {
        int diePair = 0;
        int high = 0;

        for (int i = 0; i < calculateResult.Count; ++i)
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
