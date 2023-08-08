using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{
    [SerializeField] PlayerData playerData;
    [SerializeField] Dictionary<string, int> costs;
    [SerializeField] UIBase uIBase;
    [SerializeField] PlayerDataManager playerDataManager;

    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        playerDataManager = playerController.GetComponent<PlayerDataManager>();
        talks.Add("It's a Nice Day!");
        talks.Add("Today I Recommend...Apple. Look!");
        talks.Add("This Sweetroll is My Masterpiece!");
        talks.Add("Country Road~ Take Me Home~");
        talks.Add("Have you Seen My Husband?");
    }

    public override void Interact()
    {
        base.Interact();
        playerController.ControllOut();
        playerController.HideUI();
        interactUI.SetActive(true);
        SettingUICost();
        Cursor.lockState = CursorLockMode.None;
    }

    void SettingUICost()
    {
        costs = new Dictionary<string, int>
        {
            { "NowHP", 15 * (int)(playerData.MaxLife - playerData.NowLife) },
            { "MaxHP", 15 + 15 * playerData.MaxLifeCounter },
            { "NowArmor", 15 * (int)(playerData.MaxArmor - playerData.NowArmor) },
            { "MaxArmor", 15 + 15 * playerData.MaxArmorCounter }
        };

        uIBase.texts["HPNowText"].text = $"${costs["NowHP"]}";
        uIBase.texts["HPMaxText"].text = $"${costs["MaxHP"]}";
        uIBase.texts["ArmorNowText"].text = $"${costs["NowArmor"]}";
        uIBase.texts["ArmorMaxText"].text = $"${costs["MaxArmor"]}";
    }

    public void OnClickedUpgradeHPButton(bool type)
    {
        if (type)
        {
            if (playerDataManager.Money >= costs["NowHP"])
            {
                playerDataManager.Money -= costs["NowHP"];
                playerDataManager.Heal(playerData.MaxLife);
                SettingUICost();
            }
        }
        else
        {
            if (playerDataManager.Money >= costs["MaxHP"])
            {
                playerDataManager.Money -= costs["MaxHP"];
                playerDataManager.Heal(0f, 10f);
                SettingUICost();
            }
        }
    }

    public void OnClickedUpgradeArmorButton(bool type)
    {
        if (type)
        {
            if (playerDataManager.Money >= costs["NowArmor"])
            {
                playerDataManager.Money -= costs["NowArmor"];
                playerDataManager.AddArmor(playerData.MaxArmor);
                SettingUICost();
            }
        }
        else
        {
            if (playerDataManager.Money >= costs["MaxArmor"])
            {
                playerDataManager.Money -= costs["MaxArmor"];
                playerDataManager.AddArmor(0f, 10f);
                SettingUICost();
            }
        }
    }
}
