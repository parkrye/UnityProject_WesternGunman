using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : NPC
{
    public override void Initialize()
    {
        base.Initialize();
        talks.Add("You Got a Gun?");
        talks.Add("How about Bigger One?");
        talks.Add("Ah, Good Smell");
        talks.Add("Bigger is Better!");
        talks.Add("Of Course You Have Enough Money?");
    }

    public override void Interact()
    {

    }
}

