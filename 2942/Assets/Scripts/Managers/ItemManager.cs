using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum Types
    {
        EnergyPlus,
        PowerPlus,
        BulletTime
    }

    struct ItemData
    {
        public Types type;

        public GameObject prefab;
    }

    public float movementPerSecond;
    float leftScreenLimit;
    float rightScreenLimit;
    float upperScreenLimit;
    float lowerScreenLimit;

    public List<ItemSO> itemSOs;
    List<ItemData> items;

    void OnEnable()
    {
        GameManager.onScreenLimitsSetting += SetScreenLimits;

        EnemyModel.onItemGeneration += Generate;
    }

    void Start()
    {
        items = new List<ItemData>();

        for (int i = 0; i < itemSOs.Count; i++)
        {
            items.Add(Initialize(itemSOs[i]));
        }
    }

    void OnDisable()
    {
        GameManager.onScreenLimitsSetting -= SetScreenLimits;

        EnemyModel.onItemGeneration -= Generate;
    }

    void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }

    ItemData Initialize(ItemSO itemSO)
    {
        ItemData newItem;

        newItem.type = itemSO.type;
        newItem.prefab = itemSO.prefab;

        return newItem;
    }

    void Generate(float x, float y)
    {
        Vector2 position = new Vector2(x, y);

        ItemData newItem = items[Random.Range(0, items.Count)];

        Item item = Instantiate(newItem.prefab, position, Quaternion.identity, transform).GetComponent<Item>();
        item.Initialize(movementPerSecond, newItem.type);
        item.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
    }
}