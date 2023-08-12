using UnityEngine;

public class QuestArea : MonoBehaviour
{
    [SerializeField] Transform playerStartTransform, enemySpawnTransform;
    [SerializeField] ZoneManager zoneManager;
    public Transform PlayerStartTransform { get { return playerStartTransform; } }
    public Transform EnemySpawnTransform { get { return enemySpawnTransform; } }
    public ZoneManager ZoneManager { get { return zoneManager; } }
}
