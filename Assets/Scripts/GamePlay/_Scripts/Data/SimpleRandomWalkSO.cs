using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SimpleRandomWalkSO_", menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    [SerializeField] private int iteration = 10;
    [SerializeField] private int walkLength = 10;
    [SerializeField] private bool startRandomlyEachIteration = true;

    public int Iteration => iteration;
    public int WalkLength => walkLength;
    public bool StartRandomlyEachIteration => startRandomlyEachIteration;
}
