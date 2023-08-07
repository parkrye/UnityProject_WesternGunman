using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderTaker : NPC
{
    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        talks.Add("You...Need...Something...?");
        talks.Add("... ... ...");
        talks.Add("Be...Quiet...");
        talks.Add("Why...?");
        talks.Add("Close...That...Door...");
    }

    public override void Interact()
    {
        base.Interact();
        playerController.ControllOut();
        playerController.HideUI();
    }
}
