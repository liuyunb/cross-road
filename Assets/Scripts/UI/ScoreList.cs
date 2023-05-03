using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreList : MonoBehaviour
{
    public List<ScoreBar> scoreBars;

    private List<int> _scoreList;

    private List<string> _nameList;

    private void OnEnable()
    {
        EventsManager.OnLeaderboardUpdate += SetListData;
        _scoreList = GameManager.instance.LoadData();
        SetScore();
    }

    public void SetListData()
    {
        _nameList = PlayFabLogin.Instance.nameList;
        _scoreList = PlayFabLogin.Instance.scoreList;
        SetScore();
    }
    
    public void SetScore()
    {
        _nameList = PlayFabLogin.Instance.nameList;
        
        for (int i = 0; i < scoreBars.Count; i++)
        {
            if (i < _scoreList.Count)
            {
                scoreBars[i].SetScoreText(_scoreList[i]);
                scoreBars[i].SetNameText(_nameList[i]);
                scoreBars[i].gameObject.SetActive(true);
            }
            else
            {
                scoreBars[i].gameObject.SetActive(false);
            }
        }
    }
    
}
