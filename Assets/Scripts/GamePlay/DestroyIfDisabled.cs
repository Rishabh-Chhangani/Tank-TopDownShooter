using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfDisabled : MonoBehaviour
{
    public bool SelfDestructionEnabled { get; set; } = false;
    public bool SelfDistructionEnabled { get => SelfDestructionEnabled; set => SelfDestructionEnabled = value; }

    private void OnDisable()
    {
        if (SelfDestructionEnabled)
            Destroy(gameObject);
    }
}
