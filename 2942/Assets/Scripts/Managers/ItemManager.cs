using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemTypes
    {
        EnergyPlus,
        PowerPlus,
        Size
    }

    float leftScreenLimit;
    float rightScreenLimit;
    float upperScreenLimit;
    float lowerScreenLimit;

    public GameObject energyPlusPrefab;
    public GameObject powerPlusPrefab;

    void OnEnable()
    {
        GameManager.onScreenLimitsSetting += SetScreenLimits;

        EnemyModel.onItemGeneration += GenerateItem;
    }

    void OnDisable()
    {
        GameManager.onScreenLimitsSetting -= SetScreenLimits;

        EnemyModel.onItemGeneration -= GenerateItem;
    }

    void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }

    void GenerateItem(float x, float y)
    {
        Vector2 position = new Vector2(x, y);

        ItemTypes type = (ItemTypes)Random.Range(0, (int)ItemTypes.Size);

        GameObject prefab;

        switch (type)
        {
            case ItemTypes.EnergyPlus:
                prefab = energyPlusPrefab;
                break;
            case ItemTypes.PowerPlus:
                prefab = powerPlusPrefab;
                break;
            default:
                prefab = null;
                break;
        }

        if (prefab)
        {
            Item item = Instantiate(prefab, position, Quaternion.identity, transform).GetComponent<Item>();
            item.SetType(type);
            item.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
        }
    }
}
