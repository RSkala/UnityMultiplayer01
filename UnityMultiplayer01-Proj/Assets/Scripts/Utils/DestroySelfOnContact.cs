using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfOnContact : MonoBehaviour
{
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DestroySelfOnContact,OnTriggerEnter2D - this object " + name + " collided with " + other.name);
        
        // We dont care what we collide with, just destroy this object when it collides with anything
        Destroy(gameObject);
    }
}
