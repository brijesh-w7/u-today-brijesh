using UnityEngine;
using System;
using UnityEngine.UI;
 
public class CommonPopup : MonoParent
{
    [SerializeField] private Text messageText; // Or use Text
    [SerializeField] private Button positiveButton;
    [SerializeField] private Button negativeButton;
    [SerializeField] private Text positiveButtonText;
    [SerializeField] private Text negativeButtonText;

    private Action<bool> callback;

    private void Awake()
    { 

        gameObject.SetActive(false);
        positiveButton.onClick.AddListener(OnPositiveClicked);
        negativeButton.onClick.AddListener(OnNegativeClicked);
    }

    /// <summary>
    /// Show popup
    /// </summary>
    public void Show(string message, Action<bool> resultCallback,  string positiveText = "OK", string negativeText = "Cancel")
    {
        messageText.text = message;

        positiveButtonText.text = positiveText;
        negativeButtonText.text = negativeText;

        callback = resultCallback;

        gameObject.SetActive(true);
    }

    private void OnPositiveClicked()
    {
        AudioHandler.Instance.PlayButtonClickedAudio();
        callback?.Invoke(true);
        Close();
    }

    private void OnNegativeClicked()
    {
        AudioHandler.Instance.PlayButtonClickedAudio();
        callback?.Invoke(false);
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
