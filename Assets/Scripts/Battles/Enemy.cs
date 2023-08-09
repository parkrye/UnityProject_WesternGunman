using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHitable
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] EnemyAI enemyAI;

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] List<GameObject> avatars;
    [SerializeField] PlayerDataManager player;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<Transform> attackPositions;

    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] int avatarNum, weaponNum;
    [SerializeField] float life, attackSpeed, attackRange, attackDamage;
    [SerializeField] bool attackable;

    public void Initialize(PlayerDataManager _player, int _avatarNum, int _weaponNum)
    {
        player = _player;
        avatarNum = _avatarNum;
        weaponNum = _weaponNum;

        life = enemyData.Life;
        attackDamage = enemyData.AttackDamage;
        attackSpeed = enemyData.AttackSpeed;
        attackRange = enemyData.AttackRange;

        for(int i = 0; i < avatars.Count; i++)
        {
            if (i == avatarNum)
                avatars[i].SetActive(true);
            else
                avatars[i].SetActive(false);
        }

        for(int i = 0; i < weapons.Count; i++)
        {
            if (i == weaponNum)
                weapons[i].SetActive(true);
            else
                weapons[i].SetActive(false);
        }

        StartCoroutine(URoutine());
        StartCoroutine(AttackRoutine());
    }

    IEnumerator URoutine()
    {
        while (life > 0)
        {
            if (player != null)
            {
                if (Vector3.SqrMagnitude(player.transform.position - agent.destination) > 10f)
                {
                    agent.SetDestination(player.transform.position);
                }
                AttackCheck();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void AttackCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(attackPositions[weaponNum].position, transform.forward, out hit, attackRange))
        {
            PlayerDataManager hitTarget = hit.collider.GetComponent<PlayerDataManager>();
            if (hitTarget)
            {
                attackable = true;
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        while(life > 0)
        {
            if (attackable)
            {
                player.Hit(attackDamage);
                attackable = false;
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void Hit(float damage)
    {
        life -= damage;
        if(life < 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        StopAllCoroutines();
    }
}
