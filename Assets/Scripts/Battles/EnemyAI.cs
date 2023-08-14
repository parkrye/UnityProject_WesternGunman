using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum AI_State { Watch, Shot, Hide }

    [SerializeField] Enemy enemy;
    [SerializeField] QuestData questData;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] ZoneManager zoneManager;
    [SerializeField] Player player;
    [SerializeField] LayerMask bulletCollideLayer;

    [SerializeField] AI_State aI_State;
    [SerializeField][Range(0f, 1f)] float brave, timid;

    [SerializeField] int nowZone = -1;
    [SerializeField] bool readyToNextAction, isKeepingAction, startActionRoutine, findShootable;
    [SerializeField] float timewait, moveSpeed;
    [SerializeField] List<int> exceptNums;

    IEnumerator hideRoutine; 

    public void Initialize(Enemy _enemy, Player _player, ZoneManager _zoneManager, float _brave = -1f, float _timid = -1f)
    {
        enemy = _enemy;
        player = _player;
        zoneManager = _zoneManager;
        readyToNextAction = true;
        isKeepingAction = false;
        startActionRoutine = false;
        findShootable = false;
        exceptNums = new List<int>();

        moveSpeed = 2f;

        brave = _brave;
        timid = _timid;
        if(brave < 0f)
            brave = Random.Range(0f, 1f);
        if(timid < 0f)
            timid = Random.Range(0f, 1f);

        hideRoutine = HideRoutine();

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
            //Debug.DrawRay(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition) * float.MaxValue);
            yield return new WaitForSeconds(0.1f);
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
            timewait += 0.1f;
            if(timewait > 10f)
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

        readyToNextAction = false;
        Zone moveZone = zoneManager.Zone[nowZone].PassibleZone[Random.Range(0, zoneManager.Zone[nowZone].PassibleZone.Count)];
        navMeshAgent.SetDestination(moveZone.transform.position);
        //Debug.Log($"{aI_State} : Move to {moveZone}");
        StartCoroutine(CheckDestination());
    }

    IEnumerator CheckDestination()
    {
        //Debug.Log($"{aI_State} : Start Check Destination");
        float timeCheck = 0f;
        enemy.Move();
        navMeshAgent.speed = moveSpeed;
        while (!readyToNextAction)
        {
            if (aI_State != AI_State.Watch)
                break;
            if(navMeshAgent.remainingDistance < 1f)
                break;
            if (timeCheck > 10f)
                break;
            timeCheck += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        //Debug.Log($"{aI_State} : Move Over");
        enemy.Wait();
        navMeshAgent.speed = 0f;
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
            readyToNextAction = false;
            timewait = 0f;
            int moveZoneNum = zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistanceBlockedZone(transform, exceptNums);
            navMeshAgent.SetDestination(zoneManager.Zone[moveZoneNum].transform.position);
            if (!exceptNums.Contains(moveZoneNum))
                exceptNums.Add(moveZoneNum);
            enemy.Move();
            navMeshAgent.speed = moveSpeed;
            //Debug.Log($"{aI_State} : Move to {zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistanceBlockedZone(transform).position}");
        }
        else
        {
            if (!isKeepingAction)
            {
                //Debug.Log($"{aI_State} : Avoid Player");
                isKeepingAction = true;
            }
            else
            {
                //Debug.DrawRay(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition) * float.MaxValue);
                if (Physics.Raycast(enemy.EnemyBodyPosition, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition), out RaycastHit hit, float.MaxValue, bulletCollideLayer.value))
                {
                    if (hit.collider.tag.Equals("Player"))
                    {
                        if (navMeshAgent.remainingDistance < 1f)
                        {
                            timewait += 0.1f;
                            if (timewait > 2f)
                            {
                                readyToNextAction = true;
                                isKeepingAction = false;
                                StopCoroutine(hideRoutine);
                                if (ShotOrHideDecision())
                                    aI_State = AI_State.Shot;
                            }
                        }
                        return;
                    }
                }

                if (!startActionRoutine)
                {
                    startActionRoutine = true;
                    StartCoroutine(hideRoutine);
                }
            }
            return;
        }
    }

    IEnumerator HideRoutine()
    {
        //Debug.Log($"{aI_State} : Start Hide Move");
        float timeCheck = Random.Range(1f, 3f);
        exceptNums = new();
        enemy.Hide(true);
        navMeshAgent.speed = 0f;
        while (timeCheck > 0f)
        {
            if (aI_State != AI_State.Hide)
                break;

            timeCheck -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        //Debug.Log($"{aI_State} : End Hide Move");
        isKeepingAction = false;
        readyToNextAction = true;
        startActionRoutine = false;
        aI_State = AI_State.Shot;
        enemy.Hide(false);
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
            readyToNextAction = false;
            timewait = 0f;
            int moveZoneNum = zoneManager.Zone[zoneManager.PlayerPositionedZone].GetLeastDistancePassibleZone(transform, exceptNums);
            //Debug.Log($"{aI_State} Move to {moveZoneNum}");
            navMeshAgent.SetDestination(zoneManager.Zone[moveZoneNum].transform.position);
            if(!exceptNums.Contains(moveZoneNum))
                exceptNums.Add(moveZoneNum);
            enemy.Move();
            navMeshAgent.speed = moveSpeed;
        }
        else
        {
            //Debug.DrawRay(transform.position, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition) * float.MaxValue);
            if (Physics.Raycast(enemy.EnemyBodyPosition, (player.PlayerBodyPosotion - enemy.EnemyBodyPosition), out RaycastHit hit, float.MaxValue, bulletCollideLayer.value))
            {
                //Debug.Log($"{aI_State} : {hit.collider.name}");
                if (hit.collider.tag.Equals("Player"))
                {
                    if (!isKeepingAction)
                    {
                        //Debug.Log($"{aI_State} : Meet Player");
                        isKeepingAction = true;
                        timewait = 0f;
                        enemy.Wait();
                        navMeshAgent.speed = 0f;
                        StartCoroutine(ShotRoutine());
                        findShootable = true;
                    }
                    return;
                }
                findShootable = false;
            }

            if (!findShootable && !isKeepingAction && navMeshAgent.remainingDistance < 1f)
            {
                timewait += 0.1f;
                if (timewait > 5f)
                {
                    //Debug.Log($"{aI_State} Time Over");
                    readyToNextAction = true;
                }
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
        //Debug.Log($"{aI_State} : End Shot ");
        if (!ShotOrHideDecision())
            aI_State = AI_State.Hide;
        isKeepingAction = false;
        readyToNextAction = true;
        findShootable = false;
    }

    bool ShotOrHideDecision()
    {
        float shotPoint = 0f, hidePoint = 0f;

        if (questData.EnemyKilled <= questData.EnemyCount * 0.5f)
            hidePoint += questData.EnemyKilled;
        else
            shotPoint += questData.EnemyCount - questData.EnemyKilled;

        if(player.PlayerDataManager.PlayerData.NowLife <= player.PlayerDataManager.PlayerData.MaxLife * 0.5f)
            shotPoint += (player.PlayerDataManager.PlayerData.MaxLife - player.PlayerDataManager.PlayerData.NowLife) * 0.1f;
        else
            hidePoint += player.PlayerDataManager.PlayerData.NowLife * 0.1f;

        if(player.PlayerDataManager.PlayerData.NowArmor <= player.PlayerDataManager.PlayerData.MaxArmor * 0.5f)
            shotPoint += (player.PlayerDataManager.PlayerData.MaxArmor - player.PlayerDataManager.PlayerData.NowArmor) * 0.1f;
        else
            hidePoint += (player.PlayerDataManager.PlayerData.NowArmor * 0.1f);

        shotPoint += player.PlayerDataManager.PlayerData.Money > 100 ? 10 : player.PlayerDataManager.PlayerData.Money * 0.1f;

        shotPoint *= brave;
        hidePoint *= timid;

        if (aI_State == AI_State.Shot)
            hidePoint *= 1.5f;
        else
            shotPoint *= 1.5f;

        if(Random.Range(0f, shotPoint + hidePoint) < shotPoint)
            return true;
        return false;
    }

    public void StopWork()
    {
        navMeshAgent.enabled = false;
        StopAllCoroutines();
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
