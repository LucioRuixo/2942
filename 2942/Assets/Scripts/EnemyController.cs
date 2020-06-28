using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ShipModel model;

    enum States
    {
        NotShooting,
        Shooting
    }

    float width;
    float height;
    float movementSpeed;
    float positionYGoal;
    float shootingTimer = 0f;

    [HideInInspector] public Vector3 forward;
    Vector3 movement;

    States state;

    GameManager gameManager;
    public GameObject proyectilePrefab;
    public Transform rightCannon;
    public Transform leftCannon;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        width = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        positionYGoal = gameManager.lowerScreenLimit - height;

        movementSpeed = model.MovementSpeed;
        forward = transform.up;
        movement = forward * movementSpeed;

        state = States.NotShooting;
    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;

        if (transform.position.y <= positionYGoal)
            Destroy(gameObject);

        if (state == States.NotShooting)
        {
            if (PlayerDetected())
            {
                state = States.Shooting;
                Shoot();
            }
        }
        else
        {
            shootingTimer += Time.deltaTime;

            if (shootingTimer >= model.ShootingInterval)
            {
                Shoot();
                shootingTimer = 0f;
            }

            if (!PlayerDetected())
            {
                state = States.NotShooting;
                shootingTimer = 0f;
            }
        }
    }

    bool PlayerDetected()
    {
        float rayDistance = 7.5f;

        Vector3 position = transform.position + forward * height;

        Collider2D collider;
        Ray ray;
        RaycastHit2D raycastHit;

        ray = new Ray(position, forward);
        raycastHit = Physics2D.Raycast(position, forward, rayDistance);
        collider = raycastHit.collider;
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow);

        if (collider != null)
            return collider.gameObject.tag == "Player" ? true : false;
        else
            return false;
    }

    void Shoot()
    {
        Proyectile proyectile;

        proyectile = Instantiate(proyectilePrefab, rightCannon.position, Quaternion.identity).GetComponent<Proyectile>();
        proyectile.SetMovement(forward, movementSpeed);

        proyectile = Instantiate(proyectilePrefab, leftCannon.position, Quaternion.identity).GetComponent<Proyectile>();
        proyectile.SetMovement(forward, movementSpeed);
    }
}