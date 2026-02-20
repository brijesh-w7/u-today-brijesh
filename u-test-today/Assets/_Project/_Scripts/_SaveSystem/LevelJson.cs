using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelJson
{

    #region Sub class
    [Serializable]
    public class CardItem
    {
        public int imageId;
        public bool isActive;

        public CardItem() { }

        public CardItem(bool isActive, int imageId)
        {
            this.isActive = isActive;
            this.imageId = imageId;
        }

    }
    #endregion Sub class


    public int levelNo;
    public int cardMatchedCount;
    public int turnCount;
    public int duration;
    public Vector2Int gridSize;
    public List<List<CardItem>> gridData;

    public bool HasWon()
    {

        for (int r = 0; r < gridSize.x; r++)
            for (int c = 0; c < gridSize.y; c++)
                if (gridData[r][c].isActive) return false;
        return true;
    }

    public CardItem GetCardItemAt(int row, int column)
    {
        return gridData[row][column];
    } 

    public void SetFreshData(Vector2Int gridSize, Card[] cards)
    {
        this.gridSize = gridSize;
        int index = 0;
        gridData = new List<List<CardItem>>(gridSize.x);
        for (int r = 0; r < gridSize.x; r++)
        {
            gridData.Add(new List<CardItem>());
            for (int c = 0; c < gridSize.y; c++)
            {
                index = r * gridSize.y + c;
                gridData[r].Add(new CardItem(true, cards[index].ImageDataSO.Id));
                gridData[r][c].isActive = true;
            }
        }
    }

    public int GetCardCountLeftToReveal()
    {
        int revealCount = 0;
        for (int r = 0; r < gridSize.x; r++)
            for (int c = 0; c < gridSize.y; c++)
                if (gridData[r][c].isActive) revealCount++;

        return revealCount;
    }

}
