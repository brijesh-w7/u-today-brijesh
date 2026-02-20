using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoParent
{
    [SerializeField] private Button btnHome;

    [SerializeField] private Text txtMatchTileCount;
    [SerializeField] private Text txtTurnCount;
    [SerializeField] private Text txtDuration;

    [SerializeField] GameObject comboGameObject;

    int duration = 0;

    void Start()
    {
        txtTurnCount.text = "0";
        txtMatchTileCount.text = "0";
        txtDuration.text = "0";
        comboGameObject?.SetActive(false);
    }

    IEnumerator Timer()
    {
        yield return null;

        WaitForSeconds wfs = new WaitForSeconds(1);

        while (true)
        {
            yield return wfs;
            duration++;
            txtDuration.text = duration.ToString();
            SaveAndLoadData.Instance.SetDuration(duration);
        }
    }

    private void OnEnable()
    {
        if (btnHome != null) btnHome.onClick.AddListener(OnClickedHomeButton);
        GameManager.OnTurnCompleted += OnTurnCompletedCallback;
        GameManager.OnCardMatched += OnCardMatchedCallback;
        GameManager.OnComboMade += ShowComboUI;
        StartCoroutine(Timer());
    }

    private void OnDisable()
    {
        if (btnHome != null) btnHome.onClick.RemoveListener(OnClickedHomeButton);
        GameManager.OnTurnCompleted -= OnTurnCompletedCallback;
        GameManager.OnCardMatched -= OnCardMatchedCallback;
        GameManager.OnComboMade -= ShowComboUI;
        StopCoroutine(Timer());
    }

    void OnCardMatchedCallback()
    {
        if (txtMatchTileCount != null) txtMatchTileCount.text = GameManager.Instance.MatchedCardCount.ToString();
    }

    void OnTurnCompletedCallback()
    {
        if (txtTurnCount != null) txtTurnCount.text = GameManager.Instance.CurrentTunrCount.ToString();
    }

    void OnClickedHomeButton()
    {
        AudioHandler.Instance.PlayButtonClickedAudio();
        UIController.Instance.Popup.Show("Are you sure quit the game,\n The progress will be lost.", (state) =>
        {
            UIController.Instance.Popup.Close();
            if (state)
            {
                GameManager.Instance.ForceEndGame();
                UIController.Instance.ShowMainMenuScreen();
                UIController.Instance.HideGamePlayScreen();
            }

        });
    }

    public void SetDataOnGameStarts(int duration)
    {
        OnCardMatchedCallback();
        OnTurnCompletedCallback();
        this.duration = duration;
    }

    public void ShowComboUI()
    {
        comboGameObject?.SetActive(true);
        CallAfterSeconds(.5f, () => { comboGameObject?.SetActive(false); });

    }

}