using UnityEngine;

public class EnemyView : MonoBehaviour
{
    GameObject explosionPrefab;
    Transform explosionContainer;

    public void SetExplosion(GameObject explosionPrefab, Transform explosionContainer)
    {
        this.explosionPrefab = explosionPrefab;
        this.explosionContainer = explosionContainer;
    }

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity, explosionContainer);
    }
}