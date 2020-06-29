using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public ShipSO stats;

    public int Life { set;  get; }
    public int Damage { set; get; }

    public float MovementSpeed { set; get; }
    public float ShootingInterval { set; get; }

    void Start()
    {
        Life = stats.life;
        Damage = stats.damage;

        MovementSpeed = stats.movementSpeed;
        ShootingInterval = stats.shootingInterval;
    }

    public void TakeDamage(int damage)
    {
        Life -= damage;

        if (Life <= 0)
            Destroy(gameObject);
    }
}