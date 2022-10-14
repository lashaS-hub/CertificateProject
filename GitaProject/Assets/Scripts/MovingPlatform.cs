using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private int cubeInt = 1;
    private float speed = .03f;
    private float maxRange = 2f;

    void FixedUpdate()
    {
        if (transform.position.x > maxRange) cubeInt *= -1;
        else if (transform.position.x < -maxRange) cubeInt *= -1;
        transform.Translate(Vector3.right * cubeInt * speed);
    }
}
