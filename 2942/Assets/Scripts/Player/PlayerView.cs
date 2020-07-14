using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    bool damageColorOn;

    public Color damageColor;
    public float damageColorDuration;

    public GameObject explosionPrefab;
    public Transform explosionContainer;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CheckIfDamageColorOn()
    {
        if (!damageColorOn)
            StartCoroutine(ChangeColorOnDamage());
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