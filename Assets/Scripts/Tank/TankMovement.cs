
using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]

public class TankMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 movementVector;
    public TankMovementData movementData;

    public float currentSpeed = 0;
    public float currentDriveDirection = 1;

    public event Action<float> OnSpeedChange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found.");
            enabled = false;
            return;
        }

    }

    public void MoveTank(Vector2 movementVector)
    {
        this.movementVector = -movementVector;


    }
    private void UpdateSpeed(Vector2 movementInput)
    {
        float verticalInput = movementInput.y;

        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            currentSpeed += movementData.acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, movementData.maxSpeed);

            currentDriveDirection = Mathf.Sign(verticalInput);
        }
        else
        {
            currentSpeed -= movementData.deceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, movementData.maxSpeed);
        }

        OnSpeedChange?.Invoke(currentSpeed);
    }

    private void RotateTank(Vector2 movementInput)
    {
        rb.MoveRotation(rb.rotation + movementInput.x * movementData.rotationSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {


        UpdateSpeed(movementVector);
        RotateTank(movementVector);
        rb.velocity = (Vector2)transform.up * currentSpeed * currentDriveDirection;

    }
}
