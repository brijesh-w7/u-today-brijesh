using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoParent
{
    [SerializeField] ToggleGroup difficultyLevel;
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    bool isStartingGamePlay = false;

    private void OnEnable()
    {
        if (playButton != null) playButton.onClick.AddListener(OnClickedPlayButton);
        if (settingsButton != null) settingsButton.onClick.AddListener(OnClickedSettingsButton);
        isStartingGamePlay = false;

        CallAfterSeconds(.1f, () =>
        {
            if (SaveAndLoadData.Instance.HasJson())
            {
                Vector2Int gridSize = SaveAndLoadData.Instance.json.gridSize;
                int childIndex = 0;
                if (gridSize.x == 2 && gridSize.y == 2) childIndex = 0;
                else if (gridSize.x == 2 && gridSize.y == 3) childIndex = 1;
                if (gridSize.x == 5 && gridSize.y == 6) childIndex = 2;
                difficultyLevel.transform.GetChild(childIndex).GetComponent<Toggle>().isOn = true;
            }
        });
    }

    private void OnDisable()
    {
        if (playButton != null) playButton.onClick.RemoveListener(OnClickedPlayButton);
        if (settingsButton != null) settingsButton.onClick.RemoveListener(OnClickedSettingsButton);
        isStartingGamePlay = false;
    }

    private void OnClickedPlayButton()
    {
        AudioHandler.Instance.PlayButtonClickedAudio();
        if (isStartingGamePlay) return;

        isStartingGamePlay = true;
        Vector2Int gameSize = GetGameSize();
        UIController.Instance.ShowGamePlayScreen();
        GameManager.Instance.StartCardGame(gameSize);

        CallAfterSeconds(.1f, UIController.Instance.HideMainMenuScreen);

    }

    private void OnClickedSettingsButton()=>  AudioHandler.Instance.PlayButtonClickedAudio();

    Vector2Int GetGameSize()
    {

        switch (difficultyLevel.GetFirstActiveToggle().transform.GetSiblingIndex())
        {
            case 0: return new Vector2Int(2, 2);
            case 1: return new Vector2Int(2, 3);
            case 2: return new Vector2Int(5, 6);
        }
        return new Vector2Int(2, 2);
    }
}