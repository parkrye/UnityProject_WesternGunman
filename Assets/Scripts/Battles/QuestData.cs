using UnityEngine;

[CreateAssetMenu (fileName = "questData", menuName = "Data/questData")]
public class QuestData : ScriptableObject
{
    [SerializeField] int enemyCount, pay, areaNum;
    [SerializeField] GameData.QuestState questState;

    public int EnemyCount { get {  return enemyCount; } set {  enemyCount = value; } }
    public int QuestPay { get {  return pay; } set { pay = value; } }
    public int AreaNum { get { return areaNum; } set { areaNum = value; } }
    public GameData.QuestState QuestState { get {  return questState; } set { questState = value; } }
}
