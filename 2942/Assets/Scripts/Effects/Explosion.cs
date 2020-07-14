using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage;

    public ParticleSystem ps;

    void Start()
    {
        ps.Play();

        SoundManager.Get().PlaySound(SoundManager.Sounds.Explosion);
    }

    void Update()
    {
        if (!ps.isPlaying)
            Destroy(gameObject);
    }

    public int GetDamage()
    {
        return damage;
    }
}
