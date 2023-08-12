using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float damage;

    public void FireBullet(Vector3 dir, float _damage)
    {
        damage = _damage;
        StartCoroutine(ShotRoutine(dir));
    }

    IEnumerator ShotRoutine(Vector3 dir)
    {
        while(true)
        {
            transform.Translate(dir);
            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerDataManager player = other.GetComponent<PlayerDataManager>();
        if (player)
        {
            player.Hit(damage);
            Destroy(gameObject);
        }
        else
        {
            if((1 << other.gameObject.layer) == LayerMask.GetMask("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}
