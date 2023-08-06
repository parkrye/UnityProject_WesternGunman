using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{
    public override void Initialize()
    {
        base.Initialize();
        talks.Add("It's a Nice Day!");
        talks.Add("Today I Recommend...Apple. Look!");
        talks.Add("This Sweetroll is My Masterpiece!");
        talks.Add("Country Road~ Take Me Home~");
        talks.Add("Have you Seen My Husband?");
    }

    public override void Interact()
    {

    }
}
