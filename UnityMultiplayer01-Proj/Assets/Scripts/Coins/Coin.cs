using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer; // Hide on client side immediately, then disable on server

    protected int _coinValue;
    protected bool _alreadyCollected; // Ensure that only one person can collect a coin

    public abstract int Collect();

    // Set the value of the Coin instance
    public void SetValue(int value) => _coinValue = value;

    // Show or Hide the Coin in the scene
    protected void Show(bool show) => _spriteRenderer.enabled = show;

}
