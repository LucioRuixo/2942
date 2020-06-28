using UnityEngine;

public class LocustController : MonoBehaviour
{
    public LocustModel model;

    enum States
    {
        NotShooting,
        Shooting
    }

    enum RaycastPosition
    {
        Right,
        Center,
        Left
    }

    float width;
    float height;
    float positionYGoal;
    float shootingTimer = 0f;

    Vector3 movement;
    Vector3 forward;

    States state;

    GameManager gameManager;
    public GameObject proyectilePrefab;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        width = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        positionYGoal = gameManager.lowerScreenLimit - height;

        movement = new Vector3(0f, model.MovementSpeed, 0f);
        forward = transform.up;

        state = States.NotShooting;
    }

    void Update()
    {
        transform.position -= movement * Time.deltaTime;

        if (transform.position.y <= positionYGoal)
            Destroy(gameObject);

        if (state == States.NotShooting)
        {
            if (PlayerDetected(RaycastPosition.Right) || PlayerDetected(RaycastPosition.Left) || PlayerDetected(RaycastPosition.Center))
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

            if (!PlayerDetected(RaycastPosition.Right) && !PlayerDetected(RaycastPosition.Left) && !PlayerDetected(RaycastPosition.Center))
            {
                state = States.NotShooting;
                shootingTimer = 0f;
            }
        }
    }

    bool PlayerDetected(RaycastPosition raycastPosition)
    {
        float rayDistance = 7.5f;

        Vector3 position = transform.position;

        if (raycastPosition != RaycastPosition.Center)
            position.x += raycastPosition == RaycastPosition.Right ? -width : width;
        position.y -= height;

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
        float yOffset = -0.75f;
        float proyectileYValue = transform.position.y + yOffset;

        Vector2 rightProyectilePosition = new Vector2(transform.position.x - width, proyectileYValue);
        Vector2 leftProyectilePosition = new Vector2(transform.position.x + width, proyectileYValue);

        Instantiate(proyectilePrefab, rightProyectilePosition, Quaternion.identity);
        Instantiate(proyectilePrefab, leftProyectilePosition, Quaternion.identity);
    }
}