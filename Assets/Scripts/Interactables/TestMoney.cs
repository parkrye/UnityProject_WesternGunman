using UnityEngine;

public class TestMoney : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerDataManager playerDataManager = other.GetComponent<PlayerDataManager>();
        if (playerDataManager)
        {
            playerDataManager.Money += 100;
            Debug.Log(playerDataManager.Money);
        }
    }
}
