using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    

    public GameObject gameOverPanel;
    public GameObject leaderBoard;

    private void OnEnable()
    {
        Time.timeScale = 1;
        EventsManager.GetPoint += ChangePoint;
        EventsManager.GameOverEvent += GameOver;
    }

    public void ChangePoint(int point)
    {
        scoreText.text = point.ToString();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        EventsManager.GetPoint -= ChangePoint;
        EventsManager.GameOverEvent -= GameOver;
    }

    public void RestartGame()
    {
        AdsManager.instance.ShowAds(0);
        TransitionManager.instance.TransitionTo(SceneManager.GetActiveScene().name);
        gameOverPanel.SetActive(false);
    }

    public void BackMain()
    {
        TransitionManager.instance.TransitionTo("GameStart");
        gameOverPanel.SetActive(false);
    }

    public void ToLeaderBoard()
    {
        leaderBoard.SetActive(true);
    }
}
