using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSound : MonoBehaviour
{
    #region Singleton
    private static InGameSound _instance = null;

    public static InGameSound Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    #endregion

    public AudioClip slide;
    public AudioClip recieveMeessage;
    public AudioClip sendMessage;
    public AudioClip gameOver;
    public AudioSource audio;

    private bool isEnd = false;
    private float time = 1;

    #region LifeCycle
    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Update()
    {
        if (isEnd)
        {
            time -= Time.deltaTime;
            if (time < 0) time = 0;
            audio.volume = time * 0.05f;
        }
    }
    #endregion

    public void End()
    {
        isEnd = true;
    }

    public void PlaySound(string sound)
    {
        if (sound == "slide")
            audio.PlayOneShot(slide, 0.7f);
        if (sound == "recieveMeessage")
            audio.PlayOneShot(recieveMeessage);
        if (sound == "sendMessage")
            audio.PlayOneShot(sendMessage);
        if (sound == "gameOver")
            audio.PlayOneShot(gameOver);
    }
}
