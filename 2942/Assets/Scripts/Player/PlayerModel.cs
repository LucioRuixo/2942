using System;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public ShipSO stats;

    public int score;

    [Header("From scriptable object: ")]
    public int energy;
    public int damage;

    public float movementSpeed;
    public float shootingInterval;

    public static event Action<int> onEnergyUpdate;
    public static event Action<int> onScoreUpdate;
    public static event Action onDamageTaken;
    public static event Action<bool, int> onDeath;
    public static event Action<bool, int> onVictory;

    void OnEnable()
    {
        LevelManager.onLastLevelCompleted += SetVictoryFinalScore;

        EnemyModel.onDeath += IncreaseScore;
    }

    void Start()
    {
        energy = stats.energy;
        damage = stats.damage;
        score = 0;

        movementSpeed = stats.movementSpeed;
        shootingInterval = stats.shootingInterval;

        if (onEnergyUpdate != null)
            onEnergyUpdate(energy);

        if (onScoreUpdate != null)
            onScoreUpdate(score);
    }

    void OnDisable()
    {
        LevelManager.onLastLevelCompleted += SetVictoryFinalScore;

        EnemyModel.onDeath -= IncreaseScore;
    }

    void IncreaseScore()
    {
        score += 100;

        if (onScoreUpdate != null)
            onScoreUpdate(score);
    }

    void SetVictoryFinalScore()
    {
        if (onVictory != null)
            onVictory(true, score);
    }

    public void TakeDamage(int damage)
    {
        energy = Mathf.Clamp(energy - damage, 0, 100);

        if (onEnergyUpdate != null)
            onEnergyUpdate(energy);

        if (onDamageTaken != null)
            onDamageTaken();

        if (energy == 0)
        {
            if (onDeath != null)
                onDeath(false, score);

            Destroy(gameObject);
        }
    }

    public void RefillEnergy()
    {
        energy = 100;

        if (onEnergyUpdate != null)
            onEnergyUpdate(energy);
    }
}