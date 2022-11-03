using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFinisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            player.DetachCamera();
            player.RestrictMovementWithFakeDeath();
    {
    }
        }
    }
}
