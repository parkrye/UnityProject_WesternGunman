using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dealer : NPC
{
    [SerializeField] List<WeaponData> weaponDatas;
    [SerializeField] Dictionary<(GameData.Gun, int), int> costs;
    [SerializeField] UIBase uIBase;
    [SerializeField] PlayerDataManager playerDataManager;

    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        uIBase = interactUI.GetComponent<UIBase>();
        playerDataManager = playerController.GetComponent<PlayerDataManager>();
        talks.Add("You Got a Gun?");
        talks.Add("How about Bigger One?");
        talks.Add("Ah, Good Smell");
        talks.Add("Bigger is Better! And Biggest is Best!");
        talks.Add("Of Course You Have Enough Money?");
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
        costs = new Dictionary<(GameData.Gun, int), int> ();
        costs.Add((GameData.Gun.HandGun, 0), 15 + 15 * weaponDatas[0].DamageUpgradeCount);
        costs.Add((GameData.Gun.HandGun, 1), 15 + 15 * weaponDatas[0].SpeedUpgradeCount);
        costs.Add((GameData.Gun.HandGun, 2), 15 + 15 * weaponDatas[0].RangeUpgradeCount);
        costs.Add((GameData.Gun.ShotGun, 0), 15 + 15 * weaponDatas[1].DamageUpgradeCount);
        costs.Add((GameData.Gun.ShotGun, 1), 15 + 15 * weaponDatas[1].SpeedUpgradeCount);
        costs.Add((GameData.Gun.ShotGun, 2), 15 + 15 * weaponDatas[1].RangeUpgradeCount);

        uIBase.texts["HandGunDamageText"].text = $"${costs[(GameData.Gun.HandGun, 0)]}";
        uIBase.texts["HandGunSpeedText"].text = $"${costs[(GameData.Gun.HandGun, 1)]}";
        uIBase.texts["HandGunRangeText"].text = $"${costs[(GameData.Gun.HandGun, 2)]}";
        uIBase.texts["ShotGunDamageText"].text = $"${costs[(GameData.Gun.ShotGun, 0)]}";
        uIBase.texts["ShotGunSpeedText"].text = $"${costs[(GameData.Gun.ShotGun, 1)]}";
        uIBase.texts["ShotGunRangeText"].text = $"${costs[(GameData.Gun.ShotGun, 2)]}";
    }

    public void OnClickedUpgradeDamageButton(int gunNum)
    {
        if(playerDataManager.Money >= costs[((GameData.Gun)gunNum, 0)])
        {
            playerDataManager.Money -= costs[((GameData.Gun)gunNum, 0)];
            weaponDatas[gunNum].Damage *= 1.1f;
            weaponDatas[gunNum].DamageUpgradeCount++;
            SettingUICost();
        }
    }

    public void OnClickedUpgradeSpeedButton(int gunNum)
    {
        if (playerDataManager.Money >= costs[((GameData.Gun)gunNum, 1)])
        {
            playerDataManager.Money -= costs[((GameData.Gun)gunNum, 1)];
            weaponDatas[gunNum].Speed *= 0.9f;
            weaponDatas[gunNum].SpeedUpgradeCount++;
            SettingUICost();
        }
    }

    public void OnClickedUpgradeRangeButton(int gunNum)
    {
        if (playerDataManager.Money >= costs[((GameData.Gun)gunNum, 2)])
        {
            playerDataManager.Money -= costs[((GameData.Gun)gunNum, 2)];
            weaponDatas[gunNum].Range *= 1.1f;
            weaponDatas[gunNum].RangeUpgradeCount++;
            SettingUICost();
        }
    }
}

