using System.Collections;
using UnityEngine;

public class ShotGun : Weapon
{
    protected override IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject fireSmoke = Instantiate(fireEffect, fireTransform.position, Quaternion.identity, fireTransform);
        Destroy(fireSmoke, 5f);

        RaycastHit rayCastHit;
        for (int i = -1; i <= 1; i += 2)
        {
            for(int j = -1;  j <= 1; j += 2)
            {
                if (Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward + (player.Cam.transform.up * i + player.Cam.transform.right * j) * 0.05f, out rayCastHit, range))
                {
                    IHitable hit = rayCastHit.collider.GetComponent<IHitable>();
                    if (hit != null && (1 << rayCastHit.collider.gameObject.layer) == targetLayerMask.value)
                    {
                        hit.Hit(damage * 0.2f);
                    }
                }
            }
        }

        if (Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward, out rayCastHit, range))
        {
            IHitable hit = rayCastHit.collider.GetComponent<IHitable>();
            if (hit != null && (1 << rayCastHit.collider.gameObject.layer) == targetLayerMask.value)
            {
                hit.Hit(damage * 0.2f);
            }
        }
        fireSound.Play();
    }
}
