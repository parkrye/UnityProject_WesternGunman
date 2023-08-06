using System.Collections;
using UnityEngine;

public class HandGun : Weapon
{
    protected override IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject fireSmoke = Instantiate(fireEffect, fireTransform.position, Quaternion.identity, fireTransform);
        Destroy(fireSmoke, 5f);
        RaycastHit cameraRayCastHit, gunRayCastHit;
        if(Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward, out cameraRayCastHit, weaponData.Range))
        {
            Vector3 shotDir = (cameraRayCastHit.point - transform.position).normalized;
            if (Physics.Raycast(transform.position, shotDir, out gunRayCastHit, weaponData.Range))
            {
                IHitable hit = gunRayCastHit.collider.GetComponent<IHitable>();
                if (hit != null && (1 << gunRayCastHit.collider.gameObject.layer) == targetLayerMask.value)
                {
                    hit.Hit(weaponData.Damage);
                }
            }
        }
        fireSound.Play();
    }
}
