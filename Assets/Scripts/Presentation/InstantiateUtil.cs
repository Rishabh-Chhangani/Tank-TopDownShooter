using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateUtil : MonoBehaviour
{
    public GameObject objectToInstantiate;

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