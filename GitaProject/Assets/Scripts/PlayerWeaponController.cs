using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void AttackStarted()
    {
        _collider.enabled = true;
    }

    private IEnumerator DelayAttackFinish()
    {
        yield return new WaitForSeconds(1f);
        AttackFinished();
    }

    public void AttackFinished()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var enemy = other.GetComponent<MageController>();
            enemy.Die();
        }
    }
}
