using UnityEngine;

public class PlayerPortrait : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedMesh;

    public void ChangeMaterial(Material material)
    {
        skinnedMesh.material = material;
    }
}
