using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zone : MonoBehaviour
{
    [SerializeField] List<Zone> passibleZone;
    [SerializeField] List<Zone> blockedZone;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] int zoneNumber;
    public List<Zone> PassibleZone { get { return passibleZone; } }
    public List<Zone> BlockedZone { get { return blockedZone; } }
    public int ZoneNumber { get { return zoneNumber; } }

    public UnityEvent<int> PlayerEnterEvent;

    public void Initialize(Zone[] wholeZone, LayerMask _playerLayerMask, int _zoneNumber)
    {
        PlayerEnterEvent = new UnityEvent<int>();
        passibleZone = new List<Zone>();
        blockedZone = new List<Zone>();

        playerLayerMask = _playerLayerMask;
        zoneNumber = _zoneNumber;

        for(int i = 0; i < wholeZone.Length; i++)
        {
            if (wholeZone[i].Equals(this))
                continue;

            if (Physics.Raycast(transform.position, (wholeZone[i].transform.position - transform.position).normalized, out RaycastHit hit, (wholeZone[i].transform.position - transform.position).magnitude))
            {
                
                blockedZone.Add(wholeZone[i]);
            }
            else
            {
                passibleZone.Add(wholeZone[i]);
            }
        }
    }

    public int GetLeastDistancePassibleZone(Transform nowTransform, List<int> exceptNums = null)
    {
        int leastDistanceIndex = 0;
        float leastDistance = float.MaxValue;
        for (int i = 0; i < passibleZone.Count; i++)
        {
            if(exceptNums.Count > 0)
            {
                bool pass = false;
                for(int j = 0; j < exceptNums.Count; j++)
                {
                    if (passibleZone[i].ZoneNumber == exceptNums[j])
                    {
                        pass = true;
                    }
                }
                if (pass)
                {
                    continue;
                }
            }
            float sqrDistance = Vector3.SqrMagnitude(passibleZone[i].transform.position - nowTransform.position);
            if(sqrDistance < leastDistance)
            {
                leastDistanceIndex = i;
                leastDistance = sqrDistance;
            }
        }

        return passibleZone[leastDistanceIndex].ZoneNumber;
    }

    public int GetLeastDistanceBlockedZone(Transform nowTransform, List<int> exceptNums = null)
    {
        int leastDistanceIndex = 0;
        float leastDistance = float.MaxValue;
        for (int i = 0; i < blockedZone.Count; i++)
        {
            if (exceptNums.Count > 0)
            {
                bool pass = false;
                for (int j = 0; j < exceptNums.Count; j++)
                {
                    if (blockedZone[i].ZoneNumber == exceptNums[j])
                    {
                        pass = true;
                    }
                }
                if (pass)
                {
                    continue;
                }
            }

            float sqrDistance = Vector3.SqrMagnitude(blockedZone[i].transform.position - nowTransform.position);
            if (sqrDistance < leastDistance)
            {
                leastDistanceIndex = i;
                leastDistance = sqrDistance;
            }
        }

        return blockedZone[leastDistanceIndex].ZoneNumber;
    }

    void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer) == playerLayerMask.value)
        {
            PlayerEnterEvent?.Invoke(zoneNumber);
        }
    }
}
