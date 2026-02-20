using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingletonMono<GameManager>
{
    public CardImageDatabase cardImageDatabase;

    [SerializeField] GridGenerator gridGenerator;

    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private Sprite defaultCardSprite;

    private Card[] cards;

    [SerializeField] private GameObject cardsContainer;
    [SerializeField] private ComboValidator comboValidator;

    private Vector2Int gridSize;

    private int spriteSelected;
    private int cardSelected;
    private int cardLeft;
    private bool gameStart;
    private int matchedCardCount = 0;
    private int currentTunrCount = 0;

    public static event Action OnTurnCompleted;
    public static event Action OnCardMatched;
    public static event Action OnComboMade;


    public int CurrentTunrCount
    {
        private set { }

        get => currentTunrCount;
    }

    public int MatchedCardCount
    {
        private set { }
        get => matchedCardCount;
    }

    void Start()
    {
        gameStart = false;
        cardsContainer.SetActive(false);
    }

    public void StartCardGame(Vector2Int gameSize)
    {
        if (gameStart) return; // return if game already running
        
        int duration;

        bool hasSaveData = SaveAndLoadData.Instance.HasJson();

        this.gridSize = gameSize;
        gameStart = true;

        cardsContainer.SetActive(true);

        cards = gridGenerator.GenerateGrid(cardsContainer.GetComponent<RectTransform>(), cardPrefab, gameSize.x, gameSize.y);

        cardSelected = spriteSelected = -1;

        if (hasSaveData)
        {
            matchedCardCount = SaveAndLoadData.Instance.json.cardMatchedCount;
            currentTunrCount = SaveAndLoadData.Instance.json.turnCount;
            duration = SaveAndLoadData.Instance.json.duration;

            SpriteCardAllocationFromSaveData();
            cardLeft = SaveAndLoadData.Instance.json.GetCardCountLeftToReveal();
        }
        else
        {
            matchedCardCount = 0;
            currentTunrCount = 0;
            duration = 0;

            SpriteCardAllocation();
            cardLeft = cards.Length;

            SaveAndLoadData.Instance.json = new LevelJson();
            SaveAndLoadData.Instance.SetFreshData(gameSize, cards);
        }
        StartCoroutine(HideFace());
        CallAfterSeconds(.1f, () => { UIController.Instance.GamePlayUI.SetDataOnGameStarts(duration); });

        HanndleComboValidator();
        
    }

    void HanndleComboValidator() {
        if (comboValidator != null) comboValidator.RemoveListener();
        comboValidator = new ComboValidator();
        comboValidator.AddListener();
    }

    IEnumerator HideFace()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < cards.Length; i++)
            cards[i].Flip();
        AudioHandler.Instance.PlayAudioClip(AudioHandler.Instance.clipCardFlip);
        yield return new WaitForSeconds(0.5f);
    }

    private void SpriteCardAllocation()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].ImageDataSO = null;
            cards[i].ResetRotation();
        }

        int max = cards.Length / 2;

        List<CardImageDatabase.ItemData> items = cardImageDatabase.GetRandomeItemListByCount(max);

        List<Card> tmpList = new List<Card>(cards);
        tmpList.Shuffle();
        int processIndex = 0;
        for (int i = 0; i < max; i++)
        {
            tmpList[processIndex++].ImageDataSO = items[i % items.Count];
            tmpList[processIndex++].ImageDataSO = items[i % items.Count];
        }
    }

    private void SpriteCardAllocationFromSaveData()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Active();
            cards[i].ImageDataSO = null;
            cards[i].ResetRotation();
        }
        LevelJson saveJson = SaveAndLoadData.Instance.json;
        int index = 0;
        for (int row = 0; row < gridSize.x; row++)
        {
            for (int col = 0; col < gridSize.y; col++)
            {
                index = row * gridSize.y + col;

                if (saveJson.GetCardItemAt(row, col).isActive)
                {
                    cards[index].ImageDataSO = cardImageDatabase.GetItemById(saveJson.GetCardItemAt(row, col).imageId);
                    cards[index].Active();
                }
                else
                {
                    cards[index].ImageDataSO = null;
                    cards[index].Inactive();
                }
            }
        }

    }

    public Sprite CardBack()
    {
        return defaultCardSprite;
    }

    public bool CanCardClick()
    {
        if (!gameStart) return false;
        return true;
    }

    public void OnClickedCardButton(int clickedSpriteId, int clickedCardId)
    {
        if (spriteSelected == -1)
        {
            spriteSelected = clickedSpriteId;
            cardSelected = clickedCardId;
        }
        else
        {
            if (spriteSelected == clickedSpriteId)
            {
                AudioHandler.Instance.PlayAudioClip(AudioHandler.Instance.clipCardMatched);
                cards[cardSelected].Inactive();
                cards[clickedCardId].Inactive();
                cardLeft -= 2;

                matchedCardCount++;
                SaveAndLoadData.Instance.SetCardItemStatusFalseAt(cardSelected / gridSize.y, cardSelected % gridSize.y);
                SaveAndLoadData.Instance.SetCardItemStatusFalseAt(clickedCardId / gridSize.y, clickedCardId % gridSize.y);

                
            }
            else
            {

                AudioHandler.Instance.PlayAudioClip(AudioHandler.Instance.clipCardmismatched);
                cards[cardSelected].Flip();
                cards[clickedCardId].Flip();
            }
            currentTunrCount++;
            OnTurnCompleted?.Invoke();
            if (spriteSelected == clickedSpriteId)
            {
                OnCardMatched?.Invoke();
                CheckGameWin();
            }
            cardSelected = spriteSelected = -1;

        }
    }

    private void CheckGameWin()
    {
        if (cardLeft <= 0 || matchedCardCount >= gridSize.x * gridSize.y)
        {
            EndGame();
            AudioHandler.Instance.PlayAudioClip(AudioHandler.Instance.clipGameOver);
        }
    }

    private void EndGame()
    {
        gameStart = false;
        SaveAndLoadData.Instance.OnLevelFinished();

        CallAfterSeconds(.75f, () =>
        {
            cardsContainer.SetActive(false);
            UIController.Instance.HideGamePlayScreen();
            UIController.Instance.ShowMainMenuScreen();
        });
    }

    public void ForceEndGame()
    {
        SaveAndLoadData.Instance.ClearJson();
        gameStart = false;
        cardsContainer.SetActive(false);
    }

    public void DoComboMade() => OnComboMade?.Invoke();

}