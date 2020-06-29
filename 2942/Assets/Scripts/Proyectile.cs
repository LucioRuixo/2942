using UnityEngine;

public class Proyectile : MonoBehaviour
{
    bool playerProyectile;

    int damage;

    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    Vector3 movement;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        leftScreenLimit = gameManager.leftScreenLimit;
        rightScreenLimit = gameManager.rightScreenLimit;
        lowerScreenLimit = gameManager.lowerScreenLimit;
        upperScreenLimit = gameManager.upperScreenLimit;
    }                      

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerProyectile && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyModel>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!playerProyectile && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerModel>().TakeDamage(damage);
            Destroy(gameObject);
        }

    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;

        if (OffScreen())
            Destroy(gameObject);
    }

    public void Initialize(bool playerProyectile, int damage, float movementSpeed, Vector3 forward)
    {
        this.playerProyectile = playerProyectile;
        this.damage = damage;
        movement = forward * movementSpeed * 1.5f;
    }

    bool OffScreen()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        return x > leftScreenLimit + height || x < rightScreenLimit - height
               ||
               y < lowerScreenLimit - height || y > upperScreenLimit + height ?
               true : false;
    }
}