using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private MageController mage;
    [SerializeField] private ArcherController archer;

    void Start()
    {
        mage.SetPlayer(player);
    }
}
