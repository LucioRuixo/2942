using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    bool damageColorOn;

    public float damageColorChangeDuration;

    public Color damageColor;
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

    IEnumerator ChangeColorOnDamage()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(damageColorChangeDuration);

        spriteRenderer.color = Color.white;
    }
}