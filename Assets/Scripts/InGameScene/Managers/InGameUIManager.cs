using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    #region Singleton
    private static InGameUIManager _instance = null;

    public static InGameUIManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    #endregion

    public bool isChatOn
    {
        get;
        private set;
    }

    public int whoChat
    {
        get;
        private set;
    }

    private int pre = 0;

    public RectTransform friendsParent;
    public GameObject friendUI;

    public RectTransform messagesParent;
    public GameObject myMessage;
    public GameObject yourMessage;

    public InputField inputField;
    public Text friendName;
    public ScrollRect chatList;

    public Animator chattingAnim;

    #region LifeCycle
    private void Awake()
    {
        if (!Instance) Instance = this;
        isChatOn = false;
        whoChat = 0;
    }
    #endregion

    public void ChatOff()
    {
        ChatToggle(false, whoChat);
    }

    public void ChatToggle(bool flag, int whoChat)
    {
        InGameSound.Instance.PlaySound("slide");

        isChatOn = flag;
        this.whoChat = whoChat;
        chattingAnim.SetBool("isOn", isChatOn);

        if (flag)
        {
            friendName.text = FriendsManager.Instance.friends[whoChat].name;
            pre = 0;
        }

        RefreshChat();
    }

    public void SendChat()
    {
        RequireManager.Instance.RequireComplete(whoChat, inputField.text);
        inputField.text = "";
        RefreshChat();
    }

    public void RefreshChat()
    {
        if (isChatOn)
            FriendsManager.Instance.friends[whoChat].isNew = false;

        FriendsManager.Instance.sortedFriends.Sort((x1, x2) => {
            return x1.latelyTime < x2.latelyTime ? 1 : -1;
        });

        // 기존 객체 삭제
        foreach (RectTransform friend in friendsParent.GetComponentInChildren<RectTransform>())
        {
            if (friend != friendsParent)
            {
                GameObject.Destroy(friend.gameObject);
            }
        }
        
        // 채팅창 목록 변경
        float scale = 0;
        for (int i = 0; i < FriendsManager.Instance.GetFriendsCount(); i++)
        {
            var newFriend = GameObject.Instantiate(friendUI, friendsParent);
            newFriend.name = "Friend";
            var rect = newFriend.GetComponent<RectTransform>();

            scale = rect.sizeDelta.y;
            rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, scale * -i, 0);

            FriendUI fu = newFriend.GetComponent<FriendUI>();
            fu.code = FriendsManager.Instance.sortedFriends[i].code;
            fu.name.text = FriendsManager.Instance.sortedFriends[i].name;
            fu.lastChat.text = FriendsManager.Instance.sortedFriends[i].lastChat.Substring(0, FriendsManager.Instance.sortedFriends[i].lastChat.Length >= 30 ? 30 : FriendsManager.Instance.sortedFriends[i].lastChat.Length);
            if (!FriendsManager.Instance.sortedFriends[i].isNew) fu.nnnn.color = new Color(1, 1, 1, 0);
            fu.profileImage.sprite = FriendsManager.Instance.sortedFriends[i].profileImage;

            if (isChatOn && whoChat == fu.code)
                fu.background.color = new Color(215/255.0f, 226/255.0f, 223/255.0f);
        }
        friendsParent.sizeDelta = new Vector2(friendsParent.sizeDelta.x, scale * FriendsManager.Instance.GetFriendsCount());

        // 기존 객체 삭제
        if (pre <= 0)
        {
            foreach (RectTransform message in messagesParent.GetComponentInChildren<RectTransform>())
            {
                if (message != messagesParent)
                {
                    GameObject.Destroy(message.gameObject);
                }
            }
        }

        // 채팅창 변경
        for (int i = pre; i < FriendsManager.Instance.friends[whoChat].chattings.Count; i++)
        {
            GameObject newMessage;
            if (FriendsManager.Instance.friends[whoChat].chattings[i].isMe)
                newMessage = GameObject.Instantiate(myMessage, messagesParent);
            else
                newMessage = GameObject.Instantiate(yourMessage, messagesParent);

            newMessage.name = "message";
            var rect = newMessage.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector3(-4000, scale * -i, 0);
            StartCoroutine(AppearMessage(rect, scale * -i));

            MessageUI mu = newMessage.GetComponent<MessageUI>();
            mu.text.text = FriendsManager.Instance.friends[whoChat].chattings[i].message;
            if (!FriendsManager.Instance.friends[whoChat].chattings[i].isMe) mu.profileImage.sprite = FriendsManager.Instance.friends[whoChat].profileImage;
            if (mu.text.text.Length >= 30) mu.text.text = mu.text.text.Insert(28, "\n");
            if (mu.text.text.Length >= 60) mu.text.text = mu.text.text.Insert(56, "\n");
        }

        pre = FriendsManager.Instance.friends[whoChat].chattings.Count;
        messagesParent.sizeDelta = new Vector2(messagesParent.sizeDelta.x, scale * FriendsManager.Instance.friends[whoChat].chattings.Count);
        chatList.normalizedPosition = new Vector2(0, 0);
    }

    IEnumerator AppearMessage(RectTransform rect, float y)
    {
        yield return new WaitForEndOfFrame();

        try
        {
            rect.anchoredPosition = new Vector3(0, y, 0);
        }
        catch (MissingReferenceException e)
        {
        };
    }
}