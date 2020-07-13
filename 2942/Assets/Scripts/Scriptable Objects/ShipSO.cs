using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class ShipSO : ScriptableObject
{
    public int energy;
    public int damage;

    public float movementSpeed;
    public float shootingInterval;
}