using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;
    public PlayerView view;

    bool canPlaceBomb = false;
    bool powerPlusOn = false;

    int damage;

    public float bombCooldown;
    public float powerPlusDuration;
    float width;
    float height;
    float movementSpeed;
    float bombTimer = 0f;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    [HideInInspector] public Vector3 forward;
    Vector3 movement;

    public GameObject bombPrefab;
    public GameObject proyectilePrefab;
    public Transform rightCannon;
    public Transform leftCannon;
    public Transform proyectileContainer;

    public static event Action onCollisionWithProyectile;
    public static event Action<bool> onBombStateUpdate;
    public static event Action onLevelEndReached;

    void OnEnable()
    {
        GameManager.onScreenLimitsSetting += SetScreenLimits;

        Item.onEnergyPlus += ApplyEnergyPlus;
        Item.onPowerPlus += ApplyPowerPlus;
    }

    void Start()
    {
        damage = model.damage;

        width = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        movementSpeed = model.movementSpeed;

        forward = transform.up;

        if (onBombStateUpdate != null)
            onBombStateUpdate(canPlaceBomb);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy Proyectile")
        {
            int damageTaken = collision.gameObject.GetComponent<Proyectile>().GetDamage();
            model.TakeDamage(damageTaken);
            view.CheckIfDamageColorOn();

            if (onCollisionWithProyectile != null)
                onCollisionWithProyectile();
        }
        else if (collision.tag == "Level End" && onLevelEndReached != null)
            onLevelEndReached();
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

    void OnDisable()
    {
        GameManager.onScreenLimitsSetting -= SetScreenLimits;

        Item.onEnergyPlus -= ApplyEnergyPlus;
        Item.onPowerPlus -= ApplyPowerPlus;
    }

    void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
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

        Proyectile newProyectile;

        newProyectile = Instantiate(proyectilePrefab, rightCannon.position, rotation, proyectileContainer).GetComponent<Proyectile>();
        newProyectile.InitializeAsPlayerProyectile(powerPlusOn, damage, movementSpeed);
        newProyectile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);

        newProyectile = Instantiate(proyectilePrefab, leftCannon.position, rotation, proyectileContainer).GetComponent<Proyectile>();
        newProyectile.InitializeAsPlayerProyectile(powerPlusOn, damage, movementSpeed);
        newProyectile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
    }

    void PlaceBomb()
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);

        canPlaceBomb = false;
        bombTimer = 0f;

        if (onBombStateUpdate != null)
            onBombStateUpdate(canPlaceBomb);
    }

    void ApplyEnergyPlus()
    {
        model.RefillEnergy();
    }

    void ApplyPowerPlus()
    {
        powerPlusOn = true;

        StartCoroutine(PowerPlusTimer(powerPlusDuration));
    }

    IEnumerator PowerPlusTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        powerPlusOn = false;
    }
}