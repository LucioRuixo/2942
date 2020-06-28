using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public float leftScreenLimit;
    [HideInInspector] public float rightScreenLimit;
    [HideInInspector] public float lowerScreenLimit;
    [HideInInspector] public float upperScreenLimit;

    [HideInInspector] public Vector2 screenBounds;

    void Start()
    {
        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(position);

        leftScreenLimit = screenBounds.x * -1;
        rightScreenLimit = screenBounds.x;
        lowerScreenLimit = screenBounds.y;
        upperScreenLimit = screenBounds.y * -1;
    }
}