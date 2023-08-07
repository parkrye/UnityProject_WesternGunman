using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    [SerializeField] public Dictionary<string, Button> buttons;
    [SerializeField] public Dictionary<string, TMP_Text> texts;

    void OnEnable()
    {
        buttons = new Dictionary<string, Button>();
        texts = new Dictionary<string, TMP_Text>();

        RectTransform[] children = gameObject.GetComponentsInChildren<RectTransform>();
        for(int i  = 0; i < children.Length; i++)
        {
            Button button = children[i].GetComponent<Button>();
            if (button)
            {
                buttons[button.name] = button;
            }

            TMP_Text text = children[i].GetComponent<TMP_Text>();
            if (text)
            {
                texts[text.name] = text;
            }
        }
    }
}
