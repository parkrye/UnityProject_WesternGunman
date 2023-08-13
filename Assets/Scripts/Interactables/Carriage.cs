using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carriage : MonoBehaviour, IInteractable
{
    [SerializeField] QuestData questData;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Player player;
    [SerializeField] List<QuestArea> questAreas;
    [SerializeField] Transform returnTransform;
    [SerializeField] List<Enemy> enemies;
    [SerializeField] Animator questUI;

    public void Interact()
    {
        if(questData.QuestState == GameData.QuestState.Progress)
        {
            questAreas[questData.AreaNum].gameObject.SetActive(true);
            player.PlayerController.SetEnableCharacterController(false);
            player.transform.position = questAreas[questData.AreaNum].PlayerStartTransform.position;
            player.PlayerController.SetEnableCharacterController(true);
            player.PlayerController.IsBattle = true;
            questData.EnemyKilled = 0;
            enemies = new();
            for (int i = 0; i < questData.EnemyCount; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab, questAreas[questData.AreaNum].EnemySpawnTransform.position, Quaternion.identity);
                enemy.Initialize(player, Random.Range(0, 8), Random.Range(0, 2), questAreas[questData.AreaNum].ZoneManager, this);
                enemies.Add(enemy);
            }

            questUI.SetTrigger("ShowUI");
            questUI.GetComponentInChildren<TMP_Text>().text = "Quest\nStart!";
        }
    }

    public void OnEnemyDied()
    {
        questData.EnemyKilled++;
        if(questData.EnemyKilled == questData.EnemyCount)
        {
            questData.QuestState = GameData.QuestState.Clear;

            for (int i = 0; i < enemies.Count; i++)
            {
                Destroy(enemies[i].gameObject);
            }

            questAreas[questData.AreaNum].gameObject.SetActive(false);
            player.PlayerController.IsBattle = false;

            questUI.GetComponentInChildren<TMP_Text>().text = "Quest\nClear!";
        }
    }
}
