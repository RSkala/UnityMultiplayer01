using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    // Force a Transform with this Component as Client Authoritative
    protected override bool OnIsServerAuthoritative()
    {
        return false;
        //return base.OnIsServerAuthoritative();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("ClientNetworkTransform.OnNetworkSpawn - " + name);
        base.OnNetworkSpawn();

        // If this object is the owner (i.e. locally controlled), then we can commit to updating its transform (position, rotation, and scale changes) 
        CanCommitToTransform = IsOwner; // "IsOwner" as opposed to "IsOwnedByServer"
    }

    public override void OnNetworkDespawn()
    {
        Debug.Log("ClientNetworkTransform.ClientNetworkTransform - " + name);
        base.OnNetworkDespawn();
    }

    protected override void Update()
    {
        CanCommitToTransform = IsOwner; // Unity does this every frame, so we will do it as well. 
        base.Update();

        if(NetworkManager != null) // dont need to use NetworkManager.Singleton as as NetworkBehaviour has a reference
        {
            // If we are connected as a Client and Listening
            if(NetworkManager.IsConnectedClient || NetworkManager.IsListening)
            {
                if(CanCommitToTransform)
                {
                    // We are allowed to update our own transform, so update the server with this transform
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
    }
}
