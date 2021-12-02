using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnEnter : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ResetToStart();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ResetToStart();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ResetToStart();
        }
    }
}
