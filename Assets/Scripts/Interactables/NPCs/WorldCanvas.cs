using Cinemachine;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{

    [SerializeField] CinemachineBrain mainCamera;

    void Update()
    {
        if(mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
