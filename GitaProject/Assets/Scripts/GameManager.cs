using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    private int livesLeft;

    public int LivesLeft { get { return livesLeft; } }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        livesLeft = PlayerPrefs.GetInt("LivesLeft");
        if (livesLeft == 0) livesLeft = 3;
    }

    public bool FinishRound()
    {
        Cursor.lockState = CursorLockMode.Confined;
        livesLeft--;
        PlayerPrefs.SetInt("LivesLeft", livesLeft);
        if (livesLeft == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GameWon()
    {
        PlayerPrefs.SetInt("LivesLeft", 3);
        Cursor.lockState = CursorLockMode.Confined;
    }
}
