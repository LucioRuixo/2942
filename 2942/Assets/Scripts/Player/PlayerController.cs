using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;
    public PlayerView view;

    enum Weapons
    {
        RegularGun,
        MachineGun,
        Shotgun,
        HomingMissile
    }

    bool canPlaceBomb = true;
    bool powerPlusOn = false;
    bool machineGunReady = true;
    bool shotgunReady = true;

    float width;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    [HideInInspector] public Vector3 forward;
    Vector3 movement;
    Vector3 proyectileRotationEuler;

    Quaternion proyectileRotation;

    Weapons currentWeapon;

    public GameObject bombPrefab;
    public GameObject proyectilePrefab;
    public GameObject homingMissilePrefab;
    public Transform rightCannon;
    public Transform leftCannon;
    public Transform missileCannon;
    public Transform proyectileContainer;

    public static event Action onCollision;
    public static event Action<bool> onBombStateUpdate;
    public static event Action onBulletTimeUsage;
    public static event Action<HomingMissile> onHomingMissileLaunch;
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

        proyectileRotationEuler = new Vector3(0f, 0f, 180f);
        proyectileRotation = Quaternion.Euler(proyectileRotationEuler);

        currentWeapon = Weapons.RegularGun;

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
        if (currentWeapon != Weapons.MachineGun && Input.GetButtonDown("Shoot"))
            Shoot();
        if (Input.GetButtonDown("Place Bomb") && canPlaceBomb)
            PlaceBomb();

        movement = Vector3.zero;
        if (Input.GetButton("Horizontal"))
            movement.x += Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Vertical"))
            movement.y += Input.GetAxisRaw("Vertical");
        UpdatePosition(movement);
        if (currentWeapon == Weapons.MachineGun && machineGunReady && Input.GetButton("Shoot"))
        {
            Shoot();
            machineGunReady = false;
            StartCoroutine(WeaponCooldown(Weapons.MachineGun, model.machineGunCooldownTime));
        }

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
        switch (currentWeapon)
        {
            case Weapons.RegularGun:
            case Weapons.MachineGun:
                ShootRegularGun();
                break;
            case Weapons.Shotgun:
                if (shotgunReady)
                {
                    ShootShotgun();
                    shotgunReady = false;
                    StartCoroutine(WeaponCooldown(Weapons.Shotgun, model.shotgunCooldownTime));
                }
                break;
            case Weapons.HomingMissile:
                LaunchHomingMissile();
                break;
            default:
                break;
        }

        SoundManager.Get().PlaySound(SoundManager.Sounds.PlayerShot);
    }

    void ShootRegularGun()
    {
        ShootProyectile(true, proyectileRotation);
        ShootProyectile(false, proyectileRotation);
    }

    void ShootShotgun()
    {
        ShootProyectile(true, proyectileRotation);
        ShootProyectile(false, proyectileRotation);

        Vector3 addedRotationEuler = new Vector3(0f, 0f, -model.shotgunProyectileAngle);
        Quaternion addedRotation = Quaternion.Euler(addedRotationEuler);
        ShootProyectile(true, proyectileRotation * addedRotation);
        ShootProyectile(false, proyectileRotation * addedRotation);

        addedRotationEuler = new Vector3(0f, 0f, model.shotgunProyectileAngle);
        addedRotation = Quaternion.Euler(addedRotationEuler);
        ShootProyectile(true, proyectileRotation * addedRotation);
        ShootProyectile(false, proyectileRotation * addedRotation);
    }

    void LaunchHomingMissile()
    {
        HomingMissile newMissile = Instantiate(homingMissilePrefab, missileCannon.position, Quaternion.identity, proyectileContainer).GetComponent<HomingMissile>();
        newMissile.Initialize(powerPlusOn, model.missileDamage, model.movementSpeed);
        newMissile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);

        if (onHomingMissileLaunch != null)
            onHomingMissileLaunch(newMissile);
    }

    void ShootProyectile(bool fromRightCannon, Quaternion rotation)
    {
        Vector2 position = fromRightCannon ? rightCannon.position : leftCannon.position;

        Proyectile newProyectile = Instantiate(proyectilePrefab, position, rotation, proyectileContainer).GetComponent<Proyectile>();
        newProyectile.InitializeAsPlayerProyectile(powerPlusOn, model.damage, model.movementSpeed);
        newProyectile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
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
                StartCoroutine(ItemTimer(type));
                break;
            case ItemManager.Types.BulletTime:
                if (onBulletTimeUsage != null)
                    onBulletTimeUsage();
                break;
            case ItemManager.Types.MachineGun:
                currentWeapon = Weapons.MachineGun;
                StartCoroutine(ItemTimer(type));
                break;
            case ItemManager.Types.Shotgun:
                currentWeapon = Weapons.Shotgun;
                StartCoroutine(ItemTimer(type));
                break;
            case ItemManager.Types.HomingMissile:
                currentWeapon = Weapons.HomingMissile;
                StartCoroutine(ItemTimer(type));
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

    IEnumerator ItemTimer(ItemManager.Types type)
    {
        yield return new WaitForSeconds(model.itemEffectDuration);

        switch (type)
        {
            case ItemManager.Types.PowerPlus:
                powerPlusOn = false;
                break;
            case ItemManager.Types.MachineGun:
            case ItemManager.Types.Shotgun:
            case ItemManager.Types.HomingMissile:
                currentWeapon = Weapons.RegularGun;
                break;
            default:
                break;
        }
    }

    IEnumerator WeaponCooldown(Weapons weapon, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (weapon == Weapons.MachineGun)
            machineGunReady = true;
        else
            shotgunReady = true;
    }
}