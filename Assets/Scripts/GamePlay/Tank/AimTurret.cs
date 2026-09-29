using UnityEngine;



public class AimTurret: MonoBehaviour
{
    

    

    [SerializeField] private  Transform tankTurretParent;



    [SerializeField] private float turretRotationSpeed = 800f;

    [SerializeField]
    private float visualOffset = -90f;

    

    private void Awake()
    {
        if(tankTurretParent == null)
        {
            Debug.LogError("TankTurretParent is not assigned!");
            enabled = false;
            return;
        }
    }




    public void RotateTurret(Vector2 targetWorldPosition)
    {
        
        Vector2 direction = targetWorldPosition - (Vector2)tankTurretParent.position;

        float desiredAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        float localAngle = desiredAngle - tankTurretParent.parent.eulerAngles.z - visualOffset;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, localAngle);
        float maxRotationThisFrame = turretRotationSpeed * Time.deltaTime;

        tankTurretParent.localRotation = Quaternion.RotateTowards(tankTurretParent.localRotation,targetRotation,maxRotationThisFrame);

    }



}

