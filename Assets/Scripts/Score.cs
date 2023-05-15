using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class Score : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();

        //int highscore = HighScore.GetHighScore(); // Leemos Puntuación Max
        //transform.Find("highscoreText").GetComponent<Text>().text = "PUNTUACION MAX:" + highscore.ToString();
    }

    private void Update()
    {
        scoreText.text = GameHandler.GetScore().ToString();
    }
}
