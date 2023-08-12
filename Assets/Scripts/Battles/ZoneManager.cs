using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] Zone[] wholeZone;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] int playerPositionZone;
    [SerializeField] float radius;
    public int GetPlayerPositionedZone { get { return playerPositionZone; } }

    void Awake()
    {
        wholeZone = GetComponentsInChildren<Zone>();
        for(int i = 0; i < wholeZone.Length; i++)
        {
            wholeZone[i].Initialize(wholeZone, playerLayerMask, i, radius);
            wholeZone[i].PlayerEnterEvent.AddListener(PlayerMoveZone);
        }
    }

    void PlayerMoveZone(int zoneNum)
    {
        playerPositionZone = zoneNum;
    }
}
