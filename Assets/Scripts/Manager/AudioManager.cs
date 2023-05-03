using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clip")] 
    public AudioClip bgmClip;

    public AudioClip jumpClip;

    public AudioClip longJumpClip;
    public AudioClip deadClip;

    [Header("Audio Source")] 
    public AudioSource bgmMusic;

    public AudioSource triggerMusic;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        
        ToggleBgm();
        
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        EventsManager.GameOverEvent += OnGameOver;
    }

    private void OnDisable()
    {
        EventsManager.GameOverEvent -= OnGameOver;
    }

    private void OnGameOver()
    {
        SetTriggerMusic(2);
        PlayTriggerMusic();
    }

    /// <summary>
    /// 设置单次音效
    /// </summary>
    /// <param name="type">0为小跳，1为长跳，2为死亡</param>
    public void SetTriggerMusic(int type)
    {
        switch (type)
        {
            case 0:
                triggerMusic.clip = jumpClip;
                break;
            case 1:
                triggerMusic.clip = longJumpClip;
                break;
            case 2:
                triggerMusic.clip = deadClip;
                break;
        }
    }

    public void PlayTriggerMusic()
    {
        triggerMusic.Play();
    }
    
    public void ToggleBgm()
    {
        if(bgmMusic.isPlaying)
            bgmMusic.Stop();
        else
        {
            bgmMusic.clip = bgmClip;
            bgmMusic.loop = true;
            bgmMusic.Play();
        }
    }
}
