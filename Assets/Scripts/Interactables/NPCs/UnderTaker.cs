using UnityEngine;

public class UnderTaker : NPC
{
    [SerializeField] Transform respawnTransform;
    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        talks.Add("You...Need...Something...?");
        talks.Add("... ... ...");
        talks.Add("Be...Quiet...");
        talks.Add("Why...?");
        talks.Add("Close...That...Door...");
        playerController.GetComponent<PlayerDataManager>().AddLifeEventListener(PlayerDied);
    }

    public override void Interact()
    {
        base.Interact();
        playerController.ControllOut();
        playerController.HideUI();
    }

    void PlayerDied((float, float) life)
    {
        if (life.Item1 <= 0f)
            playerController.MoveCharacterController(respawnTransform.position);
    }
}
