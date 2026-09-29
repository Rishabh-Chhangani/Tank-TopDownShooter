using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerInputHandler : MonoBehaviour
{
    

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    
    public event Action FirePerformed;
    public event Action ReloadPerformed;
    public event Action PausePerformed;



 

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        
        LookInput = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        if(value.isPressed)
            FirePerformed?.Invoke();
    }

    public void OnReload(InputValue value)
    {
        if(value.isPressed)
            ReloadPerformed?.Invoke();
    }

    public void OnPause(InputValue value)
    {
        if(value.isPressed)
            PausePerformed?.Invoke();
    }

   

}
