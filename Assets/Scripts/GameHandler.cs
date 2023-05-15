using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    private static int foodScore;
    private static int score;

    [SerializeField] private SnakeHandler snake;
    private LevelGrid levelGrid;

    private void Awake()
    {
        instance = this;
        score = 0;
    }
 
    private void Start()
    {
        levelGrid = new LevelGrid(20, 20);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
        foodScore = 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }


    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += foodScore;
    }

    public static void SnakeDies()
    {
        GameOver.ShowStatic();
    }

    // Continuar partida
    public static void ResumeGame()
    {
        Pause.HideStatic();
        Time.timeScale = 1f;
    }

    // Pausar
    public static void PauseGame()
    {
        Pause.ShowStatic();
        Time.timeScale = 0f;
    }

}
