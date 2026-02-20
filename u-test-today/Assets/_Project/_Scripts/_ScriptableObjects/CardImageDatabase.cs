using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/Item Database")]
public class CardImageDatabase : ScriptableObject
{

    #region Inner Class
    [System.Serializable]
    public class ItemData
    {
        [SerializeField] private int id;
        [SerializeField] private Sprite sprite;

        // Constructor
        public ItemData(int id, Sprite sprite)
        {
            this.id = id;
            this.sprite = sprite;
        }

        // Getter
        public int Id
        {
            get => id;
        }

        public Sprite Sprite
        {
            get => sprite;
        }
       
    }
    #endregion Inner Class

    [SerializeField] private List<ItemData> items = new List<ItemData>();

    // Get full list
    public List<ItemData> GetAllItems()
    {
        return items;
    }

    // Get by ID
    public ItemData GetItemById(int id)
    {
        return items.Find(item => item.Id == id);
    }

    // Get by ID
    public ItemData GetRandomItem()
    {
        return items[UnityEngine.Random.Range(0, items.Count)];

    }

    // Remove by ID
    public void RemoveItem(int id)
    {
        ItemData item = GetItemById(id);
        if (item != null)
        {
            items.Remove(item);
        }
    }
     
    public List<ItemData> GetRandomeItemListByCount(int maxCount)
    {

        if (maxCount >= items.Count)
        {
            return new List<ItemData>(items);
        }
        else
        {

            List<ItemData> tmpList = new List<ItemData>(maxCount);

            List<ItemData> shuffle = new List<ItemData>(items);
            shuffle.Shuffle();

            foreach (var item in shuffle)
            {
                tmpList.Add(item);
                if (tmpList.Count == maxCount) break;
            }
            return tmpList;
        }
    }
}
