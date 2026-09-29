using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultEnemyAI : MonoBehaviour
{

    //This is an base for or Strategy Pattern for AI, we can have different behaviours for different AI types, and we can switch between them based on the situation.


    [SerializeField]
    private AIBehaviour shootBehaviour, patrolBehaviour;


    //TankContoller Equivalent- combination of Movement, Weapon, Look, Input others will be added as need 
    [SerializeField]
    private TankController tank;
   
    [SerializeField]
    private AIDetector aiDetector;

    private void Awake()
    {
        aiDetector = GetComponentInChildren<AIDetector>();
        tank = GetComponentInChildren<TankController>();   
    }

    private void Update()
    {
        if(aiDetector.TargetVisible)
        {
            shootBehaviour.PerformAction(tank, aiDetector);
        }
        else
        {
            patrolBehaviour.PerformAction(tank, aiDetector);
        }
    }
}
