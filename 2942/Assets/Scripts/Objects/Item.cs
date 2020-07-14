using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    float movementPerSecond;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    ItemManager.Types type;

    public static event Action<ItemManager.Types> onItemAdquisition;

    void Start()
    {
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        if (onItemAdquisition != null)
        {
            switch (type)
            {
                case ItemManager.Types.EnergyPlus:
                        onItemAdquisition(ItemManager.Types.EnergyPlus);
                    break;
                case ItemManager.Types.PowerPlus:
                    onItemAdquisition(ItemManager.Types.PowerPlus);
                    break;
                case ItemManager.Types.BulletTime:
                    onItemAdquisition(ItemManager.Types.BulletTime);
                    break;
                default:
                    break;
            }
        }

        SoundManager.Get().PlaySound(SoundManager.Sounds.Item);

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

    public void Initialize(float movementPerSecond, ItemManager.Types type)
    {
        this.movementPerSecond = movementPerSecond;
        this.type = type;
    }

    public void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }
}