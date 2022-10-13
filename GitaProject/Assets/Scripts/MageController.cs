using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private float attackRange;
    [SerializeField] private float spellSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int damage;

    private PlayerController player;
    private Animator _animator;

    private float prevShootTime = 0;


    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    void Update()
    {
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.GetPlayerCenter());
        if (distanceToPlayer < attackRange)
        {
            Attack();
        }
        else
        {
            Idle();
        }
    }

    private void Attack()
    {
        var curTime = Time.time;
        if (curTime - prevShootTime > attackSpeed)
        {
            var playerCenter = player.GetPlayerCenter();
            transform.LookAt(new Vector3(playerCenter.x, transform.position.y, playerCenter.z));
            prevShootTime = curTime;

            Shoot();
        }
    }

    private void Idle()
    {

    }

    private void Shoot()
    {
        _animator.SetTrigger("Attack");

        var spellObject = Instantiate(spellPrefab, handTransform.position, Quaternion.identity);
        var spell = spellObject.GetComponent<Spell>();

        spell.Shoot(player.GetPlayerCenter(), spellSpeed, damage);
        spell.DestroyOverTime(2);
    }
}
