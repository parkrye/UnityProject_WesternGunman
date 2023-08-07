using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] float nowLife, maxLife;
    [SerializeField] int money;

    public float NowLife { get { return nowLife; } set { nowLife = value; } }
    public float MaxLife { get { return maxLife; } set { maxLife = value; } }
    public int Money { get {  return money; } set {  money = value; } }
}
