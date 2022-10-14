using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFinisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            player.RestrictMovementWithFakeDeath();
            GameManager.Singleton.GameWon();
            UIController.Singleton.InitFinishDialog("You Won", "Play again");

        }
    }
}
