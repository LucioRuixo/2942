using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;
    public PlayerView view;

    bool canPlaceBomb = true;
    bool powerPlusOn = false;

    float width;
    float height;
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

    public static event Action onCollision;
    public static event Action<bool> onBombStateUpdate;
    public static event Action onBulletTimeUsage;
    public static event Action onLevelEndReached;

    void OnEnable()
    {
        GameManager.onScreenLimitsSetting += SetScreenLimits;

        Item.onItemAdquisition += UseItem;
    }

    void Start()
    {
        width = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;

        forward = transform.up;

        if (onBombStateUpdate != null)
            onBombStateUpdate(canPlaceBomb);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy Proyectile" || collision.tag == "Enemy")
        {
            int damageTaken;
            if (collision.tag == "Enemy Proyectile")
                damageTaken = collision.GetComponent<Proyectile>().GetDamage();
            else
                damageTaken = collision.GetComponent<EnemyModel>().GetCollisionDamage();

            model.TakeDamage(damageTaken);
            view.CheckIfDamageColorOn();

            if (onCollision != null)
                onCollision();
        }
        else if (collision.tag == "Level End" && onLevelEndReached != null)
            onLevelEndReached();
    }

    void Update()
    {
        ProcessInput();
    }

    void OnDisable()
    {
        GameManager.onScreenLimitsSetting -= SetScreenLimits;

        Item.onItemAdquisition -= UseItem;
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
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0f)
            view.ToggleThrustParticleSystem(true);
        if (Input.GetButtonDown("Shoot"))
            Shoot();
        if (Input.GetButtonDown("Place Bomb") && canPlaceBomb)
            PlaceBomb();

        movement = Vector3.zero;
        if (Input.GetButton("Horizontal"))
            movement.x += Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Vertical"))
            movement.y += Input.GetAxisRaw("Vertical");
        UpdatePosition(movement);

        if (Input.GetButtonUp("Vertical"))
            view.ToggleThrustParticleSystem(false);
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
        newProyectile.InitializeAsPlayerProyectile(powerPlusOn, model.damage, model.movementSpeed);
        newProyectile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);

        newProyectile = Instantiate(proyectilePrefab, leftCannon.position, rotation, proyectileContainer).GetComponent<Proyectile>();
        newProyectile.InitializeAsPlayerProyectile(powerPlusOn, model.damage, model.movementSpeed);
        newProyectile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);

        SoundManager.Get().PlaySound(SoundManager.Sounds.PlayerShot);
    }

    void PlaceBomb()
    {
        Bomb newBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity).GetComponent<Bomb>();
        newBomb.Initialize(model.bombDetonationTime, model.bombDamage);

        canPlaceBomb = false;
        StartCoroutine(BombCooldown());

        if (onBombStateUpdate != null)
            onBombStateUpdate(canPlaceBomb);
    }

    void UseItem(ItemManager.Types type)
    {
        switch (type)
        {
            case ItemManager.Types.EnergyPlus:
                model.RefillEnergy();
                break;
            case ItemManager.Types.PowerPlus:
                powerPlusOn = true;
                StartCoroutine(PowerPlusTimer());
                break;
            case ItemManager.Types.BulletTime:
                if (onBulletTimeUsage != null)
                    onBulletTimeUsage();
                break;
            default:
                break;
        }
    }

    public void Destroy()
    {
        view.Explode();

        Destroy(gameObject);
    }

    IEnumerator BombCooldown()
    {
        yield return new WaitForSeconds(model.bombCooldownTime);

        canPlaceBomb = true;
    }

    IEnumerator PowerPlusTimer()
    {
        yield return new WaitForSeconds(model.itemEffectDuration);

        powerPlusOn = false;
    }
}