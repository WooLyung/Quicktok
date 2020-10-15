using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GAME_STATE
{
    GAME, OVER
}

public class GameMainManager : MonoBehaviour
{
    #region Singleton
    private static GameMainManager _instance = null;

    public static GameMainManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    #endregion

    public GAME_STATE state
    {
        get;
        private set;
    }

    public int difficulty
    {
        get;
        private set;
    }

    public int complete
    {
        get;
        private set;
    }

    public Animator panel;
    public int totalRequests = 0;
    public Text info;
    public Text scoreText;
    public static int staticScore = 0;

    #region LifeCycle
    private void Awake()
    {
        if (!Instance) Instance = this;
        state = GAME_STATE.GAME;
    }
    #endregion

    public void CompleteRequire()
    {
        complete++;
        UpdateDifficulty();
    }

    public void UpdateDifficulty()
    {
        UpdateFriend();

        if (complete >= 60)
        {
            difficulty = 5;
        }
        else if (complete >= 40)
        {
            difficulty = 4;
        }
        else if (complete >= 25)
        {
            difficulty = 3;
        }
        else if (complete >= 12)
        {
            difficulty = 2;
        }
        else if (complete >= 6)
        {
            difficulty = 1;
        }
        else
        {
            difficulty = 0;
        }

        RequireManager.Instance.RequireTimeUpdate(GetTime());
    }

    public float GetTime()
    {
        if (difficulty == 0) return 50;
        else if (difficulty == 1) return 40;
        else if (difficulty == 2) return 30;
        else if (difficulty == 3) return 25;
        else if (difficulty == 4) return 20;
        else return 18;
    }

    public void InfoUpdate()
    {
        int time = (int) Mathf.Round(RequireManager.Instance.requireTime - RequireManager.Instance.time);
        int score = complete;
        int left = totalRequests;

        info.text = time + "초˙" + score + "점˙" + left + "개";
        scoreText.text = score + "점";
        staticScore = score;
    }

    public void GameOver(int code)
    {
        state = GAME_STATE.OVER;
        panel.SetBool("isOff", true);
        StartCoroutine("Over");
        InGameSound.Instance.End();
    }

    private void UpdateFriend()
    {
        if (complete < 50 && complete % 4 == 0)
            FriendsManager.Instance.AppendFriend();
    }

    IEnumerator Over()
    {
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
}
