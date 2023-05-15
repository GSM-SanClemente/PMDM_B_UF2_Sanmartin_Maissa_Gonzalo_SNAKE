using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    private static GameOver instance;

    // Escondemos el botón
    public void Awake()
    {
        instance = this;
        Hide();
    }

    // Volver
    public void Volver()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void ShowStatic()
    {
        instance.Show();
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
