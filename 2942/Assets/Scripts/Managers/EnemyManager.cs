using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    enum EnemyTypes
    {
        Locust,
        Mantis
    }

    struct Enemy
    {
        public float minWaitTime;
        public float maxWaitTime;
        public float width;
        public float height;

        public EnemyTypes type;

        public GameObject prefab;
    }

    public float minLocustWaitTime;
    public float maxLocustWaitTime;
    public float minMantisWaitTime;
    public float maxMantisWaitTime;
    float locustInitialYValue;
    float leftScreenLimit;
    float rightScreenLimit;
    float upperScreenLimit;
    float lowerScreenLimit;

    Enemy locust;
    Enemy mantis;

    List<EnemyController> enemies;

    public GameObject locustPrefab;
    public GameObject mantisPrefab;
    public GameObject explosionPrefab;
    public Transform proyectileContainer;
    public Transform explosionContainer;

    void OnEnable()
    {
        GameManager.onScreenLimitsSetting += SetScreenLimits;

        LevelManager.onNextLevelSetting += ClearEnemies;
    }

    void Start()
    {
        enemies = new List<EnemyController>();

        InitializeLocust();
        InitializeMantis();

        StartCoroutine(GenerateEnemy(locust));
        StartCoroutine(GenerateEnemy(mantis));
    }

    void OnDisable()
    {
        GameManager.onScreenLimitsSetting -= SetScreenLimits;

        LevelManager.onNextLevelSetting -= ClearEnemies;
    }

    void SetScreenLimits(float left, float right, float top, float bottom)
    {
        leftScreenLimit = left;
        rightScreenLimit = right;
        upperScreenLimit = top;
        lowerScreenLimit = bottom;
    }

    void InitializeLocust()
    {
        locust.minWaitTime = minLocustWaitTime;
        locust.maxWaitTime = maxLocustWaitTime;
        locust.width = locustPrefab.transform.GetComponent<SpriteRenderer>().size.x / 2f;
        locust.height = locustPrefab.transform.GetComponent<SpriteRenderer>().size.y / 2f;
        locustInitialYValue = upperScreenLimit + locust.height;

        locust.type = EnemyTypes.Locust;

        locust.prefab = locustPrefab;
    }

    void InitializeMantis()
    {
        mantis.minWaitTime = minMantisWaitTime;
        mantis.maxWaitTime = maxMantisWaitTime;
        mantis.width = mantisPrefab.transform.GetComponent<SpriteRenderer>().size.x / 2f;
        mantis.height = mantisPrefab.transform.GetComponent<SpriteRenderer>().size.y / 2f;

        mantis.type = EnemyTypes.Mantis;

        mantis.prefab = mantisPrefab;
    }

    Vector3 GetEnemyPosition(EnemyTypes type)
    {
        float positionXValue;
        Vector3 position;

        switch (type)
        {
            case EnemyTypes.Locust:
                positionXValue = UnityEngine.Random.Range(rightScreenLimit + locust.width, leftScreenLimit - locust.width);
                position = new Vector3(positionXValue, locustInitialYValue);
                break;
            case EnemyTypes.Mantis:
                bool rightSide = UnityEngine.Random.Range(0, 1) % 2 == 0 ? true : false;
                positionXValue = rightSide ? leftScreenLimit - mantis.height : rightScreenLimit + mantis.height;
                float positionYValue = UnityEngine.Random.Range(lowerScreenLimit + mantis.width, upperScreenLimit - mantis.width);
                position = new Vector3(positionXValue, positionYValue);
                break;
            default:
                position = Vector3.zero;
                break;
        }

        return position;
    }

    Quaternion GetEnemyRotation(EnemyTypes type)
    {
        Vector3 rotationEuler;
        Quaternion rotation;

        switch (type)
        {
            case EnemyTypes.Locust:
                rotationEuler = new Vector3(0f, 0f, 180f);
                rotation = Quaternion.Euler(rotationEuler);
                break;
            case EnemyTypes.Mantis:
                rotationEuler = Vector3.zero;
                bool rightSide = UnityEngine.Random.Range(0, 1) % 2 == 0 ? true : false;
                rotationEuler.z = rightSide ? 90f : -90f;
                rotation = Quaternion.Euler(rotationEuler);
                break;
            default:
                rotation = Quaternion.identity;
                break;
        }

        return rotation;
    }

    void ClearEnemies()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy) Destroy(enemy.gameObject);
        }

        enemies.Clear();
    }

    IEnumerator GenerateEnemy(Enemy enemy)
    {
        while (this)
        {
            Vector3 position = GetEnemyPosition(enemy.type);
            Quaternion rotation = GetEnemyRotation(enemy.type);

            EnemyController newController = Instantiate(enemy.prefab, position, rotation, transform).GetComponent<EnemyController>();
            if (newController)
            {
                newController.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
                newController.SetProyectileContainer(proyectileContainer);
                enemies.Add(newController);

                EnemyView newView = newController.gameObject.GetComponent<EnemyView>();
                newView.SetExplosion(explosionPrefab, explosionContainer);
            }

            float waitTime = UnityEngine.Random.Range(enemy.minWaitTime, enemy.maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}