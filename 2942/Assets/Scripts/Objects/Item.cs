using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    float movementPerSecond = 2f;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    ItemManager.ItemTypes type;

    public static event Action onEnergyPlus;
    public static event Action onPowerPlus;

    void Start()
    {
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        switch (type)
        {
            case ItemManager.ItemTypes.EnergyPlus:
                EnergyPlus();
                break;
            case ItemManager.ItemTypes.PowerPlus:
                PowerPlus();
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }

    void Update()
    {
        Vector2 position = transform.position;
        position.y -= movementPerSecond * Time.deltaTime;
        transform.position = position;

        if (OffScreen())
            Destroy(gameObject);
    }

    bool OffScreen()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        return x > leftScreenLimit + height || x < rightScreenLimit - height
               ||
               y < lowerScreenLimit - height || y > upperScreenLimit + height ? true : false;
    }

    void EnergyPlus()
    {
        if (onEnergyPlus != null)
            onEnergyPlus();
    }

    void PowerPlus()
    {
        if (onPowerPlus != null)
            onPowerPlus();
    }

    public void SetType(ItemManager.ItemTypes newType)
    {
        type = newType;
    }

    public void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }
}