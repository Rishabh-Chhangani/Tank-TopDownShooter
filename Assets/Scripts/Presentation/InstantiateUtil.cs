using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateUtil : MonoBehaviour
{
    [SerializeField] private GameObject objectToInstantiate;

    public GameObject ObjectToInstantiate
    {
        get => objectToInstantiate;
        set => objectToInstantiate = value;
    }

    public void InstantiateObject(Vector2 position)
    {
        if (objectToInstantiate == null)
            return;

        Instantiate(
            objectToInstantiate,
            position,
            Quaternion.identity
        );
    }
}