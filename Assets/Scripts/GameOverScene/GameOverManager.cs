using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Text a;
    public Animator b;

    void Start()
    {
        a.text = GameMainManager.staticScore + "점";
    }

    public void Main1()
    {
        b.SetBool("isOn", true);
        StartCoroutine("BB");
    }

    public void End()
    {
        Application.Quit();
    }

    IEnumerator BB()
    {
        yield return new WaitForSeconds(0.8f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

    }
}
