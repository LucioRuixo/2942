using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float timeToExplode = 2f;
    float explosionTimer = 0f;

    public static event Action onBombExplosion;

    void Update()
    {
        explosionTimer += Time.deltaTime;
        
        if (explosionTimer >= timeToExplode)
        {
            if (onBombExplosion != null)
                onBombExplosion();

            Destroy(gameObject);
        }
    }
}