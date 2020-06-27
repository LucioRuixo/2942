using UnityEngine;

public class BombController : MonoBehaviour
{
    public BombModel model;

    float explosionTimer = 0f;

    void Update()
    {
        explosionTimer += Time.deltaTime;
        
        if (explosionTimer >= model.TimeToExplode)
            Destroy(gameObject);
    }
}