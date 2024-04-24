using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JoinServer : MonoBehaviour
{
    public void ClientJoinServer()
    {
        Debug.Log("JoinServer.ClientJoinServer");
        
        NetworkManager.Singleton.StartClient();
        //NetworkManager.Singleton.StartServer();
        //NetworkManager.Singleton.StartHost();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
