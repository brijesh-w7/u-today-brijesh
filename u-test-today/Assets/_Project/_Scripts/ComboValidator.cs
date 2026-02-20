using UnityEngine;
using System;

[Serializable]
public class ComboValidator
{
    [SerializeField] private int lastMatchAtTurn = -1;

    [SerializeField] bool isCombo = false;

    public bool IsCombo { get => isCombo; }


    public void AddListener()
    {
        GameManager.OnCardMatched += OnCardMatchedCallback;
    }

    public void RemoveListener()
    {
        GameManager.OnCardMatched -= OnCardMatchedCallback;
    }

    void OnCardMatchedCallback()
    {
        int currentMatchTurn = GameManager.Instance.CurrentTunrCount;
        if (lastMatchAtTurn == -1) lastMatchAtTurn = currentMatchTurn;

        if (lastMatchAtTurn + 1 == currentMatchTurn)
        {
            isCombo = true;
            GameManager.Instance?.DoComboMade();
        }
        else
        {
            isCombo = false;
        }

        lastMatchAtTurn = currentMatchTurn;

    }
}