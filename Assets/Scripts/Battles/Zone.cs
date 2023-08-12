using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Zone : MonoBehaviour
{
    [SerializeField] List<Zone> passibleZone;
    [SerializeField] List<Zone> blockedZone;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] int zoneNumber;
    [SerializeField] float radius;

    public UnityEvent<int> PlayerEnterEvent;

    public void Initialize(Zone[] wholeZone, LayerMask _playerLayerMask, int _zoneNumber, float _radius)
    {
        PlayerEnterEvent = new UnityEvent<int>();
        passibleZone = new List<Zone>();
        blockedZone = new List<Zone>();

        playerLayerMask = _playerLayerMask;
        zoneNumber = _zoneNumber;
        radius = _radius;

        for(int i = 0; i < wholeZone.Length; i++)
        {
            if (wholeZone[i].Equals(this))
                continue;

            if (Physics.SphereCast(transform.position, radius, (transform.position - wholeZone[i].transform.position), out _))
            {
                blockedZone.Add(wholeZone[i]);
            }
            else
            {
                passibleZone.Add(wholeZone[i]);
            }
        }
    }

    public Transform GetLeastDistancePassibleZone(Transform nowTransform)
    {
        int leastDistanceIndex = 0;
        float leastDistance = float.MaxValue;
        for(int i = 0; i < passibleZone.Count; i++)
        {
            float sqrDistance = Vector3.SqrMagnitude(nowTransform.position - passibleZone[i].transform.position);
            if(sqrDistance < leastDistance)
            {
                leastDistanceIndex = i;
                leastDistance = sqrDistance;
            }
        }

        return passibleZone[leastDistanceIndex].transform;
    }

    public Transform GetLeastDistanceBlockedZone(Transform nowTransform)
    {
        int leastDistanceIndex = 0;
        float leastDistance = float.MaxValue;
        for (int i = 0; i < blockedZone.Count; i++)
        {
            float sqrDistance = Vector3.SqrMagnitude(nowTransform.position - blockedZone[i].transform.position);
            if (sqrDistance < leastDistance)
            {
                leastDistanceIndex = i;
                leastDistance = sqrDistance;
            }
        }

        return blockedZone[leastDistanceIndex].transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if((1 << other.gameObject.layer) == playerLayerMask.value)
        {
            PlayerEnterEvent?.Invoke(zoneNumber);
        }
    }
}
