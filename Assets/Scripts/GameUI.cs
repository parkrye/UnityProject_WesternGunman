using UnityEngine;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    [SerializeField] Animator animator;

    void OnTabInput(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            animator.SetTrigger("Tab");
        }
    }
}
