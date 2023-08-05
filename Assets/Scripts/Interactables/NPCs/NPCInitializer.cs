using System.Collections.Generic;
using UnityEngine;

public class NPCInitializer : MonoBehaviour
{
    [SerializeField] List<NPC> npcs;

    public void Initialize()
    {
        for(int i = 0; i < npcs.Count; i++)
        {
            npcs[i].Initialize();
        }
    }
}
