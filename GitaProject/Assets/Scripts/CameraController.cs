using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;

    private Transform parent;


    void Start()
    {
        parent = transform.parent;
    }

    // private void Update()
    // {

    // }

    // private void Rotate()
    // {

    // }
}
