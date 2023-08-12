using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum AI_State { Watch, Shot, Hide }

    [SerializeField] Enemy enemy;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] ZoneManager zoneManager;
    [SerializeField] Player player;
    [SerializeField] LayerMask bulletCollideLayer;

    [SerializeField] AI_State aI_State;
    [SerializeField] int nowZone = -1;
    [SerializeField] bool readyToNextAction, isKeepingAction;
    [SerializeField] float timewait;
    [SerializeField] List<int> exceptNums;

    public void Initialize(Enemy _enemy, Player _player, ZoneManager _zoneManager)
    {
        enemy = _enemy;
        player = _player;
        zoneManager = _zoneManager;
        readyToNextAction = true;
        isKeepingAction = false;
        exceptNums = new List<int>();

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
                    ShotMove();
                    break;
                case AI_State.Hide:
                    HideMove();
                    break;
            }
            Debug.DrawRay(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition) * float.MaxValue);
            yield return null;
        }
    }

    void FindPlayer()
    {
        if((player.transform.position - transform.position).sqrMagnitude < enemy.AttackRange * enemy.AttackRange)
        {
            //Debug.Log($"{aI_State} : Find Player");
            timewait = 0f;
            readyToNextAction = true;
            aI_State = AI_State.Shot;
        }
        else
        {
            timewait += Time.deltaTime;
            if(timewait > 5f)
                readyToNextAction = true;
        }
    }

    void WatchAround()
    {
        //Debug.Log($"{aI_State} : Start Watch Around");
        if (aI_State != AI_State.Watch)
        {
            readyToNextAction = true;
            return;
        }
        if (!readyToNextAction)
            return;
        if (nowZone < 0)
            return;

        Zone moveZone = zoneManager.Zone[nowZone].PassibleZone[Random.Range(0, zoneManager.Zone[nowZone].PassibleZone.Count)];
        navMeshAgent.SetDestination(moveZone.transform.position);
        enemy.Move();
        //Debug.Log($"{aI_State} : Move to {moveZone}");
        StartCoroutine(CheckDestination());

        readyToNextAction = false;
    }

    IEnumerator CheckDestination()
    {
        //Debug.Log($"{aI_State} : Start Check Destination");
        float timeCheck = 0f;
        while(!readyToNextAction)
        {
            if (aI_State != AI_State.Watch)
                break;
            if(navMeshAgent.remainingDistance < 1f)
                break;
            if (timeCheck > 5f)
                break;
            timeCheck += Time.deltaTime;
            yield return null;
        }
        //Debug.Log($"{aI_State} : Move Over");
        readyToNextAction = true;
    }

    void HideMove()
    {
        if (aI_State != AI_State.Hide)
        {
            readyToNextAction = true;
            return;
        }
        if (readyToNextAction)
        {
            timewait = 0f;
            readyToNextAction = false;
            int moveZoneNum = zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistanceBlockedZone(transform, exceptNums);
            navMeshAgent.SetDestination(zoneManager.Zone[moveZoneNum].transform.position);
            exceptNums.Add(moveZoneNum);
            enemy.Move();
            //Debug.Log($"{aI_State} : Move to {zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistanceBlockedZone(transform).position}");
        }
        else
        {
            //Debug.DrawRay(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition) * float.MaxValue);
            if (Physics.Raycast(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition), out RaycastHit hit, float.MaxValue, bulletCollideLayer.value))
            {
                if (hit.collider.tag.Equals("Player"))
                {
                    timewait += Time.deltaTime;
                    if (timewait > 1f)
                    {
                        readyToNextAction = true;
                    }
                    return;
                }
            }
            if (!isKeepingAction)
            {
                //Debug.Log($"{aI_State} : Avoid Player");
                isKeepingAction = true;
                enemy.Wait();
                StartCoroutine(HideRoutine());
            }
            return;
        }
    }

    IEnumerator HideRoutine()
    {
        //Debug.Log($"{aI_State} : Start Hide Move");
        float timeCheck = Random.Range(1f, 3f);
        exceptNums = new();
        while (timeCheck > 0f)
        {
            if (aI_State != AI_State.Hide)
                break;
            timeCheck -= Time.deltaTime;
            yield return null;
        }
        //Debug.Log($"{aI_State} : End Hide Move");
        isKeepingAction = false;
        readyToNextAction = true;
        aI_State = AI_State.Shot;
    }

    void ShotMove()
    {
        //Debug.Log($"{aI_State} Start Shot");
        if (aI_State != AI_State.Shot)
        {
            readyToNextAction = true;
            return;
        }

        transform.LookAt(player.PlayerBodyPosotion);
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);

        if (readyToNextAction)
        {
            //Debug.Log($"{aI_State} Ready to Shot");
            timewait = 0f;
            readyToNextAction = false;
            int moveZoneNum = zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistancePassibleZone(transform, exceptNums);
            navMeshAgent.SetDestination(zoneManager.Zone[moveZoneNum].transform.position);
            exceptNums.Add(moveZoneNum);
            enemy.Move();
        }
        else
        {
            //Debug.DrawRay(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition) * float.MaxValue);
            if (Physics.Raycast(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition), out RaycastHit hit, float.MaxValue, bulletCollideLayer.value))
            {
                if (hit.collider.tag.Equals("Player"))
                {
                    if (!isKeepingAction)
                    {
                        //Debug.Log($"{aI_State} : Meet Player");
                        isKeepingAction = true;
                        enemy.Wait();
                        StartCoroutine(ShotRoutine());
                    }
                    return;
                }
            }
            timewait += Time.deltaTime;
            if (timewait > 1f)
            {
                //Debug.Log($"{aI_State} Time Over");
                readyToNextAction = true;
            }
            return;
        }
    }

    IEnumerator ShotRoutine()
    {
        //Debug.Log($"{aI_State} : Start Shot");
        int shotCount = Random.Range(1, 4);
        exceptNums = new();
        while (shotCount > 0)
        {
            if (aI_State != AI_State.Shot)
                break;
            //Debug.Log($"{aI_State} : Shot {shotCount}");
            enemy.Shot();
            shotCount--;
            yield return new WaitForSeconds(enemy.AttackSpeed);
        }
        isKeepingAction = false;
        readyToNextAction = true;
        //Debug.Log($"{aI_State} : End Shot ");
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
