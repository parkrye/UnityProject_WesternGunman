using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderTaker : NPC
{
    public override void Initialize()
    {
        base.Initialize();
        talks.Add("You...Need...Something...?");
        talks.Add("... ... ...");
        talks.Add("Be...Quiet...");
        talks.Add("Why...?");
        talks.Add("Close...That...Door...");
    }

    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);
        player.ControllOut();
        player.HideUI();
    }
}
