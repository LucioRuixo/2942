using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public enum Types
    {
        Locust,
        Mantis
    }

    struct EnemyData
    {
        public int itemGenerationPercentage;

        public float minWaitTime;
        public float maxWaitTime;
        public float height;
        public float width;

        public Types type;

        public GameObject prefab;
    }

    bool active;

    float leftScreenLimit;
    float rightScreenLimit;
    float upperScreenLimit;
    float lowerScreenLimit;

    public GameObject locustPrefab;
    public GameObject mantisPrefab;
    public GameObject explosionPrefab;
    public Transform proyectileContainer;
    public Transform explosionContainer;

    public List<EnemySO> enemySOs;
    List<EnemyData> enemies;
    List<EnemyController> currentEnemies;

    [Header("General enemy attributes")]
    public int collisionDamage;

    public float initialWaitTime;

    public Color damageColor;
    public float damageColorDuration;

    void OnEnable()
    {
        GameManager.onScreenLimitsSetting += SetScreenLimits;

        LevelManager.onNewLevelSetting += SetOnNewLevel;
        LevelManager.onLevelEndReached += StopGeneration;
        LevelManager.onLastLevelCompleted += StopGeneration;
    }

    void Start()
    {
        enemies = new List<EnemyData>();
        currentEnemies = new List<EnemyController>();

        for (int i = 0; i < enemySOs.Count; i++)
        {
            enemies.Add(Initialize(enemySOs[i]));
        }
    }

    void OnDisable()
    {
        GameManager.onScreenLimitsSetting -= SetScreenLimits;

        LevelManager.onNewLevelSetting -= SetOnNewLevel;
        LevelManager.onLevelEndReached -= StopGeneration;
        LevelManager.onLastLevelCompleted -= StopGeneration;
    }

    void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }

    EnemyData Initialize(EnemySO enemySO)
    {
        EnemyData newEnemy;

        newEnemy.itemGenerationPercentage = enemySO.itemGenerationPercentage;
        newEnemy.minWaitTime = enemySO.minWaitTime;
        newEnemy.maxWaitTime = enemySO.maxWaitTime;
        newEnemy.type = enemySO.type;
        newEnemy.prefab = enemySO.prefab;

        newEnemy.height = newEnemy.prefab.GetComponent<SpriteRenderer>().size.y / 2f;
        newEnemy.width = newEnemy.prefab.GetComponent<SpriteRenderer>().size.x / 2f;

        return newEnemy;
    }

    Vector2 SetNewEnemyPosition(EnemyData enemy)
    {
        float positionXValue;
        Vector2 position;

        switch (enemy.type)
        {
            case Types.Locust:
                positionXValue = Random.Range(rightScreenLimit + enemy.width, leftScreenLimit - enemy.width);
                float initialYValue = upperScreenLimit + enemy.height;
                position = new Vector2(positionXValue, initialYValue);
                break;
            case Types.Mantis:
                bool rightSide = Random.Range(0, 2) % 2 == 0 ? true : false;
                positionXValue = rightSide ? leftScreenLimit - enemy.height : rightScreenLimit + enemy.height;
                float positionYValue = Random.Range(lowerScreenLimit + enemy.width, upperScreenLimit - enemy.width);
                position = new Vector2(positionXValue, positionYValue);
                break;
            default:
                position = Vector2.zero;
                break;
        }

        return position;
    }

    Quaternion SetNewEnemyRotation(EnemyData enemy, Vector3 position)
    {
        Vector3 rotationEuler;
        Quaternion rotation;

        switch (enemy.type)
        {
            case Types.Locust:
                rotationEuler = new Vector3(0f, 0f, 180f);
                rotation = Quaternion.Euler(rotationEuler);
                break;
            case Types.Mantis:
                rotationEuler = Vector3.zero;
                bool rightSide = position.x > 0f ? true : false;
                rotationEuler.z = rightSide ? 90f : -90f;
                rotation = Quaternion.Euler(rotationEuler);
                break;
            default:
                rotation = Quaternion.identity;
                break;
        }

        return rotation;
    }

    void SetOnNewLevel()
    {
        if (currentEnemies.Count > 0)
            Clear();

        StartCoroutine(WaitToGenerate());
    }

    void StopGeneration()
    {
        active = false;
    }

    void Clear()
    {
        foreach (EnemyController enemy in currentEnemies)
        {
            if (enemy) Destroy(enemy.gameObject);
        }

        currentEnemies.Clear();
    }

    IEnumerator WaitToGenerate()
    {
        yield return new WaitForSeconds(initialWaitTime);

        active = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            StartCoroutine(Generate(enemies[i]));
        }
    }

    IEnumerator Generate(EnemyData enemy)
    {
        while (active)
        {
            Vector2 position = SetNewEnemyPosition(enemy);
            Quaternion rotation = SetNewEnemyRotation(enemy, position);

            EnemyController newController = Instantiate(enemy.prefab, position, rotation, transform).GetComponent<EnemyController>();
            if (newController)
            {
                newController.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
                newController.SetProyectileContainer(proyectileContainer);
                currentEnemies.Add(newController);

                EnemyModel newModel = newController.gameObject.GetComponent<EnemyModel>();
                newModel.Initialize(collisionDamage, enemy.itemGenerationPercentage);

                EnemyView newView = newController.gameObject.GetComponent<EnemyView>();
                newView.InitializeDamageColor(damageColor, damageColorDuration);
                newView.InitializeExplosion(explosionPrefab, explosionContainer);
            }

            float waitTime = Random.Range(enemy.minWaitTime, enemy.maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}