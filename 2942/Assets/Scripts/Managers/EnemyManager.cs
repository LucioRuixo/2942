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

    bool active;

    float locustInitialYValue;
    float leftScreenLimit;
    float rightScreenLimit;
    float upperScreenLimit;
    float lowerScreenLimit;

    Enemy locust;
    Enemy mantis;

    public GameObject locustPrefab;
    public GameObject mantisPrefab;
    public GameObject explosionPrefab;
    public Transform proyectileContainer;
    public Transform explosionContainer;

    List<EnemyController> enemies;

    [Header("Enemy attributes")]
    public int itemGenerationPercentage;

    public float initialWaitTime;
    public float minLocustWaitTime;
    public float maxLocustWaitTime;
    public float minMantisWaitTime;
    public float maxMantisWaitTime;

    void OnEnable()
    {
        GameManager.onScreenLimitsSetting += SetScreenLimits;

        LevelManager.onNewLevelSetting += SetOnNewLevel;
        LevelManager.onLevelEndReached += StopGeneration;
        LevelManager.onLastLevelCompleted += StopGeneration;
    }

    void Start()
    {
        enemies = new List<EnemyController>();

        InitializeLocust();
        InitializeMantis();

        StartCoroutine(Generate(locust));
        StartCoroutine(Generate(mantis));
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

    Vector3 SetNewEnemyPosition(EnemyTypes type)
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

    Quaternion SetNewEnemyRotation(EnemyTypes type)
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

    void SetOnNewLevel()
    {
        if (enemies.Count > 0)
            Clear();

        StartCoroutine(WaitToGenerate());
    }

    void StopGeneration()
    {
        active = false;
    }

    void Clear()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy) Destroy(enemy.gameObject);
        }

        enemies.Clear();
    }

    IEnumerator WaitToGenerate()
    {
        yield return new WaitForSeconds(initialWaitTime);

        active = true;
        StartCoroutine(Generate(locust));
        StartCoroutine(Generate(mantis));
    }

    IEnumerator Generate(Enemy enemy)
    {
        while (active)
        {
            Vector3 position = SetNewEnemyPosition(enemy.type);
            Quaternion rotation = SetNewEnemyRotation(enemy.type);

            EnemyController newController = Instantiate(enemy.prefab, position, rotation, transform).GetComponent<EnemyController>();
            if (newController)
            {
                newController.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
                newController.SetProyectileContainer(proyectileContainer);
                enemies.Add(newController);

                EnemyModel newModel = newController.gameObject.GetComponent<EnemyModel>();
                newModel.SetItemGenerationPercentage(itemGenerationPercentage);

                EnemyView newView = newController.gameObject.GetComponent<EnemyView>();
                newView.SetExplosion(explosionPrefab, explosionContainer);
            }

            float waitTime = UnityEngine.Random.Range(enemy.minWaitTime, enemy.maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}