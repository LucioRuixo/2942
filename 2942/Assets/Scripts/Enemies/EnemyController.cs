using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyModel model;

    enum States
    {
        NotShooting,
        Shooting
    }

    bool onScreen = false;

    int damage;

    float height;
    float movementSpeed;
    float shootingInterval;
    float shootingTimer = 0f;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    [HideInInspector] public Vector3 forward;
    Vector3 movement;

    States state;

    GameManager gameManager;
    public GameObject proyectilePrefab;
    public Transform rightCannon;
    public Transform leftCannon;

    void OnEnable()
    {
        Bomb.onBombExplosion += Explode;
    }

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        damage = model.damage;

        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        shootingInterval = model.shootingInterval;
        leftScreenLimit = gameManager.leftScreenLimit;
        rightScreenLimit = gameManager.rightScreenLimit;
        lowerScreenLimit = gameManager.lowerScreenLimit;
        upperScreenLimit = gameManager.upperScreenLimit;

        movementSpeed = model.movementSpeed;
        forward = transform.up;
        movement = forward * movementSpeed;

        state = States.NotShooting;
    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;

        ProcessStates();

        if (!onScreen)
        {
            if (!OffScreen())
                onScreen = true;
        }
        else if (OffScreen())
            Destroy(gameObject);
    }

    void OnDisable()
    {
        Bomb.onBombExplosion -= Explode;
    }

    void ProcessStates()
    {
        if (state == States.NotShooting)
        {
            if (shootingTimer < shootingInterval)
                shootingTimer += Time.deltaTime;


            if (PlayerDetected())
            {
                state = States.Shooting;

                if (shootingTimer >= shootingInterval)
                {
                    Shoot();
                    shootingTimer = 0f;
                }
            }
        }
        else
        {
            shootingTimer += Time.deltaTime;

            if (shootingTimer >= shootingInterval)
            {
                Shoot();
                shootingTimer = 0f;
            }

            if (!PlayerDetected())
            {
                state = States.NotShooting;
            }
        }
    }

    bool PlayerDetected()
    {
        float rayDistance = Screen.width;

        Vector3 position = transform.position + forward * height;

        Collider2D collider;
        Ray ray;
        RaycastHit2D raycastHit;

        ray = new Ray(position, forward);
        raycastHit = Physics2D.Raycast(position, forward, rayDistance);
        collider = raycastHit.collider;
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow);

        if (collider != null)
        {
            return collider.gameObject.tag == "Player" ? true : false;
        }
        else
            return false;
    }

    void Shoot()
    {
        Proyectile proyectile;

        proyectile = Instantiate(proyectilePrefab, rightCannon.position, Quaternion.identity).GetComponent<Proyectile>();
        proyectile.Initialize(false, damage, movementSpeed, forward);

        proyectile = Instantiate(proyectilePrefab, leftCannon.position, Quaternion.identity).GetComponent<Proyectile>();
        proyectile.Initialize(false, damage, movementSpeed, forward);
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

    void Explode()
    {
        Destroy(gameObject);
    }
}