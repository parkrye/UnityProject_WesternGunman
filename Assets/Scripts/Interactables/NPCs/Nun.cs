using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nun : NPC
{
    public override void Initialize(PlayerController _player)
    {
        base.Initialize(_player);
        talks.Add("Pastor Went to a Neighboring Town");
        talks.Add("What Brings You Here?");
        talks.Add("Me? I'm a Nun! Even If It's Like This...");
        talks.Add("You're a Sheriff, Right?");
        talks.Add("Do You Want to Pray?");
    }

    public override void Interact()
    {
        base.Interact();
        playerController.ControllOut();
        playerController.HideUI();
    }
}
