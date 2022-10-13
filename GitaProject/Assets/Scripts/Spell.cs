using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private Vector3 direction;
    private int damage;
    public void Shoot(Vector3 playerCenter, float speed, int damage)
    {
        this.damage = damage;
        direction = playerCenter - transform.position;
        direction.Normalize();
        direction *= speed;
    }

    public void DestroyOverTime(float time)
    {
        Destroy(this, time);
    }


    void Update()
    {
        transform.Translate(direction * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            var player = other.GetComponent<PlayerController>();
            player.GetDamaged(damage);
        }
    }
}
