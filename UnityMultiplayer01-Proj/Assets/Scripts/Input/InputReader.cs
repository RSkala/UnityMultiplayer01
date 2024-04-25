using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Controls; // For IPlayerActions, which was generated from our Controls.inputactions asset

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event UnityAction<bool> PrimaryFireEvent;
    public event UnityAction<Vector2> MoveEvent;

    public UnityEvent<bool> PrimaryFireEvent2;

    Controls _controls;

    // IPlayerActions methods
    #region IPlayerActions
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        MoveEvent?.Invoke(moveInput);
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            // PrimaryFire button pressed
            PrimaryFireEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            // PrimaryFire button released
            PrimaryFireEvent?.Invoke(false);
        }
    }

    #endregion // IPlayerActions

    void Awake()
    {
        Debug.Log("InputReader.Awake");
    }

    void Start()
    {
        Debug.Log("InputReader.Start");
    }

    void OnEnable()
    {
        EnableInputReaderControls();
    }

    void OnDestroy()
    {
        Debug.Log("InputReader.Start");
    }

    public void EnableInputReaderControls()
    {
        if(_controls == null)
        {
            Debug.Log("InputReader.EnableInputReaderControls - Creating _controls...");
            _controls = new Controls();
            _controls.Player.SetCallbacks(this); // "Player" refers to the "Player" Action Map in the Controls.inputactions asset
        }

        _controls.Player.Enable(); // Turn on the "Player" Action Map
    }
}
