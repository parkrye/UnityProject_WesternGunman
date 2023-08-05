using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected PlayerController player;
    [SerializeField] protected GameObject fireEffect;
    [SerializeField] protected AudioSource fireSound;
    [SerializeField] protected Transform fireTransform;
    [SerializeField] protected LayerMask targetLayerMask;

    [SerializeField] protected float damage, speed, range;

    public float Attack()
    {
        StartCoroutine(AttackRoutine());
        return speed;
    }

    protected abstract IEnumerator AttackRoutine();

    public void WeaponOn()
    {
        gameObject.SetActive(true);
    }

    public void WeaponOff()
    {
        gameObject.SetActive(false);
    }
}