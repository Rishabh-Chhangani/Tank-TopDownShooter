using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTankMovementData", menuName = "Data/TankMovement")]
public class TankMovementData : ScriptableObject
{
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 70f;
    [SerializeField] private float deceleration = 50f;

    public float RotationSpeed => rotationSpeed;
    public float MaxSpeed => maxSpeed;
    public float Acceleration => acceleration;
    public float Deceleration => deceleration;

}
