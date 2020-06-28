using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class ShipSO : ScriptableObject
{
    public int life;

    public float movementSpeed;
    public float shootingInterval;
}