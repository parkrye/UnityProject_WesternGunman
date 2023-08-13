using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Image lifeImage;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] Slider armorSlider;

    public void Initialize(PlayerDataManager playerDataManager, PlayerData playerData)
    {
        playerDataManager.AddLifeEventListener(ModifyLife);
        playerDataManager.AddMoneyEventListener(ModifyMoney);
        playerDataManager.AddArmorEventListener(ModifyArmor);

        ModifyLife((playerData.NowLife, playerData.MaxLife));
        ModifyArmor((playerData.NowArmor, playerData.MaxArmor));
        ModifyMoney(playerData.Money);
    }

    void ModifyLife((float, float) life)
    {
        lifeImage.fillAmount = (life.Item1 / life.Item2 );
    }

    void ModifyMoney(int money)
    {
        moneyText.text = $"${money}";
    }

    void ModifyArmor((float, float) armor)
    {
        armorSlider.maxValue = armor.Item2;
        armorSlider.value = armor.Item1;
    }

    void OnTabInput(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            animator.SetTrigger("Tab");
        }
    }
}
