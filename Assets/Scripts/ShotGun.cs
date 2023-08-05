using System.Collections;
using UnityEngine;

public class ShotGun : Weapon
{
    protected override IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject fireSmoke = Instantiate(fireEffect, fireTransform.position, Quaternion.identity, fireTransform);
        Destroy(fireSmoke, 5f);

        RaycastHit cameraRayCastHit, gunRayCastHit;
        for (int i = -1; i <= 1; i += 2)
        {
            for(int j = -1;  j <= 1; j += 2)
            {
                if (Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward + (player.Cam.transform.up * i + player.Cam.transform.right * j) * 0.05f, out cameraRayCastHit, range))
                {
                    Vector3 shotDir = (cameraRayCastHit.point - transform.position).normalized;
                    if (Physics.Raycast(transform.position, shotDir, out gunRayCastHit, range))
                    {
                        IHitable hit = gunRayCastHit.collider.GetComponent<IHitable>();
                        if (hit != null && (1 << gunRayCastHit.collider.gameObject.layer) == targetLayerMask.value)
                        {
                            hit.Hit(damage * 0.2f);
                        }
                    }
                }
            }
        }

        if (Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward, out cameraRayCastHit, range))
        {
            Vector3 shotDir = (cameraRayCastHit.point - transform.position).normalized;
            if (Physics.Raycast(transform.position, shotDir, out gunRayCastHit, range))
            {
                IHitable hit = gunRayCastHit.collider.GetComponent<IHitable>();
                if (hit != null && (1 << gunRayCastHit.collider.gameObject.layer) == targetLayerMask.value)
                {
                    hit.Hit(damage * 0.2f);
                }
            }
        }
        fireSound.Play();
    }
}
