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
        Vector3 shotDir;
        for (int i = -1; i <= 1; i += 2)
        {
            for(int j = -1;  j <= 1; j += 2)
            {
                if (Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward + (player.Cam.transform.up * i + player.Cam.transform.right * j) * 0.05f, out cameraRayCastHit, weaponData.Range))
                {
                    shotDir = (cameraRayCastHit.point - transform.position).normalized;
                }
                else
                {
                    shotDir = transform.forward + (player.Cam.transform.up * i + player.Cam.transform.right * j) * 0.05f;
                }

                if (Physics.Raycast(transform.position, shotDir, out gunRayCastHit, weaponData.Range))
                {
                    IHitable hit = gunRayCastHit.collider.GetComponent<IHitable>();
                    if (hit != null && (1 << gunRayCastHit.collider.gameObject.layer) == targetLayerMask.value)
                    {
                        hit.Hit(weaponData.Damage * 0.2f);
                    }
                }
            }
        }

        if (Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward, out cameraRayCastHit, weaponData.Range))
        {
            shotDir = (cameraRayCastHit.point - transform.position).normalized;
        }
        else
        {
            shotDir = transform.forward;
        }

        if (Physics.Raycast(transform.position, shotDir, out gunRayCastHit, weaponData.Range))
        {
            IHitable hit = gunRayCastHit.collider.GetComponent<IHitable>();
            if (hit != null && (1 << gunRayCastHit.collider.gameObject.layer) == targetLayerMask.value)
            {
                hit.Hit(weaponData.Damage * 0.2f);
            }
        }
        fireSound.Play();
    }
}
