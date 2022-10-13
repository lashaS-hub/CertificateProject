using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            player.ScoreUp();

            Destroy(this.gameObject);
        }
    }
}
