using UnityEngine;

public class LocustManager : MonoBehaviour
{
    float minTimeToGenerate = 0.1f;
    float maxTimeToGenerate = 1f;
    float generationTimer = 0f;
    float timeToGenerate;
    float width;
    float height;
    float initialYValue;
    float leftScreenLimit;
    float rightScreenLimit;
    float upperScreenLimit;

    public GameManager gameManager;
    public GameObject locustPrefab;

    void Start()
    {
        timeToGenerate = Random.Range(minTimeToGenerate, maxTimeToGenerate);
        width = locustPrefab.transform.GetComponent<SpriteRenderer>().size.x / 2f;
        height = locustPrefab.transform.GetComponent<SpriteRenderer>().size.y / 2f;
        leftScreenLimit = gameManager.leftScreenLimit;
        rightScreenLimit = gameManager.rightScreenLimit;
        upperScreenLimit = gameManager.upperScreenLimit;
        initialYValue = upperScreenLimit + height;
    }

    void Update()
    {
        generationTimer += Time.deltaTime;

        if (generationTimer >= timeToGenerate)
        {
            GenerateLocust();

            generationTimer = 0f;
            timeToGenerate = Random.Range(minTimeToGenerate, maxTimeToGenerate);
        }
    }

    void GenerateLocust()
    {
        float positionXValue = Random.Range(rightScreenLimit + width, leftScreenLimit - width);

        Vector3 position = new Vector3(positionXValue, initialYValue);

        Vector3 rotationEuler = new Vector3(0f, 0f, 180f);
        Quaternion rotation = Quaternion.Euler(rotationEuler);

        Instantiate(locustPrefab, position, rotation, transform);
    }
}