using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    float width;
    float height;

    Vector2 screenBounds;
    Vector3 movement;

    public GameObject bombPrefab;

    void Start()
    {
        width = transform.GetComponent<SpriteRenderer>().size.x / 2f;
        height = transform.GetComponent<SpriteRenderer>().size.y / 2f;

        Vector3 position = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        screenBounds = Camera.main.ScreenToWorldPoint(position);
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        movement = Vector3.zero;

        if (Input.GetButton("Horizontal"))
            movement.x += Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Vertical"))
            movement.y += Input.GetAxisRaw("Vertical");

        UpdatePosition(movement);

        if (Input.GetButtonDown("Place Bomb"))
            PlaceBomb();
    }

    void UpdatePosition(Vector3 movement)
    {
        Vector3 position = transform.position;
        position += movement * model.MovementSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, screenBounds.x + width, screenBounds.x * -1 - width);
        position.y = Mathf.Clamp(position.y, screenBounds.y + height, screenBounds.y * -1 - height);

        transform.position = position;
    }

    void PlaceBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}