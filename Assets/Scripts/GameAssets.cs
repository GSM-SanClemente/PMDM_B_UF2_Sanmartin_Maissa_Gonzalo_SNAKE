using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class GameAssets : MonoBehaviour
{
    public static GameAssets i;

    private void Awake() {
        i = this;
    }

    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite snakeBodySprite;

    public AudioClip snakeMovement;
    public AudioClip eatSound;
    public AudioClip snakeDie;
}
