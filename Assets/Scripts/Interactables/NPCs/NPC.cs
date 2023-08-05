using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] protected GameObject worldCanvas;
    [SerializeField] protected TalkUI talkClouds;
    [SerializeField] List<GameObject> avatars;

    [SerializeField] int avatarNum;
    [SerializeField] float remainTime;
    [SerializeField] protected List<string> talks;

    public virtual void Initialize()
    {
        if(avatarNum < 0)
            avatarNum = Random.Range(0, avatars.Count);
        for (int i = 0; i < avatars.Count; i++)
        {
            if (i == avatarNum)
                avatars[i].SetActive(true);
            else
                avatars[i].SetActive(false);
        }

        talks = new List<string>();
    }

    public abstract void Interact();

    protected IEnumerator TalkRoutine()
    {
        yield return new WaitForSeconds(remainTime);
        talkClouds.gameObject.SetActive(false);
    }
}
