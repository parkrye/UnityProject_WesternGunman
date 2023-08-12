using System.Collections.Generic;
using UnityEngine;

public class Carriage : MonoBehaviour, IInteractable
{
    [SerializeField] QuestData questData;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Player player;
    [SerializeField] List<QuestArea> questAreas;
    public void Interact()
    {
        if(questData.QuestState == GameData.QuestState.Progress)
        {
            questAreas[questData.AreaNum].gameObject.SetActive(true);
            player.PlayerController.SetEnableCharacterController(false);
            player.transform.position = questAreas[questData.AreaNum].PlayerStartTransform.position;
            player.PlayerController.SetEnableCharacterController(true);
            player.PlayerController.IsBattle = true;
            for (int i = 0; i < questData.EnemyCount; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab, questAreas[questData.AreaNum].EnemySpawnTransform.position, Quaternion.identity);
                enemy.Initialize(player, Random.Range(0, 8), Random.Range(0, 2), questAreas[questData.AreaNum].ZoneManager);
            }
        }
    }
}
