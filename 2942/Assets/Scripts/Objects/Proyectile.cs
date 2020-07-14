using UnityEngine;

public class Proyectile : MonoBehaviour
{
    bool playerProyectile;
    bool powerPlusOn;

    int damage;

    float speedMultiplier = 1.5f;
    float powerPlusSizeMultiplier = 3f;
    float rotationAngleRange = 6f;
    float movementSpeed;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    Vector3 movement;

    void OnEnable()
    {
        LevelManager.onNewLevelSetting += Destroy;
    }

    void Start()
    {
        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;

        SetRotation();
        movement = -transform.up * movementSpeed * speedMultiplier;

        if (powerPlusOn)
            ApplyPowerPlus();
    }                      

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Enemy" && playerProyectile)
            ||
            (collision.tag == "Player" && !playerProyectile))
            Destroy(gameObject);
    }

    void Update()
    {
        transform.position += movement * Time.deltaTime;

        if (OffScreen())
            Destroy(gameObject);
    }

    void OnDisable()
    {
        LevelManager.onNewLevelSetting -= Destroy;
    }

    void SetRotation()
    {
        float addedRotationZ = Random.Range(-(rotationAngleRange / 2f), rotationAngleRange / 2f);
        Vector3 addedRotationEuler = new Vector3(0f, 0f, addedRotationZ);
        Quaternion addedRotation = Quaternion.Euler(addedRotationEuler);

        transform.rotation *= addedRotation;
    }

    void ApplyPowerPlus()
    {
        Vector3 scale = transform.localScale;
        scale.x *= powerPlusSizeMultiplier;
        scale.y *= powerPlusSizeMultiplier;
        transform.localScale = scale;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size *= powerPlusSizeMultiplier;
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

    public void InitializeAsPlayerProyectile(bool powerPlusOn, int damage, float movementSpeed)
    {
        playerProyectile = true;
        this.powerPlusOn = powerPlusOn;
        this.damage = damage;
        this.movementSpeed = movementSpeed;
    }

    public void InitializeAsEnemyProyectile(int damage, float movementSpeed)
    {
        playerProyectile = false;
        powerPlusOn = false;
        this.damage = damage;
        this.movementSpeed = movementSpeed;
    }

    public void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }

    public int GetDamage()
    {
        return damage;
    }
}