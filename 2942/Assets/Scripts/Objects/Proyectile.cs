using UnityEngine;

public class Proyectile : MonoBehaviour
{
    bool playerProyectile;
    bool powerPlusOn;

    int damage;

    float speedMultiplier = 1.5f;
    float powerPlusSizeMultiplier = 2f;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    Vector3 movement;

    void OnEnable()
    {
        LevelManager.onNextLevelSetting += Destroy;
    }

    void Start()
    {
        if (powerPlusOn)
        {
            Vector3 scale = transform.localScale;
            scale.x *= powerPlusSizeMultiplier;
            scale.y *= powerPlusSizeMultiplier;
            transform.localScale = scale;

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size *= powerPlusSizeMultiplier;
        }

        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
    }                      

    void OnTriggerEnter2D(Collider2D collision)
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

    void OnDisable()
    {
        LevelManager.onNextLevelSetting -= Destroy;
    }

    bool OffScreen()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        return x > leftScreenLimit + height || x < rightScreenLimit - height
               ||
               y < lowerScreenLimit - height || y > upperScreenLimit + height ? true : false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    public void InitializeAsPlayerProyectile(bool powerPlusOn, int damage, float movementSpeed, Vector3 forward)
    {
        playerProyectile = true;
        this.powerPlusOn = powerPlusOn;
        this.damage = damage;
        movement = forward * movementSpeed * speedMultiplier;
    }

    public void InitializeAsEnemyProyectile(int damage, float movementSpeed, Vector3 forward)
    {
        playerProyectile = false;
        powerPlusOn = false;
        this.damage = damage;
        movement = forward * movementSpeed * speedMultiplier;
    }

    public void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }
}