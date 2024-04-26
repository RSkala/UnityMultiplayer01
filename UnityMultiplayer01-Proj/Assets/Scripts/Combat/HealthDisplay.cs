using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] Health _health;
    [SerializeField] Image _healthBarImage;

    public override void OnNetworkSpawn()
    {
        if(!IsClient) { return; } // Only Clients care about the UI

        _health.CurrentHealth.OnValueChanged += HandleHealthChanged;

        // Force a HandleHealthChanged call in case we subscribe to the event after the health has already been set
        HandleHealthChanged(0, _health.CurrentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        if(!IsClient) { return; } // Only Clients care about the UI
        
        _health.CurrentHealth.OnValueChanged += HandleHealthChanged;
    }

    void HandleHealthChanged(int oldHealth, int newHealth)
    {
        _healthBarImage.fillAmount = (float)newHealth / (float)_health.MaxHealth;
    }
}
