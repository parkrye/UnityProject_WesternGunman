using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable, ITalkable
{
    [SerializeField] protected GameObject worldCanvas;
    [SerializeField] protected TalkUI talkClouds;
    [SerializeField] List<GameObject> avatars;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected GameObject interactUI;

    [SerializeField] int avatarNum;
    [SerializeField] float remainTime;
    [SerializeField] protected List<string> talks;
    [SerializeField] Vector3 prevRotation;

    public virtual void Initialize(PlayerController _player)
    {
        playerController = _player;
        if (avatarNum < 0)
            avatarNum = Random.Range(0, avatars.Count);
        for (int i = 0; i < avatars.Count; i++)
        {
            if (i == avatarNum)
                avatars[i].SetActive(true);
            else
                avatars[i].SetActive(false);
        }

        talks = new List<string>();
        prevRotation = transform.localEulerAngles;
    }

    public virtual void Interact()
    {

    }

    public void EndInteract()
    {
        Cursor.lockState = CursorLockMode.Locked;
        interactUI.SetActive(false);
        playerController.ControllIn();
        playerController.ShowUI();
    }

    public virtual void Talk(Transform talker)
    {
        StopAllCoroutines();
        transform.LookAt(new Vector3(talker.position.x, transform.position.y, talker.position.z));
        talkClouds.SetText(talks[Random.Range(0, talks.Count)]);
        talkClouds.gameObject.SetActive(true);
        StartCoroutine(TalkRoutine());
    }

    protected IEnumerator TalkRoutine()
    {
        yield return new WaitForSeconds(remainTime);
        talkClouds.gameObject.SetActive(false);
        transform.localEulerAngles = prevRotation;
    }
}
