using UnityEngine;

public class Bartender : NPC
{
    public override void Initialize()
    {
        base.Initialize();
        talks.Add("Good Morning Ya");
        talks.Add("Need Some Liquor?");
        talks.Add("Fancy a drink?");
        talks.Add("Yo Piece Maker!");
        talks.Add("How about Beer?");
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
