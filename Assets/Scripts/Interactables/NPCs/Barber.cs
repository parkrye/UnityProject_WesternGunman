using UnityEngine;

public class Barber : NPC
{
    [SerializeField] PlayerAvaterMaterialManager playerAvaterMaterialManager;
    [SerializeField] int prevStyleNum;
    [SerializeField] PlayerDataManager playerDataManager;

    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        playerDataManager = playerController.GetComponent<PlayerDataManager>();
        talks.Add("Good clothes open all doors");
        talks.Add("Manners maketh man");
        talks.Add("Hello Gentleman");
        talks.Add("If you want to get married, grow a beard");
        talks.Add("The scissors are rusty...What a joke! Haha!");
    }

    public override void Interact()
    {
        base.Interact();
        playerController.ControllOut();
        playerController.HideUI();
        interactUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        prevStyleNum = playerAvaterMaterialManager.MaterialNum;
    }

    public void OnChangeStyleButtonClicked(bool isLeft)
    {
        if(isLeft)
        {
            playerAvaterMaterialManager.MaterialNum--;
        }
        else
        {
            playerAvaterMaterialManager.MaterialNum++;
        }
    }

    public void OnConfirmStyleButtonClicked()
    {
        if(playerDataManager.Money > 10)
        {
            playerDataManager.Money -= 10;
            playerDataManager.PlayerData.MaterialNum = playerAvaterMaterialManager.MaterialNum;
            EndInteract();
        }
    }

    public void OnCancelStyleButtonClicked()
    {
        playerAvaterMaterialManager.MaterialNum = prevStyleNum;
        EndInteract();
    }
}