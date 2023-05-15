using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundHandler
{
    public static void PlayMoveSound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GameAssets.i.snakeMovement);
    }

    public static void PlayEatSound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GameAssets.i.eatSound);
    }

    public static void PlayDieSound()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GameAssets.i.snakeDie);
    }
}
