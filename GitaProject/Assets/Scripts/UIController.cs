using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Singleton;
    [SerializeField] private GameObject finishDialog;
    [SerializeField] private TMP_Text finishText;
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text restartButtonText;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        restartButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
    }

    public void InitFinishDialog(string finishText, string restartButtonText)
    {
        finishDialog.SetActive(true);
        this.finishText.text = finishText;
        this.restartButtonText.text = restartButtonText;
    }
}
