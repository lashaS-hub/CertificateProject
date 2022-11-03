using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Singleton;
    

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
    }

}
