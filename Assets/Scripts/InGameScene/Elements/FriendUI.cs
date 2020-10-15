using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendUI : MonoBehaviour
{
    public int code;
    public Text name;
    public Text lastChat;
    public Image profileImage;
    public Image background;
    public Image nnnn;

    public void Click()
    {
        InGameUIManager.Instance.ChatToggle(true, code);
    }
}