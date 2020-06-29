using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;

    bool canPlaceBomb = false;

    int damage;

    float width;
    float height;
    float movementSpeed;
    float bombCooldown = 10f;
    float bombTimer = 0f;
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

    public static event Action<bool> onBombStateUpdate;

    void Start()
    {
        damage = model.damage;

        width = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        movementSpeed = model.movementSpeed;
        leftScreenLimit = gameManager.leftScreenLimit;
        rightScreenLimit = gameManager.rightScreenLimit;
        lowerScreenLimit = gameManager.lowerScreenLimit;
        upperScreenLimit = gameManager.upperScreenLimit;

        forward = transform.up;

        if (onBombStateUpdate != null)
            onBombStateUpdate(canPlaceBomb);
    }

    void Update()
    {
        ProcessInput();

        if (!canPlaceBomb)
        {
            bombTimer += Time.deltaTime;

            if (bombTimer >= bombCooldown)
            {
                canPlaceBomb = true;

                if (onBombStateUpdate != null)
                    onBombStateUpdate(canPlaceBomb);
            }
        }
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

        if (Input.GetButtonDown("Place Bomb") && canPlaceBomb)
            PlaceBomb();
    }

    void UpdatePosition(Vector3 movement)
    {
        Vector3 position = transform.position;
        position += movement * model.movementSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, rightScreenLimit + width, leftScreenLimit - width);
        position.y = Mathf.Clamp(position.y, lowerScreenLimit + height, upperScreenLimit - height);

        transform.position = position;
    }

    void Shoot()
    {
        Vector3 rotationEuler = new Vector3(0f, 0f, 180f);
        Quaternion rotation = Quaternion.Euler(rotationEuler);

        Proyectile proyectile;

        proyectile = Instantiate(proyectilePrefab, rightCannon.position, rotation).GetComponent<Proyectile>();
        proyectile.Initialize(true, damage, movementSpeed, forward);

        proyectile = Instantiate(proyectilePrefab, leftCannon.position, rotation).GetComponent<Proyectile>();
        proyectile.Initialize(true, damage, movementSpeed, forward);
    }

    void PlaceBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);

        canPlaceBomb = false;
        bombTimer = 0f;

        if (onBombStateUpdate != null)
            onBombStateUpdate(canPlaceBomb);
    }
}