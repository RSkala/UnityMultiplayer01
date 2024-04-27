using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent<Coin>(out var coin))
        {
            // Call Collect regardless whether this is Client or Server, as the Client should hide the coin in its Collect method
            int collectedCoinValue = coin.Collect();

            // Exit if this is not the server
            if(!IsServer) { return; }

            // Add to coins total
            TotalCoins.Value += collectedCoinValue;
        }
    }
}
