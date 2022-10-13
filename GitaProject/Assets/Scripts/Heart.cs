using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            player.HealUp();

            Destroy(this.gameObject);
        }
    }
}
