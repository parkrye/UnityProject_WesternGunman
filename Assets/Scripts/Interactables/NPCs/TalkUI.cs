using TMPro;
using UnityEngine;

public class TalkUI : MonoBehaviour
{
    [SerializeField] TMP_Text talkText;

    public void SetText(string text)
    {
        talkText.text = text;
    }
}
