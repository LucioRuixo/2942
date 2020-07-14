using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{
    public int itemGenerationPercentage;

    public float minWaitTime;
    public float maxWaitTime;

    public EnemyManager.EnemyTypes type;

    public GameObject prefab;
}
