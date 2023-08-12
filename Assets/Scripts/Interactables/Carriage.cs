using System.Collections.Generic;
using UnityEngine;

public class Carriage : MonoBehaviour, IInteractable
{
    [SerializeField] QuestData questData;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] PlayerDataManager playerDataManager;
    [SerializeField] List<QuestArea> questAreas;
    public void Interact()
    {
        if(questData.QuestState == GameData.QuestState.Progress)
        {
            questAreas[questData.AreaNum].gameObject.SetActive(true);
            playerDataManager.GetComponent<CharacterController>().enabled = false;
            playerDataManager.transform.position = questAreas[questData.AreaNum].PlayerStartTransform.position;
            playerDataManager.GetComponent<CharacterController>().enabled = true;
            for (int i = 0; i < questData.EnemyCount; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab, questAreas[questData.AreaNum].EnemySpawnTransform.position, Quaternion.identity);
                enemy.Initialize(playerDataManager, Random.Range(0, 8), Random.Range(0, 2), questAreas[questData.AreaNum].ZoneManager);
            }
        }
    }
}
