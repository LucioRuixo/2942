using UnityEngine;

public class Bomb : MonoBehaviour
{
    float timeToExplode = 3f;
    float explosionTimer = 0f;

    void Update()
    {
        explosionTimer += Time.deltaTime;
        
        if (explosionTimer >= timeToExplode)
            Destroy(gameObject);
    }
}