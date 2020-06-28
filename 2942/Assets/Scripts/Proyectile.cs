using UnityEngine;

public class Proyectile : MonoBehaviour
{
    float height;
    float positionYGoal;

    Vector3 movement;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        positionYGoal = gameManager.lowerScreenLimit - height;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;

        if (transform.position.y <= positionYGoal)
            Destroy(gameObject);
    }

    public void SetMovement(Vector3 forward, float movementSpeed)
    {
        movement = forward * movementSpeed * 1.5f;
    }
}