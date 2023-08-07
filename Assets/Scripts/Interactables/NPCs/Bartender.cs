using UnityEngine;

public class Bartender : NPC
{
    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
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
    }
}
