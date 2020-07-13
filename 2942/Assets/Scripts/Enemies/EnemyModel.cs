﻿using System;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public ShipSO stats;
    public EnemyController controller;

    int itemGenerationPercentage = 10;

    [Header("From scriptable object: ")]
    public int energy;
    public int damage;

    public float movementSpeed;
    public float shootingInterval;

    public static event Action onDeath;
    public static event Action<float, float> onItemGeneration;

    void Start()
    {
        energy = stats.energy;
        damage = stats.damage;

        movementSpeed = stats.movementSpeed;
        shootingInterval = stats.shootingInterval;
    }

    void CheckIfShouldGenerateItem()
    {
        int random = UnityEngine.Random.Range(0, 100);

        if (random < itemGenerationPercentage && onItemGeneration != null)
            onItemGeneration(transform.position.x, transform.position.y);
    }

    public void TakeDamage(int damage)
    {
        energy -= damage;

        if (energy <= 0)
        {
            if (onDeath != null)
                onDeath();

            CheckIfShouldGenerateItem();

            Destroy(gameObject);
        }
    }
}