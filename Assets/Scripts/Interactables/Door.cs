using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, ILockable
{
    [SerializeField] bool isOnpen, isLeft, isLocked;
    [SerializeField] float openDegree;

    public void Interact()
    {
        StartCoroutine(DoorRoutine());
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    IEnumerator DoorRoutine()
    {
        if (!isLocked)
        {
            GetComponent<Collider>().enabled = false;
            if (isOnpen)
            {
                isOnpen = false;
                for (int i = 0; i < 40; i++)
                {
                    if (!isLeft)
                        gameObject.transform.localEulerAngles -= new Vector3(0, openDegree * 0.025f, 0);
                    else
                        gameObject.transform.localEulerAngles += new Vector3(0, openDegree * 0.025f, 0);
                    yield return new WaitForSeconds(0.0125f);
                }
            }
            else
            {
                isOnpen = true;
                for (int i = 0; i < 40; i++)
                {
                    if (!isLeft)
                        gameObject.transform.localEulerAngles += new Vector3(0, openDegree * 0.025f, 0);
                    else
                        gameObject.transform.localEulerAngles -= new Vector3(0, openDegree * 0.025f, 0);
                    yield return new WaitForSeconds(0.0125f);
                }
            }
            GetComponent<Collider>().enabled = true;
        }
    }
}
