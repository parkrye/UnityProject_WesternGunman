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
            transform.Translate(dir * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerDataManager player = collision.gameObject.GetComponent<PlayerDataManager>();
        if (player)
        {
            player.Hit(damage);
        }
        Destroy(gameObject);
    }
}
