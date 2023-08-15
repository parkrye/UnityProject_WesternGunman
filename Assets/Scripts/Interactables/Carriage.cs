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
    [SerializeField] bool eventAdded;

    public void Interact()
    {
        if (!eventAdded)
        {
            eventAdded = true;
            player.PlayerDataManager.AddLifeEventListener(PlayerDied);
        }

        if(questData.QuestState == GameData.QuestState.Progress)
        {
            questAreas[questData.AreaNum].gameObject.SetActive(true);
            player.PlayerController.MoveCharacterController(questAreas[questData.AreaNum].PlayerStartTransform.position);
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

    void PlayerDied((float, float) life)
    {
        if(questData.QuestState == GameData.QuestState.Progress)
        {
            if(life.Item1 <= 0f)
                EndQuest();
        }
    }

    public void OnEnemyDied()
    {
        questData.EnemyKilled++;
        if(questData.EnemyKilled == questData.EnemyCount)
        {
            questData.QuestState = GameData.QuestState.Clear;
            EndQuest();
            questUI.GetComponentInChildren<TMP_Text>().text = "Quest\nClear!";
        }
    }

    public void EndQuest()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }

        questAreas[questData.AreaNum].gameObject.SetActive(false);
        player.PlayerController.IsBattle = false;
    }
}
