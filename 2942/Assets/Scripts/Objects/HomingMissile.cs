using UnityEngine;

public class HomingMissile : MonoBehaviour
{
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

    Transform target;

    void OnEnable()
    {
        LevelManager.onNewLevelSetting += Destroy;
    }

    void Start()
    {
        movement = transform.up * movementSpeed * speedMultiplier;

        if (powerPlusOn)
            ApplyPowerPlus();

        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
            Destroy(gameObject);
    }

    void Update()
    {
        if (target)
        {
            RotateTowardsTarget();
            movement = transform.up * movementSpeed * speedMultiplier;
        }
        transform.position += movement * Time.deltaTime;

        if (OffScreen())
            Destroy(gameObject);
    }

    void OnDisable()
    {
        LevelManager.onNewLevelSetting -= Destroy;
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

    void RotateTowardsTarget()
    {
        bool targetOnRight = target.position.x > transform.position.x;

        Vector2 directionToTarget = target.position - transform.position;

        Debug.Log("Forward: " + transform.up + ", To target: " + directionToTarget);
        Debug.DrawRay(transform.position, directionToTarget, Color.red);
        Debug.DrawRay(transform.position, transform.up * 50, Color.blue);

        float angle = Vector2.Angle(transform.up, directionToTarget);
        if (targetOnRight)
            angle *= -1;

        float addedRotationEulerZ = Mathf.Clamp(angle, -rotationAngleRange / 2f, rotationAngleRange / 2f);
        Vector3 addedRotationEuler = new Vector3(0f, 0f, addedRotationEulerZ);
        Quaternion addedRotation = Quaternion.Euler(addedRotationEuler);

        transform.rotation *= addedRotation;
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

    public void Initialize(bool powerPlusOn, int damage, float movementSpeed)
    {
        this.powerPlusOn = powerPlusOn;
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

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public int GetDamage()
    {
        return damage;
    }
}