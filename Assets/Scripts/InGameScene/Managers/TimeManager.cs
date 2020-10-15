using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    private static TimeManager _instance = null;

    public static TimeManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    #endregion

    public float time
    {
        get;
        private set;
    }

    #region LifeCycle
    private void Awake()
    {
        if (!Instance) Instance = this;
        time = 0;
    }

    private void Update()
    {
        if (GameMainManager.Instance.state != GAME_STATE.GAME)
            return;

        time += Time.deltaTime;
        GameMainManager.Instance.InfoUpdate();
        FriendsManager.Instance.SubtractTime();
    }
    #endregion
}
