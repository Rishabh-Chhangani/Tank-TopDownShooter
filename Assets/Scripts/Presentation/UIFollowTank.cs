using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowTank : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    public Transform ObjectToFollow
    {
        get => objectToFollow;
        set => objectToFollow = value;
    }
    [SerializeField]
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(objectToFollow != null)
        {
            rectTransform.anchoredPosition = objectToFollow.localPosition;
        }
    }
}
