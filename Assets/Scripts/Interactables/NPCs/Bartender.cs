using UnityEngine;

public class Bartender : NPC
{
    public override void Initialize()
    {
        base.Initialize();
        talks.Add("Good Morning Ya");
        talks.Add("Need Some Liquor? or Milk?");
        talks.Add("Fancy a drink?");
        talks.Add("Yo Our PieceMaker!");
        talks.Add("How about Beer?");
    }

    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);
        player.ControllOut();
        player.HideUI();
    }
}
