using UnityEngine;

[CreateAssetMenu (fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] float damage, speed, range;
    public float Damage { get { return damage; } set {  damage = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public float Range { get { return range; } set { range = value; } }
}
