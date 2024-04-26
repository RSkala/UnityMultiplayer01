using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public override int Collect()
    {
        if(!IsServer)
        {
            // This is a client, show just hide the coin, and don't return any value
            Show(false);
            return 0;
        }

        // If Sever (or Host, which also is a Server), return no value if this coin was already collected
        if(_alreadyCollected)
        {
            return 0;
        }

        _alreadyCollected = true;

        // This is the server/host, so return the value of the coin
        return _coinValue;
    }
}
