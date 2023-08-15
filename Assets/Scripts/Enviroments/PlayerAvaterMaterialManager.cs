using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAvaterMaterialManager : MonoBehaviour
{
    [SerializeField] List<Material> materials;
    [SerializeField] int materialNum;
    UnityEvent<Material> materialChangedEvent = new();

    public void Initialize()
    {
        materialChangedEvent = new();
    }

    public int MaterialNum 
    { 
        get 
        {  
            return materialNum; 
        } 
        set 
        { 
            materialNum = value; 
            if (materialNum < 0) 
                materialNum = materials.Count - 1;
            if (materialNum > materials.Count - 1)
                materialNum = 0;
            materialChangedEvent?.Invoke(materials[materialNum]); 
        } 
    }

    public void AddMaterialChangedEventListener(UnityAction<Material> action)
    {
        materialChangedEvent.AddListener(action);
    }
}
