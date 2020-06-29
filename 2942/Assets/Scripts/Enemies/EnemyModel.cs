using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public ShipSO stats;

    [HideInInspector] public int life;
    [HideInInspector] public int damage;

    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float shootingInterval;

    void Start()
    {
        life = stats.life;
        damage = stats.damage;

        movementSpeed = stats.movementSpeed;
        shootingInterval = stats.shootingInterval;
    }

    public void TakeDamage(int damage)
    {
        life -= damage;

        if (life <= 0)
            Destroy(gameObject);
    }
}