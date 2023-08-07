using UnityEngine;

[CreateAssetMenu (fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] float damage, speed, range;
    [SerializeField] int damageUpgradeCount, speedUpgradeCount, rangeUpgradeCount;
    public float Damage { get { return damage; } set {  damage = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public float Range { get { return range; } set { range = value; } }
    public int DamageUpgradeCount { get { return damageUpgradeCount; } set { damageUpgradeCount = value; } }
    public int SpeedUpgradeCount { get { return speedUpgradeCount; } set { speedUpgradeCount = value;  } }
    public int RangeUpgradeCount { get { return rangeUpgradeCount; } set { rangeUpgradeCount = value; } }
}
