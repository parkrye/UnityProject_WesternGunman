using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyTypeA { Offensive, Deffensive, Neutral }
    public enum EnemyTypeB { Brave, Timid, Lunatic }

    [SerializeField] EnemyTypeA typeA;
    [SerializeField] EnemyTypeB typeB;
}
