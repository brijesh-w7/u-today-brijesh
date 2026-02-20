using UnityEngine;

public class UIController : SingletonMono<UIController>
{
    [SerializeField] GamePlayUI gamePlayUI;
    [SerializeField] MainMenuUI mainMenuUI;
    [SerializeField] CommonPopup commonPopup;

    public GamePlayUI GamePlayUI { get => gamePlayUI; }
    public MainMenuUI MainMenuUI { get => mainMenuUI; }
    public CommonPopup Popup { get => commonPopup; }


    private void Start()
    {
        commonPopup.gameObject.SetActive(false);
        GamePlayUI.gameObject.SetActive(false);
    }

    public void ShowGamePlayScreen() => gamePlayUI.gameObject.SetActive(true);

    public void ShowMainMenuScreen()=>  mainMenuUI.gameObject.SetActive(true);

    public void HideGamePlayScreen() => gamePlayUI.gameObject.SetActive(false);

    public void HideMainMenuScreen() => mainMenuUI.gameObject.SetActive(false);
     
}