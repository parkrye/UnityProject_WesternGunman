using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool isOnpen, isLeft;
    [SerializeField] float openDegree;

    public void Interact()
    {
        StartCoroutine(DoorRoutine());
    }

    IEnumerator DoorRoutine()
    {
        if (isOnpen)
        {
            isOnpen = false;
            for (int i = 0; i < 40; i++)
            {
                if(!isLeft)
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
    }
}
