using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

//public class Health : MonoBehaviour
public class Health : NetworkBehaviour
{
    [field:SerializeField] public int MaxHealth { get; private set; } = 100;

    // A "NetworkVariable" can only be changed on the server
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();

    // NOTE: If we ever need a Network Serializable object, use INetworkSerializable
    // https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/serialization/inetworkserializable/index.html

    bool _isDead;

    public UnityAction<Health> OnDie;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        // Only set health on the server
        if(!IsServer) { return; }

        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }

    void ModifyHealth(int value)
    {
        if(_isDead) { return;}
        //if(!IsServer) { return; }

        int newHealth = CurrentHealth.Value + value;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);

        if(CurrentHealth.Value <= 0)
        {
            OnDie?.Invoke(this);
            _isDead = true;
        }
    }
}
