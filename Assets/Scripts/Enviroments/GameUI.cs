using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Image lifeImage;
    [SerializeField] TMP_Text moneyText;

    public void Initialize(PlayerDataManager playerDataManager)
    {
        playerDataManager.AddLifeEventListener(ModifyLife);
        playerDataManager.AddMoneyEventListener(ModifyMoney);
    }

    void ModifyLife((float, float) life)
    {
        lifeImage.fillAmount = (life.Item1 / life.Item2 );
    }

    void ModifyMoney(int money)
    {
        moneyText.text = $"${money}";
    }

    void OnTabInput(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            animator.SetTrigger("Tab");
        }
    }
}
