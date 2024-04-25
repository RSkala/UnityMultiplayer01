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

    [Header("Settings")]
    [SerializeField] float _projectileSpeed;

    bool _shouldFire;

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
        GameObject projectileInstance = GameObject.Instantiate(_clientProjectilePrefab, spawnPosition, Quaternion.identity);
        projectileInstance.transform.up = spawnDirection;
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
        SpawnDummyProjectileClientRpc(spawnPosition, spawnPosition);
    }

    [ClientRpc]
    void SpawnDummyProjectileClientRpc(Vector3 spawnPosition, Vector3 spawnDirection)
    {
        if(IsOwner) { return; } // Do not fire a projectile on the client that is calling this

        SpawnDummyProjectile(spawnPosition, spawnDirection);
    }
}
