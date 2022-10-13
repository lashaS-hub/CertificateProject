using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityRotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed);
    }
}
