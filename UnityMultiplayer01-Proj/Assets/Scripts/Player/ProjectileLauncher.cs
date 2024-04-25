using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] InputReader _inputReader;
    [SerializeField] Transform _projectileSpawnPoint;
    [SerializeField] GameObject _serverProjectilePrefab;
    [SerializeField] GameObject _clientProjectilePrefab;
    [SerializeField] GameObject _muzzleFlash;
    [SerializeField] Collider2D _playerCollider;

    [Header("Settings")]
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _fireRate;
    [SerializeField] float _muzzleFlashDuration;

    bool _shouldFire;
    float _previousFireTime;
    float _muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        //base.OnNetworkSpawn();
        if(!IsOwner) { return; }

        _inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        //base.OnNetworkDespawn();
        if(!IsOwner) { return; }
    }

    void Update()
    {
        if(_muzzleFlashTimer > 0.0f)
        {
            _muzzleFlashTimer -= Time.deltaTime;
            if(_muzzleFlashTimer <= 0.0f)
            {
                _muzzleFlash.SetActive(false);
            }
        }

        if(!IsOwner) { return; }

        if(!_shouldFire) { return; }

        PrimaryFireServerRpc(_projectileSpawnPoint.position, _projectileSpawnPoint.up);
        SpawnDummyProjectile(_projectileSpawnPoint.position, _projectileSpawnPoint.up);
    }

    void HandlePrimaryFire(bool shouldFire)
    {
        _shouldFire = shouldFire;
    }

    void SpawnDummyProjectile(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        // Enable the Muzzle Flash
        _muzzleFlash.SetActive(true);
        _muzzleFlashTimer = _muzzleFlashDuration;

        // Create the dummy projectile (the "true" projectile will be fired from the server)
        GameObject projectileInstance = GameObject.Instantiate(_clientProjectilePrefab, spawnPosition, Quaternion.identity);
        projectileInstance.transform.up = spawnDirection;

        // Ignore collision between the player collider and the newly created projectile
        Physics2D.IgnoreCollision(_playerCollider, projectileInstance.GetComponent<Collider2D>());

        // Set the velocity of the projectile
        if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            rigidbody2D.velocity = rigidbody2D.transform.up * _projectileSpeed;
        }
    }

    // In order to create an RPC, the method must have the [ServerRpc] attribute and the method name must end with "ServerRpc"
    // NOTE: 
    // * The ClientRpc and ServerRpc attributes have been deprecated.
    // * Use the Rpc attribute instead moving forward.
    // https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/
    // https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/rpc/
    [ServerRpc]
    void PrimaryFireServerRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        GameObject projectileInstance = GameObject.Instantiate(_serverProjectilePrefab, spawnPosition, Quaternion.identity);
        projectileInstance.transform.up = spawnDirection;

        // Ignore collision between the player collider and the newly created projectile
        Physics2D.IgnoreCollision(_playerCollider, projectileInstance.GetComponent<Collider2D>());

        // Set the velocity of the projectile
        if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            rigidbody2D.velocity = rigidbody2D.transform.up * _projectileSpeed;
        }

        SpawnDummyProjectileClientRpc(spawnPosition, spawnPosition);
    }

    [ClientRpc]
    void SpawnDummyProjectileClientRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        if(IsOwner) { return; } // Do not fire a projectile on the client that is calling this

        SpawnDummyProjectile(spawnPosition, spawnDirection);
    }
}
