using UnityEngine;

[CreateAssetMenu (fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] float nowLife, maxLife, nowArmor, maxArmor;
    [SerializeField] int money;

    public float NowLife { get { return nowLife; } set { nowLife = value; } }
    public float MaxLife { get { return maxLife; } set { maxLife = value; } }
    public int Money { get {  return money; } set {  money = value; } }
    public float NowArmor { get { return nowArmor; } set {  nowArmor = value; } }
    public float MaxArmor { get { return maxArmor; } set { maxArmor = value; } }
}
