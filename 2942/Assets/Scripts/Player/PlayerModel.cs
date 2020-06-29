using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public ShipSO stats;

    public int Life { set; get; }
    public int Damage { set; get; }

    public float MovementSpeed { set; get; }
    public float ShootingInterval { set; get; }

    public static event Action<int> onLifeUpdate;

    void Start()
    {
        Life = stats.life;
        Damage = stats.damage;

        MovementSpeed = stats.movementSpeed;
        ShootingInterval = stats.shootingInterval;

        if (onLifeUpdate != null)
            onLifeUpdate(Life);
    }

    public void TakeDamage(int damage)
    {
        Life -= damage;

        if (onLifeUpdate != null)
            onLifeUpdate(Life);
    }
}