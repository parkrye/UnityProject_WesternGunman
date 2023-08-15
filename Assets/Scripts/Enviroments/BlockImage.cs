using UnityEngine;

public class BlockImage : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 1f);
    }
}
