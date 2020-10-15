using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FriendsManager : MonoBehaviour
{
    #region Singleton
    private static FriendsManager _instance = null;

    public static FriendsManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    #endregion

    private string nameJSON = "";
    public List<Friend> friends = new List<Friend>();
    public List<Friend> sortedFriends = new List<Friend>();
    public List<string> names = new List<string>();
    public List<Sprite> profiles = new List<Sprite>();

    #region LifeCycle
    private void Awake()
    {
        if (!Instance) Instance = this;

        LoadJSON();
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            AppendFriend();
            friends[i].latelyTime = 0.1f * i;
        }

        InGameUIManager.Instance.RefreshChat();
    }
    #endregion

    public void AppendFriend()
    {
        // 새 친구 객체 생성
        Friend friend = new Friend();
        friend.name = names[friends.Count];
        friend.code = friends.Count;
        friend.profileImage = profiles[friends.Count];

        // 친구 목록에 추가
        friends.Add(friend);
        sortedFriends.Add(friend);
        RefreshFriendList();
    }

    public void Require(int sender, int receiver, string senderMsg, string requireMsg)
    {
        if (senderMsg != "")
        {
            friends[sender].isNew = true;

            friends[sender].chattings.Add(new Chatting(false, senderMsg)); // 보낸 사람 채팅창 추가
            friends[sender].lastChat = senderMsg;
            friends[sender].latelyTime = TimeManager.Instance.time;
        }
        friends[receiver].requires.Add(new RequireMsg(requireMsg, GameMainManager.Instance.GetTime())); // 받는 사람 요청 추가

        GameMainManager.Instance.totalRequests++;
        InGameUIManager.Instance.RefreshChat();
    }

    public int GetFriendsCount()
    {
        return friends.Count;
    }

    public void SubtractTime()
    {
        foreach (Friend friend in friends)
        {
            foreach (RequireMsg require in friend.requires)
            {
                require.leftTime -= Time.deltaTime;
                if (require.leftTime < 0)
                {
                    GameMainManager.Instance.GameOver(0);
                }
            }
        }
    }

    public void RequireComplete(int friend, string message)
    {
        if (message == "")
            return;

        int index = friends[friend].requires.FindIndex((require) => { return require.message == message; });

        friends[friend].chattings.Add(new Chatting(true, message)); // 채팅 추가
        friends[friend].lastChat = message;
        friends[friend].latelyTime = TimeManager.Instance.time;

        if (index != -1) // 요청 목록 중에 해당 메세지가 있을 경우
        {
            friends[friend].requires.RemoveAt(index);
            friends[friend].chattings.Add(new Chatting(false, "고마워!")); // 채팅 추가
            friends[friend].lastChat = "고마워!";

            GameMainManager.Instance.CompleteRequire();
            GameMainManager.Instance.totalRequests--;

            Debug.Log(GameMainManager.Instance.complete);

            if (GameMainManager.Instance.totalRequests <= 0)
            {
                RequireManager.Instance.time = RequireManager.Instance.requireTime - 5;
            }
        }

        InGameUIManager.Instance.RefreshChat();
    }

    private void LoadJSON()
    {
        TextAsset data = Resources.Load("name", typeof(TextAsset)) as TextAsset;
        nameJSON = data.text;

        JSONObject json = new JSONObject(nameJSON);
        for (int i = 0; i < json.Count; i++)
        {
            string name = json[i].ToString();
            name = name.Substring(1, name.Length - 2);
            names.Add(name);
        }

        // 셔플
        for (int i = 0; i < names.Count; i++)
        {
            string temp = names[i];
            int randomIndex = Random.Range(i, names.Count);
            names[i] = names[randomIndex];
            names[randomIndex] = temp;
        }

        // 프사 셔플
        for (int i = 0; i < profiles.Count; i++)
        {
            Sprite temp = profiles[i];
            int randomIndex = Random.Range(i, profiles.Count);
            profiles[i] = profiles[randomIndex];
            profiles[randomIndex] = temp;
        }
    }

    private void RefreshFriendList()
    {
        InGameUIManager.Instance.RefreshChat();
    }
}

public class Friend
{
    public string name;
    public int code;
    public float latelyTime;
    public string lastChat = "";
    public bool isNew = false;
    public Sprite profileImage;
    public List<RequireMsg> requires = new List<RequireMsg>();
    public List<Chatting> chattings = new List<Chatting>();
}

public class RequireMsg
{
    public string message;
    public float leftTime;

    public RequireMsg(string message, float leftTime)
    {
        this.message = message;
        this.leftTime = leftTime;
    }
}

public class Chatting
{
    public bool isMe;
    public string message;

    public Chatting(bool isMe, string message)
    {
        this.isMe = isMe;
        this.message = message;
    }
}