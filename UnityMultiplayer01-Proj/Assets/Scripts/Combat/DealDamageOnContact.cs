using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// This Component is placed on a Server-side Prefab, so all of this logic is performed on the Server
public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] int _damage = 5;

    ulong _ownerClientId;

    void Start()
    {
        
    }

    public void SetOwner(ulong ownerClientId)
    {
        _ownerClientId = ownerClientId;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.attachedRigidbody == null) { return; }

        if(collider.attachedRigidbody.TryGetComponent<NetworkObject>(out var networkObject))
        {
            if(_ownerClientId == networkObject.OwnerClientId)
            {
                Debug.Log("Projectile hit owner. Skipping this collision.");
                return;
            }
        }

        if(collider.attachedRigidbody.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(_damage);
        }
    }
}
