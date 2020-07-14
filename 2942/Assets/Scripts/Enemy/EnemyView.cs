using System.Collections;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    bool damageColorOn = false;

    float damageColorDuration;

    public ParticleSystem thrustParticleSystem;
    GameObject explosionPrefab;
    Transform explosionContainer;
    Color damageColor;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        thrustParticleSystem.Play();
    }

    public void CheckIfDamageColorOn()
    {
        if (!damageColorOn)
            StartCoroutine(ChangeColorOnDamage());
    }

    public void InitializeDamageColor(Color damageColor, float damageColorDuration)
    {
        this.damageColor = damageColor;
        this.damageColorDuration = damageColorDuration;
    }

    public void InitializeExplosion(GameObject explosionPrefab, Transform explosionContainer)
    {
        this.explosionPrefab = explosionPrefab;
        this.explosionContainer = explosionContainer;
    }

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity, explosionContainer);
    }

    IEnumerator ChangeColorOnDamage()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(damageColorDuration);

        spriteRenderer.color = Color.white;
    }
}