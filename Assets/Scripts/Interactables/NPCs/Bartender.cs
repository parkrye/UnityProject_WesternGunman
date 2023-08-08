using System.Collections.Generic;
using UnityEngine;

public class Bartender : NPC
{
    [SerializeField] QuestData questData;
    [SerializeField] UIBase uIBase;
    [SerializeField] PlayerDataManager playerDataManager;

    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        playerDataManager = playerController.GetComponent<PlayerDataManager>();
        talks.Add("Good Morning Ya");
        talks.Add("Need Some Liquor? or Milk?");
        talks.Add("Fancy a drink?");
        talks.Add("Yo Our PieceMaker!");
        talks.Add("How about Beer?");
    }

    public override void Interact()
    {
        base.Interact();
        playerController.ControllOut();
        playerController.HideUI();
        interactUI.SetActive(true);

        if (questData.EnemyCount == 0)
            CreateNewQuest();
        SettingUI();
        Cursor.lockState = CursorLockMode.None;
    }

    void SettingUI()
    {
        ShowNowQuestState();
        switch (questData.QuestState)
        {
            case GameData.QuestState.None:
                uIBase.buttons["GetQuestButton"].interactable = true;
                uIBase.buttons["ResetQuestButton"].interactable = true;
                uIBase.buttons["ClearQuestButton"].interactable = false;
                break;
            case GameData.QuestState.Progress:
                uIBase.buttons["GetQuestButton"].interactable = false;
                uIBase.buttons["ResetQuestButton"].interactable = true;
                uIBase.buttons["ClearQuestButton"].interactable = false;
                break;
            case GameData.QuestState.Clear:
                uIBase.buttons["GetQuestButton"].interactable = false;
                uIBase.buttons["ResetQuestButton"].interactable = false;
                uIBase.buttons["ClearQuestButton"].interactable = true;
                break;
        }
    }

    public void OnGetQuestButtonClicked()
    {
        questData.QuestState = GameData.QuestState.Progress;
        SettingUI();
    }

    public void OnResetQuestButtonClicked()
    {
        CreateNewQuest();
        SettingUI();
    }

    public void OnClearQuestButtonClicked()
    {
        playerDataManager.Money += questData.QuestPay;
        CreateNewQuest();
        SettingUI();
    }

    void CreateNewQuest()
    {
        int enemyCount = Random.Range(1, 10);
        int pay = enemyCount * Random.Range(10, 50);
        int area = Random.Range(0, GameData.AreaCount);

        questData.EnemyCount = enemyCount;
        questData.QuestPay = pay;
        questData.AreaNum = area;
        questData.QuestState = GameData.QuestState.None;
    }

    void ShowNowQuestState()
    {
        uIBase.texts["QuestPayText"].text = $"${questData.QuestPay}";
        uIBase.texts["QuestAreaText"].text = $"Area {questData.AreaNum}";
    }
}
