using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTankMovementData", menuName = "Data/TankMovement")]
public class TankMovementData : ScriptableObject
{
    public float rotationSpeed = 180f;
    public float maxSpeed = 10f;
    public float acceleration = 70;
    public float deceleration = 50;

}
