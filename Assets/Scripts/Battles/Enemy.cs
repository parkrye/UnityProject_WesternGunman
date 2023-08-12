using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] EnemyAI enemyAI;

    [SerializeField] Animator animator;
    [SerializeField] List<GameObject> avatars;
    [SerializeField] PlayerDataManager player;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<Transform> attackPositions;
    [SerializeField] EnemyBullet bulletPrefab;

    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] int avatarNum, weaponNum;
    [SerializeField] float life, attackSpeed, attackRange, attackDamage;
    [SerializeField] bool attackable;

    public float Life { get { return life; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }
    public float AttackDamage { get { return attackDamage; } }

    public void Initialize(PlayerDataManager _player, int _avatarNum, int _weaponNum, ZoneManager zoneManager)
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

        enemyAI.Initialize(this, player.transform, zoneManager);
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

    public void Shot()
    {
        EnemyBullet bullet = Instantiate(bulletPrefab, attackPositions[weaponNum].position, Quaternion.identity);
        bullet.FireBullet((player.transform.position - attackPositions[weaponNum].position).normalized, AttackDamage);
    }
}
