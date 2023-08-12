using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] Zone[] wholeZone;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] int playerPositionZone;
    [SerializeField] float radius;
    public int PlayerPositionedZone { get { return playerPositionZone; } }
    public Zone[] Zone { get { return wholeZone; } }

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
