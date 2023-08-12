using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum AI_State { Watch, Shot, Hide }

    [SerializeField] Enemy enemy;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] ZoneManager zoneManager;
    [SerializeField] Transform playerTransform;

    [SerializeField] AI_State aI_State;
    [SerializeField] int nowZone = -1;
    [SerializeField] bool readyToNextAction, isKeepingAction;

    public void Initialize(Enemy _enemy, Transform _playerTransform, ZoneManager _zoneManager)
    {
        enemy = _enemy;
        playerTransform = _playerTransform;
        zoneManager = _zoneManager;
        readyToNextAction = true;
        isKeepingAction = false;

        StartCoroutine(ActionRoutine());
    }

    IEnumerator ActionRoutine()
    {
        while (true)
        {
            switch (aI_State)
            {
                case AI_State.Watch:
                    FindPlayer();
                    WatchAround();
                    break;
                case AI_State.Shot:
                    HideMove();
                    break;
                case AI_State.Hide:
                    ShotMove();
                    break;
            }
            yield return null;
        }
    }

    void FindPlayer()
    {
        if((playerTransform.position - transform.position).sqrMagnitude < enemy.AttackRange * enemy.AttackRange)
        {
            aI_State = AI_State.Shot;
            readyToNextAction = true;
        }
    }

    void WatchAround()
    {
        if (!readyToNextAction)
            return;
        if (nowZone < 0)
            return;

        Zone moveZone = zoneManager.Zone[nowZone].PassibleZone[Random.Range(0, zoneManager.Zone[nowZone].PassibleZone.Count)];
        navMeshAgent.SetDestination(moveZone.transform.position);
        StartCoroutine(CheckDestination());

        readyToNextAction = false;
    }

    IEnumerator CheckDestination()
    {
        float timeCheck = 0f;
        while(!readyToNextAction)
        {
            if(navMeshAgent.remainingDistance < 1f)
                break;
            if (timeCheck > 30f)
                break;
            timeCheck += Time.deltaTime;
            yield return null;
        }
        readyToNextAction = true;
    }

    void HideMove()
    {
        if (readyToNextAction)
        {
            readyToNextAction = false;
            navMeshAgent.SetDestination(zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistanceBlockedZone(transform).position);
        }
        else
        {
            if(Physics.Raycast(transform.position, (playerTransform.position - transform.position), out RaycastHit hit))
            {
                if (hit.collider.tag.Equals("Player"))
                {
                    return;
                }
            }
            if (!isKeepingAction)
            {
                isKeepingAction = true;
                StartCoroutine(HideRoutine());
            }
            return;
        }
    }

    IEnumerator HideRoutine()
    {
        float timeCheck = Random.Range(1f, 5f);
        while(timeCheck > 0f)
        {
            timeCheck -= Time.deltaTime;
            yield return null;
        }
        isKeepingAction = false;
        aI_State = AI_State.Shot;
    }

    void ShotMove()
    {
        if (readyToNextAction)
        {
            readyToNextAction = false;
            navMeshAgent.SetDestination(zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistancePassibleZone(transform).position);
        }
        else
        {
            if (Physics.Raycast(transform.position, (playerTransform.position - transform.position), out RaycastHit hit))
            {
                if (hit.collider.tag.Equals("Player"))
                {
                    if (!isKeepingAction)
                    {
                        isKeepingAction = true;
                        StartCoroutine(ShotRoutine());
                        return;
                    }
                }
            }
            return;
        }
    }

    IEnumerator ShotRoutine()
    {
        int shotCount = Random.Range(1, 4);
        while (shotCount > 0)
        {
            transform.LookAt(playerTransform.position);
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
            enemy.Shot();
            shotCount--;
            yield return new WaitForSeconds(enemy.AttackSpeed);
        }
        isKeepingAction = false;
        aI_State = AI_State.Hide;
    }

    void OnTriggerEnter(Collider other)
    {
        Zone zone = other.GetComponent<Zone>();
        if (zone)
        {
            nowZone = zone.ZoneNumber;
        }
    }
}
