using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    private void Awake()
    {


    }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }


    public void Quit()
    {
        Application.Quit();
    }
}
