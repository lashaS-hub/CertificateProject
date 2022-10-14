using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private MageController mage;
    [SerializeField] private MageController archer;

    void Start()
    {
        mage.SetPlayer(player);
        archer.SetPlayer(player);
    }
}
