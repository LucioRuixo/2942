﻿using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyModel model;
    public EnemyView view;

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

    public GameObject proyectilePrefab;
    public Transform rightCannon;
    public Transform leftCannon;
    Transform proyectileContainer;

    void Start()
    {
        damage = model.damage;

        height = transform.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        shootingInterval = model.shootingInterval;

        movementSpeed = model.movementSpeed;
        forward = transform.up;
        movement = forward * movementSpeed;

        state = States.NotShooting;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player Proyectile" || collision.tag == "Homing Missile" || collision.tag == "Explosion")
        {
            int damageTaken;
            switch (collision.tag)
            {
                case "Player Proyectile":
                    damageTaken = collision.GetComponent<Proyectile>().GetDamage();
                    break;
                case "Homing Missile":
                    damageTaken = collision.GetComponent<HomingMissile>().GetDamage();
                    break;
                case "Explosion":
                    damageTaken = collision.GetComponent<Explosion>().GetDamage();
                    break;
                default:
                    damageTaken = 0;
                    break;
            }

            model.TakeDamage(damageTaken);
            view.CheckIfDamageColorOn();
        }
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
        Vector3 addedRotationEuler = new Vector3(0f, 0f, 180f);
        Quaternion addedRotation = Quaternion.Euler(addedRotationEuler);
        Quaternion rotation = transform.rotation * addedRotation;

        Proyectile newProyectile;

        newProyectile = Instantiate(proyectilePrefab, rightCannon.position, rotation, proyectileContainer).GetComponent<Proyectile>();
        newProyectile.InitializeAsEnemyProyectile(damage, movementSpeed);
        newProyectile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);

        newProyectile = Instantiate(proyectilePrefab, leftCannon.position, rotation, proyectileContainer).GetComponent<Proyectile>();
        newProyectile.InitializeAsEnemyProyectile(damage, movementSpeed);
        newProyectile.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);

        SoundManager.Get().PlaySound(SoundManager.Sounds.EnemyShot);
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

    public void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }

    public void SetProyectileContainer(Transform proyectileContainer)
    {
        this.proyectileContainer = proyectileContainer;
    }

    public void Destroy()
    {
        view.Explode();

        Destroy(gameObject);
    }
}