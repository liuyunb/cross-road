using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    public Text scoreText;
    public Text nameText;
    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetNameText(string playerName)
    {
        if (PlayFabLogin.Instance.playerName == playerName)
        {
            nameText.color = Color.blue;
        }
        else
        {
            nameText.color = Color.black;
        }
        nameText.text = playerName;
    }
}
