using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireManager : MonoBehaviour
{
    #region Singleton
    private static RequireManager _instance = null;

    public static RequireManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    #endregion

    public float requireTime = 50;
    public float time = 45;
    private List<List<string>> formats = new List<List<string>>();
    private List<string> messages = new List<string>();
    private string formatJSON = "";
    private string messageJSON = "";

    #region LifeCycle
    private void Awake()
    {
        if (!Instance) Instance = this;

        LoadJSON();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= requireTime)
        {
            time = 0;
            Require();
        }
    }
    #endregion

    public void RequireTimeUpdate(float requireTime)
    {
        this.requireTime = requireTime;
    }

    public void RequireComplete(int friend, string message)
    {
        if (GameMainManager.Instance.state != GAME_STATE.GAME)
            return;

        InGameSound.Instance.PlaySound("recieveMeessage");
        FriendsManager.Instance.RequireComplete(friend, message);
    }

    private int[] GetRandomIntArray(int max)
    {
        int[] indexs = new int[max];
        for (int i = 0; i < indexs.Length; i++)
        {
            indexs[i] = i;
        }
        for (int i = 0; i < max * 10; i++)
        {
            int rand = Random.Range(0, indexs.Length);
            int tmp = indexs[0];
            indexs[0] = indexs[rand];
            indexs[rand] = tmp;
        }

        return indexs;
    }

    private void Require()
    {
        if (GameMainManager.Instance.state != GAME_STATE.GAME)
            return;

        InGameSound.Instance.PlaySound("recieveMeessage");

        int[] nameIndex = GetRandomIntArray(FriendsManager.Instance.GetFriendsCount());
        int[] messageIndex = GetRandomIntArray(messages.Count);
        int[] formatIndex1 = { 0 };
        int[] formatIndex2;

        switch (GameMainManager.Instance.difficulty) // 난이도에 따라 포맷인덱스1 설정
        {
            case 0:
                formatIndex1 = GetRandomIntArray(1);
                break;
            case 1:
                formatIndex1 = GetRandomIntArray(3);
                break;
            case 2:
                formatIndex1 = GetRandomIntArray(5);
                break;
            case 3:
                formatIndex1 = GetRandomIntArray(7);
                break;
            case 4:
                formatIndex1 = GetRandomIntArray(9);
                break;
            case 5:
                formatIndex1 = GetRandomIntArray(10);
                break;
        }

        formatIndex2 = GetRandomIntArray(formats[formatIndex1[0]].Count);
        string senderMessage = formats[formatIndex1[0]][formatIndex2[0]];
        var names = FriendsManager.Instance.names;

        switch (formatIndex1[0]) // 포맷에 따라 요청 설정
        {
            case 0:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[1], senderMessage, messages[messageIndex[0]]);
                break;
            case 1:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], messages[messageIndex[1]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[1], senderMessage, messages[messageIndex[1]]);
                break;
            case 2:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], names[nameIndex[2]], messages[messageIndex[0]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[1], senderMessage, messages[messageIndex[0]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], "", messages[messageIndex[0]]);
                break;
            case 3:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], names[nameIndex[2]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], senderMessage, messages[messageIndex[0]]);
                break;
            case 4:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], names[nameIndex[2]], messages[messageIndex[1]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[1], senderMessage, messages[messageIndex[0]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], "", messages[messageIndex[1]]);
                break;
            case 5:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], names[nameIndex[2]], messages[messageIndex[1]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], senderMessage, messages[messageIndex[1]]);
                break;
            case 6:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], messages[messageIndex[1]], names[nameIndex[2]], messages[messageIndex[2]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[1], senderMessage, messages[messageIndex[1]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], "", messages[messageIndex[2]]);
                break;
            case 7:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], names[nameIndex[2]], messages[messageIndex[1]], names[nameIndex[3]], messages[messageIndex[2]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[1], senderMessage, messages[messageIndex[0]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], "", messages[messageIndex[1]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[3], "", messages[messageIndex[2]]);
                break;
            case 8:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], names[nameIndex[2]], messages[messageIndex[1]], names[nameIndex[3]], messages[messageIndex[2]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], senderMessage, messages[messageIndex[1]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[3], "", messages[messageIndex[2]]);
                break;
            case 9:
                senderMessage = string.Format(senderMessage, names[nameIndex[1]], messages[messageIndex[0]], messages[messageIndex[1]], names[nameIndex[2]], messages[messageIndex[2]], names[nameIndex[3]], messages[messageIndex[3]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[1], senderMessage, messages[messageIndex[1]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[2], "", messages[messageIndex[2]]);
                FriendsManager.Instance.Require(nameIndex[0], nameIndex[3], "", messages[messageIndex[3]]);
                break;
        }
    }

    private void LoadJSON()
    {
        TextAsset data = Resources.Load("message", typeof(TextAsset)) as TextAsset;
        messageJSON = data.text;

        JSONObject json = new JSONObject(messageJSON);

        for (int i = 0; i < json.Count; i++)
        {
            string message = json[i].ToString();
            message = message.Substring(1, message.Length - 2);
            messages.Add(message);
        }

        TextAsset data2 = Resources.Load("format", typeof(TextAsset)) as TextAsset;
        formatJSON = data2.text;

        JSONObject json2 = new JSONObject(formatJSON);
        for (int i = 0; i < json2.Count; i++)
        {
            List<string> formatDatas = new List<string>();
            formats.Add(formatDatas);

            JSONObject formatData = json2[i];

            for (int j = 0; j < formatData.Count; j++)
            {
                string format = formatData[j].ToString();
                format = format.Substring(1, format.Length - 2);
                formatDatas.Add(format);
            }
        }
    }
}