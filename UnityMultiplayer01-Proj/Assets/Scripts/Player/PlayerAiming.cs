using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//public class PlayerAiming : MonoBehaviour
public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] InputReader _inputReader;
    [SerializeField] Transform _turretTransform;

    Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        _mainCamera = Camera.main;
    }

    // Use LateUpdate so it happens after the player tank movement
    void LateUpdate()
    {
        if(!IsOwner)
        {
            return;
        }

        // Get the player's current aim position
        Vector2 aimInput = _inputReader.AimPosition;

        // Get aim position in world coordinates
        Vector2 aimPosWorld = _mainCamera.ScreenToWorldPoint(aimInput);

        // Get the vector from the turret position to the aim position
        Vector2 turretToMouseDir = (aimPosWorld - (Vector2)_turretTransform.position).normalized;

        // Get the angle from the world up to the turret direction vector
        float angle = Vector2.SignedAngle(Vector2.up, turretToMouseDir);

        // Can also use Mathf.Atan2 * Mathf.RadToDeg and subtract 90 degrees for the Angle (since arctan calculates position (counter-clockwise) from the X)
        //angle = Mathf.Atan2(turretToMouseDir.y, turretToMouseDir.x) * Mathf.Rad2Deg - 90.0f; // SUBTRACT 90 degrees because positive rotations are counter-clockwise

        // Create the rotation for the turrent
        Quaternion turretRotation = Quaternion.Euler(0.0f, 0.0f, angle);

        // Set the new rotation
        _turretTransform.rotation = turretRotation;

        // Alternate method instead of calculating the rotation. I'll assume this rebuilds the transform's orthonormal basis.
        // _turretTransform.up = new Vector2
        // (
        //     aimPosWorld.x - _turretTransform.position.x,
        //     aimPosWorld.y - _turretTransform.position.y
        // );
    }
}
