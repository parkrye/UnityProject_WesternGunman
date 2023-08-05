using System.Collections;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int isGround;
    public bool IsGround { get { return (!isDisable && isGround > 0); } }
    [SerializeField] bool isDisable;
    [SerializeField] float disableDelay;

    public void Jump()
    {
        StartCoroutine(DisableCheckerRoutine());
    }

    IEnumerator DisableCheckerRoutine()
    {
        isDisable = true;
        yield return new WaitForSeconds(disableDelay);
        isDisable = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer).Equals(groundLayer.value))
            isGround++;
    }

    void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer).Equals(groundLayer.value))
            isGround--;
    }
}
