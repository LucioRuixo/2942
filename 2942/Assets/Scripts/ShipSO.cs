using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class ShipSO : ScriptableObject
{
    public int life;
    public int damage;

    public float movementSpeed;
    public float shootingInterval;
}