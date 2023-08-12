using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] EnemyAI enemyAI;

    [SerializeField] Animator animator;
    [SerializeField] List<GameObject> avatars;
    [SerializeField] Player player;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<Transform> attackPositions;
    [SerializeField] EnemyBullet bulletPrefab;
    [SerializeField] List<ParticleSystem> fireFx;
    [SerializeField] List<AudioSource> fireSound;

    [SerializeField] int avatarNum, weaponNum;
    [SerializeField] float life, attackSpeed, attackRange, attackDamage;
    [SerializeField] bool attackable;

    public float Life { get { return life; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }
    public float AttackDamage { get { return attackDamage; } }
    public Vector3 EnemyBodyPosition { get { return (transform.position + Vector3.up); } }

    public void Initialize(Player _player, int _avatarNum, int _weaponNum, ZoneManager zoneManager)
    {
        player = _player;
        avatarNum = _avatarNum;
        weaponNum = _weaponNum;

        life = enemyData.Life;
        attackDamage = enemyData.AttackDamage;
        attackSpeed = enemyData.AttackSpeed;
        attackRange = enemyData.AttackRange;

        animator.SetInteger("Weapon", weaponNum);

        for (int i = 0; i < avatars.Count; i++)
        {
            avatars[i].SetActive(avatarNum == i);
        }

        for(int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(weaponNum == i);
        }

        enemyAI.Initialize(this, player, zoneManager);
    }

    public void Hit(float damage)
    {
        //Debug.Log($"{name} Hit {damage}");
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
        animator.SetTrigger("Shot");
        fireFx[weaponNum].Play();
        fireSound[weaponNum].Play();
        EnemyBullet bullet = Instantiate(bulletPrefab, attackPositions[weaponNum].position, Quaternion.identity);
        bullet.FireBullet((player.transform.position - attackPositions[weaponNum].position).normalized, AttackDamage);
    }

    public void Move()
    {
        animator.SetBool("Move", true);
    }

    public void Wait()
    {
        animator.SetBool("Move", false);
    }
}
