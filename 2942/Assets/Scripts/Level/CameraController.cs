using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool shaking = false;

    public int shakePointsAmount;

    public float shakeMagnitude;
    public float shakeDuration;
    float journeyDuration;
    float positionZ;

    void OnEnable()
    {
        PlayerController.onCollisionWithProyectile += CheckIfShaking;
    }

    void Start()
    {
        journeyDuration = shakeDuration / (shakePointsAmount + 1);
        positionZ = transform.position.z;
    }

    void OnDisable()
    {
        PlayerController.onCollisionWithProyectile -= CheckIfShaking;
    }

    void CheckIfShaking()
    {
        if (!shaking)
            StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        shaking = true;

        List<Vector2> points = new List<Vector2>();

        Vector2 newPoint = new Vector2(Random.Range(-shakeMagnitude, shakeMagnitude), Random.Range(-shakeMagnitude, shakeMagnitude));
        points.Add(newPoint);
        for (int i = 0; i < shakePointsAmount - 1; i++)
        {
            newPoint.x = newPoint.x > 0f ? Random.Range(-shakeMagnitude, 0f) : Random.Range(0f, shakeMagnitude);
            newPoint.y = newPoint.y > 0f ? Random.Range(-shakeMagnitude, 0f) : Random.Range(0f, shakeMagnitude);
            points.Add(newPoint);
        }

        Vector3 position;
        Vector2 a;
        Vector2 b;
        for (int i = 0; i < shakePointsAmount + 1; i++)
        {
            a = transform.position;
            b = i < shakePointsAmount ? points[i] : Vector2.zero;

            float journeyLength = Vector2.Distance(a, b);
            float fractionToMove = (journeyLength * Time.deltaTime) / journeyDuration;
            while ((Vector2)transform.position != b)
            {
                float distanceCovered = Vector2.Distance(a, (Vector2)transform.position);
                float fractionMoved = distanceCovered / journeyLength;

                position = transform.position;
                position = Vector2.Lerp(a, b, fractionMoved + fractionToMove);
                position.z = positionZ;
                transform.position = position;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        shaking = false;
    }
}