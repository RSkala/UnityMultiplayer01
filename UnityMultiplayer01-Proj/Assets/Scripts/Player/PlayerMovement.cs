using System.Collections;
using System.Collections.Generic;
using Unity.Netcode; // for NetworkBehaviour
using UnityEngine;

//public class PlayerMovement : MonoBehaviour
public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] InputReader _inputReader;
    [SerializeField] Transform _tankTreadsTransform; // Ensure this Transform has the ClientNetworkTransform Component on it
    [SerializeField] Rigidbody2D _rigidbody2D; // Use Rigidbody2D instead of NetworkRigidbody2D because our movement is Client authoritative

    [Header("Settings")]
    [SerializeField] float _movementSpeed; // 4.0f;
    [SerializeField] float _turningRate; // 30.0f

    Vector2 _previousMovementInput;

    // We use OnNetworkSpawn like Start() on a MonoBehaviour, except when this is called we know everything is set up on the network (e.g. check who the owner is, data, etc)
    public override void OnNetworkSpawn()
    {
        //base.OnNetworkSpawn();

        if(!IsOwner)
        {
            return;
        }

        _inputReader.MoveEvent += HandleMove;
    }

    // We use OnNetworkDespawn like OnDestroy() on a MonoBehaviour, except when this is called we know everything is still valid before the object is actually destroyed
    public override void OnNetworkDespawn()
    {
        //base.OnNetworkDespawn();

        if(!IsOwner)
        {
            return;
        }

        _inputReader.MoveEvent -= HandleMove;
    }

    void Update()
    {
        if(!IsOwner)
        {
            return;
        }

        // Rotate only when X movement input is changed (i.e. when the A/D or Left/Right Arrows are pressed)
        float zRotation = _previousMovementInput.x * _turningRate * -1.0f * Time.deltaTime;
        _tankTreadsTransform.Rotate(0.0f, 0.0f, zRotation);
    }

    void FixedUpdate()
    {
        if(!IsOwner)
        {
            return;
        }

        _rigidbody2D.velocity = (Vector2)_tankTreadsTransform.up * _previousMovementInput.y * _movementSpeed; // * Time.fixedDeltaTime - Dont need to multiply by time because we are using velocity
    }

    void HandleMove(Vector2 movementInput)
    {
        _previousMovementInput = movementInput;
    }
}
