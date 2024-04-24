using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionButtons : MonoBehaviour
{
    public void StartHost()
    {
        Debug.Log("ConnectionButtons.StartHost");
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        Debug.Log("ConnectionButtons.StartClient");

        NetworkManager.Singleton.StartClient();
        //NetworkManager.Singleton.StartServer();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
