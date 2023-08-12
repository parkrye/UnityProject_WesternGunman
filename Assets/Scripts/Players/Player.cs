using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerDataManager playerDataManager;
    [SerializeField] PlayerController playerController;
    public PlayerDataManager PlayerDataManager { get { return playerDataManager; } }
    public PlayerController PlayerController { get {  return playerController; } }
    public Vector3 PlayerBodyPosotion { get { return (transform.position + Vector3.up); } }
}
