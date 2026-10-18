using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAudio : MonoBehaviour
{
    [SerializeField]
    private TankMovement tankMovement;

    [SerializeField]
    private AudioSource audioSource;

    public float minVolume = 0.05f;
    public float maxVolume   = 0.1f;
    public float volumeIncrease = 0.01f;

    [SerializeField]
    private float currentVolume;

    private void Awake()
    {
        if (tankMovement == null)
            tankMovement = GetComponentInParent<TankMovement>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        currentVolume = minVolume;
    }

    private void OnEnable()
    {
        if (tankMovement != null)
        {
            tankMovement.OnSpeedChange += ControlEngineVolume;
        }
    }

    private void OnDisable()
    {
        if (tankMovement != null)
        {
            tankMovement.OnSpeedChange -= ControlEngineVolume;
        }
    }

    // Update is called once per frame
    void Start()
    {
        if (audioSource != null)
            audioSource.volume = currentVolume;
    }

    private void ControlEngineVolume(float speed)
    {
        if(speed > 0)
        {
            if(currentVolume < maxVolume)
            {
                currentVolume += volumeIncrease * Time.deltaTime;
               
            }
        }

        else
        {
            if(currentVolume > minVolume)
            {
                currentVolume -= volumeIncrease * Time.deltaTime;
            }
        }

        currentVolume = Mathf.Clamp(currentVolume, minVolume, maxVolume);
        audioSource.volume = currentVolume;
    }
}
