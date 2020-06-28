using UnityEngine;

public class LocustProyectile : MonoBehaviour
{
    float movementSpeed = 15f;
    float height;
    float positionYGoal;

    Vector3 movement;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        positionYGoal = gameManager.lowerScreenLimit - height;

        movement = new Vector3(0f, movementSpeed, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    void Update()
    {
        transform.position -= movement * Time.deltaTime;

        if (transform.position.y <= positionYGoal)
            Destroy(gameObject);
    }
}