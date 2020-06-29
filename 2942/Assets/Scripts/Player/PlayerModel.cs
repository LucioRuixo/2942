using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public ShipSO stats;

    [HideInInspector] public int life;
    [HideInInspector] public int damage;

    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float shootingInterval;

    public static event Action<int> onLifeUpdate;

    void Start()
    {
        life = stats.life;
        damage = stats.damage;

        movementSpeed = stats.movementSpeed;
        shootingInterval = stats.shootingInterval;

        if (onLifeUpdate != null)
            onLifeUpdate(life);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;

        if (onLifeUpdate != null)
            onLifeUpdate(life);
    }
}