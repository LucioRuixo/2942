using UnityEngine;

public class ShipModel : MonoBehaviour
{
    public ShipSO stats;

    public float MovementSpeed;
    public float ShootingInterval;

    void Start()
    {
        MovementSpeed = stats.movementSpeed;
        ShootingInterval = stats.shootingInterval;
    }
}