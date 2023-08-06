using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamer : NPC
{
    public override void Initialize()
    {
        base.Initialize();
        talks.Add("Hmmm...");
        talks.Add("Wait, Wait. Don't Get in the Way");
        talks.Add("It's not Bad");
        talks.Add("Actually Good Hands");
        talks.Add("Come on...Come on...!");
    }

    public override void Talk(Transform talker)
    {
        StopAllCoroutines();
        talkClouds.SetText(talks[Random.Range(0, talks.Count)]);
        talkClouds.gameObject.SetActive(true);
        StartCoroutine(TalkRoutine());
    }

    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);
    }
}
