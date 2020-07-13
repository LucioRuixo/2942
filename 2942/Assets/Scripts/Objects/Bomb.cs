using System;
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    int damage;

    float detonationTime;

    public static event Action<int> onBombExplosion;

    void Start()
    {
        StartCoroutine(WaitToExplode());
    }

    public void Initialize(float detonationTime, int damage)
    {
        this.detonationTime = detonationTime;
        this.damage = damage;
    }

    IEnumerator WaitToExplode()
    {
        yield return new WaitForSeconds(detonationTime);

        if (onBombExplosion != null)
            onBombExplosion(damage);

        Destroy(gameObject);
    }
}