using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFinisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().DetachCamera();
            var isGameEnded = GameManager.Singleton.FinishRound();
            if (isGameEnded)
            {
                UIController.Singleton.InitFinishDialog("You Lost", "Game over");
            }
            else
            {
                UIController.Singleton.InitFinishDialog("You Died, try again", "Retry");
            }
        }
    }
}
