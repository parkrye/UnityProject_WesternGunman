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
        Vector3 shotDir;
        //Debug.Log($"{name} Start Camera Check");
        if(Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward, out cameraRayCastHit, weaponData.Range))
        {
            //Debug.Log($"{name} Camera Hit {cameraRayCastHit.collider.name}");
            shotDir = (cameraRayCastHit.point - transform.position).normalized;
        }
        else
        {
            //Debug.Log($"{name} Camera Out of Range");
            shotDir = transform.forward;
        }

        if (Physics.Raycast(transform.position, shotDir, out gunRayCastHit, weaponData.Range))
        {
            //Debug.Log($"{name} Gun Hit {gunRayCastHit.collider.name}");
            IHitable hit = gunRayCastHit.collider.GetComponent<IHitable>();
            if (hit != null && (1 << gunRayCastHit.collider.gameObject.layer) == targetLayerMask.value)
            {
                //Debug.Log($"{name} Target Hit");
                hit.Hit(weaponData.Damage);
            }
            else
            {
                //Debug.Log($"{name} Not Target");
            }
        }
        else
        {
            //Debug.Log($"{name} Out of Range");
        }
        fireSound.Play();
    }
}
