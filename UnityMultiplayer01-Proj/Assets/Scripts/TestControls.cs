using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControls : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;

    void Start()
    {
        _inputReader.MoveEvent += HandleMove;
        _inputReader.PrimaryFireEvent += HandlePrimaryFire;
        _inputReader.EnableInputReaderControls();
    }

    void Update()
    {
        //Debug.Log("TestControls.Update");
    }

    void OnDestroy()
    {
        _inputReader.MoveEvent -= HandleMove;
        _inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    void HandleMove(Vector2 movement)
    {
        Debug.Log("HandleMove - movement: " + movement);
    }

    void HandlePrimaryFire(bool pressed)
    {
        Debug.Log("HandlePrimaryFire - pressed: " + pressed);
    }
}
