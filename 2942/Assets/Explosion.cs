using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem ps;

    void Start()
    {
        ps.Play();
    }

    void Update()
    {
        if (!ps.isPlaying)
            Destroy(gameObject);
    }
}
