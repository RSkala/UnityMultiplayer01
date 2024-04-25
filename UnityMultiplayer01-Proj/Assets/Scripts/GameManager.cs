using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using static Controls;

public class GameManager : MonoBehaviour
{
    [Header("____________________________________")]
    [Header("Input")]
    [SerializeField] InputReader _inputReader;

    [Header("____________________________________")]
    [Header("Input Testing")]
    [SerializeField] InputActionAsset _inputActionAsset; // Just for testing
    [SerializeField] PlayerInput _playerInput; // Just for testing

    // TEST
    //Controls.PlayerActions _playerActions;

    void Start()
    {
        InitializeUserInput();
    }

    void InitializeUserInput()
    {
        // If Reload Domain is disabled in Editor Play Mode settings, this needs to be called, otherwise controls will not register.
        _inputReader.EnableInputReaderControls();
    }
}
