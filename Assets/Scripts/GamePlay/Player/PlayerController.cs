using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputHandler inputHandler;
    [SerializeField]
    private TankController tankController;



    [SerializeField]
    private Camera mainCamera;

    



    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        tankController = GetComponentInChildren<TankController>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        tankController.HandleTankMovement(inputHandler.MoveInput);
        tankController.HandleTurretRotation(GetMouseWorldPosition(inputHandler.LookInput));
    }

   public Vector2 GetMouseWorldPosition(Vector3 mouseScreenPosition)
    {
        mouseScreenPosition.z = mainCamera.nearClipPlane;
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.FirePerformed += HandleShoot;
        }
    }
    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.FirePerformed -= HandleShoot;
        }
    }

    public void HandleShoot()
    {
        tankController.HandleShoot();
    }
}
