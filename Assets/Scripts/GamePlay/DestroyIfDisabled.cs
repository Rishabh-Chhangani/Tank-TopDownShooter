using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfDisabled : MonoBehaviour
{
    public bool SelfDistructionEnabled { get; set; } = false;

    private void OnDestroy()
    {
        if(SelfDistructionEnabled)
        {
            Destroy(gameObject);
        }
    }

}
