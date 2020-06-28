using UnityEngine;

public class MantisManager : MonoBehaviour
{
    float minTimeToGenerate = 0.75f;
    float maxTimeToGenerate = 3f;
    float generationTimer = 0f;
    float timeToGenerate;
    float width;
    float height;
    float leftScreenLimit;
    float rightScreenLimit;
    float lowerScreenLimit;
    float upperScreenLimit;

    public GameManager gameManager;
    public GameObject mantisPrefab;

    void Start()
    {
        timeToGenerate = Random.Range(minTimeToGenerate, maxTimeToGenerate);
        width = mantisPrefab.transform.GetComponent<SpriteRenderer>().size.x / 2f;
        height = mantisPrefab.transform.GetComponent<SpriteRenderer>().size.y / 2f;
        leftScreenLimit = gameManager.leftScreenLimit;
        rightScreenLimit = gameManager.rightScreenLimit;
        lowerScreenLimit = gameManager.lowerScreenLimit;
        upperScreenLimit = gameManager.upperScreenLimit;
    }

    void Update()
    {
        generationTimer += Time.deltaTime;

        if (generationTimer >= timeToGenerate)
        {
            GenerateMantis();

            generationTimer = 0f;
            timeToGenerate = Random.Range(minTimeToGenerate, maxTimeToGenerate);
        }
    }

    void GenerateMantis()
    {
        bool rightSide = Random.Range(0, 1) % 2 == 0 ? true : false;

        float positionXValue = rightSide ? leftScreenLimit - height : rightScreenLimit + height;
        float positionYValue = Random.Range(lowerScreenLimit + width, upperScreenLimit - width);
        Vector3 position = new Vector3(positionXValue, positionYValue);

        Vector3 rotationEuler = Vector3.zero;
        rotationEuler.z = rightSide ? 90f : -90f;
        Quaternion rotation = Quaternion.Euler(rotationEuler);

        Instantiate(mantisPrefab, position, rotation, transform);
    }
}