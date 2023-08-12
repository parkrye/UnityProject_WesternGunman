using UnityEngine;

[CreateAssetMenu (fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] float life, attackSpeed, attackRange, attackDamage;
    public float Life { get { return life; } set {  life = value; } }
    public float AttackSpeed { get {  return attackSpeed; } set {  attackSpeed = value; } }
    public float AttackRange { get {  return attackRange; } set { attackRange = value; } }
    public float AttackDamage { get {  return attackDamage; } set { attackDamage = value; } }
}
