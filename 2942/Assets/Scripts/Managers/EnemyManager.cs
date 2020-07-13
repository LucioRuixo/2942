using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    struct Enemy
    {
        public float minWaitTime;
        public float maxWaitTime;
        public float width;
        public float height;
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
    public Transform proyectileContainer;

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

        StartCoroutine(GenerateLocust());
        StartCoroutine(GenerateMantis());
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
    }

    void InitializeMantis()
    {
        mantis.minWaitTime = minMantisWaitTime;
        mantis.maxWaitTime = maxMantisWaitTime;
        mantis.width = mantisPrefab.transform.GetComponent<SpriteRenderer>().size.x / 2f;
        mantis.height = mantisPrefab.transform.GetComponent<SpriteRenderer>().size.y / 2f;
    }

    void ClearEnemies()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy) Destroy(enemy.gameObject);
        }

        enemies.Clear();
    }

    IEnumerator GenerateLocust()
    {
        while (this)
        {
            float positionXValue = Random.Range(rightScreenLimit + locust.width, leftScreenLimit - locust.width);

            Vector3 position = new Vector3(positionXValue, locustInitialYValue);

            Vector3 rotationEuler = new Vector3(0f, 0f, 180f);
            Quaternion rotation = Quaternion.Euler(rotationEuler);

            EnemyController newLocust = Instantiate(locustPrefab, position, rotation, transform).GetComponent<EnemyController>();
            newLocust.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
            newLocust.SetProyectileContainer(proyectileContainer);
            enemies.Add(newLocust);

            float waitTime = Random.Range(locust.minWaitTime, locust.maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator GenerateMantis()
    {
        while (this)
        {
            bool rightSide = Random.Range(0, 1) % 2 == 0 ? true : false;

            float positionXValue = rightSide ? leftScreenLimit - mantis.height : rightScreenLimit + mantis.height;
            float positionYValue = Random.Range(lowerScreenLimit + mantis.width, upperScreenLimit - mantis.width);
            Vector3 position = new Vector3(positionXValue, positionYValue);

            Vector3 rotationEuler = Vector3.zero;
            rotationEuler.z = rightSide ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(rotationEuler);

            EnemyController newMantis = Instantiate(mantisPrefab, position, rotation, transform).GetComponent<EnemyController>();
            newMantis.SetScreenLimits(leftScreenLimit, rightScreenLimit, upperScreenLimit, lowerScreenLimit);
            newMantis.SetProyectileContainer(proyectileContainer);
            enemies.Add(newMantis);

            float waitTime = Random.Range(mantis.minWaitTime, mantis.maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}