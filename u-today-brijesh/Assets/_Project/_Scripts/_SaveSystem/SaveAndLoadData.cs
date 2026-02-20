using UnityEngine;
using Newtonsoft.Json;


public class SaveAndLoadData : SingletonMono<SaveAndLoadData>
{
   [HideInInspector] public LevelJson json;

    public static readonly string key = "k_json_data";

    void Start()
    {
        if (HasJson())
        { 
            LoadJson();
            if (json.HasWon()) ClearJson();
        }
    }

    public bool HasJson() => PlayerPrefs.HasKey(key);

    private void SaveJson()
    {
        PlayerPrefs.SetString(key, JsonConvert.SerializeObject(json));
        //Debug.Log("SaveData: " + JsonConvert.SerializeObject(json)); 
    }
    public void ClearJson()
    {
        PlayerPrefs.DeleteKey(key);
        json = null;
    }

    public void LoadJson()
    {
        json = JsonConvert.DeserializeObject<LevelJson>(PlayerPrefs.GetString(key, ""));
    }

    public void OnLevelFinished() => ClearJson();

    public void SetFreshData(Vector2Int gridSize, Card[] cards) => json.SetFreshData(gridSize, cards);

    private void OnEnable()
    {
        GameManager.OnCardMatched += OnTileMatchedCallback;
        GameManager.OnTurnCompleted += OnTurnCompletedCallback;
        GameManager.OnTurnCompleted += OnTurnCompletedCallback;
    }

    private void OnDisable()
    {
        GameManager.OnCardMatched -= OnTileMatchedCallback;
        GameManager.OnTurnCompleted -= OnTurnCompletedCallback;
        GameManager.OnTurnCompleted -= OnTurnCompletedCallback;
    }

    void OnTileMatchedCallback()
    {
        if (json != null)
        {
            json.cardMatchedCount = GameManager.Instance.MatchedCardCount;
            SaveJson();
        }
    }

    void OnTurnCompletedCallback()
    {
        if (json != null)
        {
            json.turnCount = GameManager.Instance.CurrentTunrCount;
            SaveJson();
        }
    }

    public void SetCardItemStatusFalseAt(int row, int column)
    {
        if (json != null) json.gridData[row][column].isActive = false;
    }

    public void SetDuration(int duration)
    {
        if (json != null) json.duration = duration;
    }
}

 