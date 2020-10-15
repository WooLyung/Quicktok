using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Animator panel;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            panel.SetBool("isOn", true);
            StartCoroutine("ChangeScene");
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadScene(1);
    }
}
