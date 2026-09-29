using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InstantiateUtli : MonoBehaviour
{
    public GameObject objectToInstantiate;

   
    public void InstantiateObject(Vector2 position)
    {
        Instantiate(
       objectToInstantiate,
       position,
       Quaternion.identity
   );
    }
}
