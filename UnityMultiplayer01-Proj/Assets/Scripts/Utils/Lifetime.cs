using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] float _lifetime = 1.0f;

    void Start()
    {
        if(_lifetime <= 0.0f)
        {
            Debug.LogWarning(name+ " - Lifetime invalid. Object will be destroyed immediately. Check Inspector values");
        }

        Destroy(gameObject, _lifetime);
    }

    void OnDestroy()
    {
        //Debug.Log("Lifetime.OnDestroy - " + name);
    }
}
