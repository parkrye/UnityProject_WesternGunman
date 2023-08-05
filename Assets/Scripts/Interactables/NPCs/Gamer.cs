using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamer : NPC
{
    public override void Initialize()
    {
        base.Initialize();
        talks.Add("Hmmm...");
        talks.Add("Wait, Wait");
        talks.Add("It's not Bad");
        talks.Add("Actually Good");
        talks.Add("Come on...!");
    }

    public override void Interact()
    {
        Debug.Log($"Interact with {name}");
        StopAllCoroutines();
        talkClouds.SetText(talks[Random.Range(0, talks.Count)]);
        talkClouds.gameObject.SetActive(true);
        StartCoroutine(TalkRoutine());
    }
}