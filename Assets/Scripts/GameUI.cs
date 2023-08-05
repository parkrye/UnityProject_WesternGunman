using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Image lifeImage;

    public void Initialize(PlayerDataManager playerDataManager)
    {
        playerDataManager.AddLifeEventListener(ModifyLife);
    }

    void ModifyLife((float, float) life)
    {
        lifeImage.fillAmount = (life.Item1 / life.Item2 );
    }

    void OnTabInput(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            animator.SetTrigger("Tab");
        }
    }
}
