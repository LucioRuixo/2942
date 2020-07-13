using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float movementPerSecond;

    void Update()
    {
        Vector2 position = transform.position;
        position.y -= movementPerSecond * Time.deltaTime;
        transform.position = position;
    }
}