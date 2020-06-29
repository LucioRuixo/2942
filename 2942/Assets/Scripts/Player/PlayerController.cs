using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    int damage;

    float width;
    float height;
    float movementSpeed;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    [HideInInspector] public Vector3 forward;
    Vector3 movement;

    public GameManager gameManager;
    public GameObject bombPrefab;
    public GameObject proyectilePrefab;
    public Transform rightCannon;
    public Transform leftCannon;

    void Start()
    {
        damage = model.Damage;

        width = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        movementSpeed = model.MovementSpeed;
        leftScreenLimit = gameManager.leftScreenLimit;
        rightScreenLimit = gameManager.rightScreenLimit;
        lowerScreenLimit = gameManager.lowerScreenLimit;
        upperScreenLimit = gameManager.upperScreenLimit;

        forward = transform.up;
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

        if (Input.GetButtonDown("Shoot"))
            Shoot();

        if (Input.GetButtonDown("Place Bomb"))
            PlaceBomb();
    }

    void UpdatePosition(Vector3 movement)
    {
        Vector3 position = transform.position;
        position += movement * model.MovementSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, rightScreenLimit + width, leftScreenLimit - width);
        position.y = Mathf.Clamp(position.y, lowerScreenLimit + height, upperScreenLimit - height);

        transform.position = position;
    }

    void Shoot()
    {
        Proyectile proyectile;

        proyectile = Instantiate(proyectilePrefab, rightCannon.position, Quaternion.identity).GetComponent<Proyectile>();
        proyectile.Initialize(true, damage, movementSpeed, forward);

        proyectile = Instantiate(proyectilePrefab, leftCannon.position, Quaternion.identity).GetComponent<Proyectile>();
        proyectile.Initialize(true, damage, movementSpeed, forward);
    }

    void PlaceBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}