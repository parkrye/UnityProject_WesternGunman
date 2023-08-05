using System.Collections;
using UnityEngine;

public class HandGun : Weapon
{
    protected override IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject fireSmoke = Instantiate(fireEffect, fireTransform.position, Quaternion.identity, fireTransform);
        Destroy(fireSmoke, 5f);
        RaycastHit rayCastHit;
        if(Physics.Raycast(player.Cam.transform.position, player.Cam.transform.forward, out rayCastHit, range))
        {
            IHitable hit = rayCastHit.collider.GetComponent<IHitable>();
            if (hit != null && (1 << rayCastHit.collider.gameObject.layer) == targetLayerMask.value)
            {
                hit.Hit(damage);
            }
        }
        fireSound.Play();
    }
}
