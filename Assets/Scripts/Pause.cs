using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class Pause : MonoBehaviour
{
    private static Pause instance;

    // Escondemos
    public void Awake()
    {
        instance = this;
        Hide();
    }

    public void ResumeGame()
    {
        GameHandler.ResumeGame();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void ShowStatic()
    {
        instance.Show();
    }

    public static void HideStatic()
    {
        instance.Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
