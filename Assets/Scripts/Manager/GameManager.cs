using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<int> scoreList = new List<int>();

    public bool isGlobal;

    private string _dataPath;
    private int _score;

    private void Awake()
    {
        _dataPath = Application.persistentDataPath + "/myRecord";

        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        EventsManager.GetPoint += OnGetPoint;
        EventsManager.GameOverEvent += OnGameOver;
    }

    private void OnGetPoint(int point)
    {
        _score = point;
    }

    private void OnGameOver()
    {
        if (!isGlobal)
        {
            if (!scoreList.Contains(_score))
            {
                scoreList.Add(_score);
            }
        
            scoreList.Sort((a, b) =>
            {
                if (a < b)
                    return 1;
                else if (a == b)
                    return 0;
                else
                    return -1;
            });

            SaveData();
        }
        else
        {
            PlayFabLogin.Instance.SubmitScore(_score);
        }
    }

    public List<int> LoadData()
    {
        
        if (isGlobal)
        {
            PlayFabLogin.Instance.GetLeaderboardData();
            return PlayFabLogin.Instance.scoreList;
        }
        else
        {
            if (File.Exists(_dataPath))
            {
                string jsonData = File.ReadAllText(_dataPath);
                return JsonConvert.DeserializeObject<List<int>>(jsonData);
            }
        }


        return new List<int>();
    }
    
    private void SaveData()
    {
        File.WriteAllText(_dataPath, JsonConvert.SerializeObject(scoreList));
    }

    private void OnDisable()
    {
        EventsManager.GetPoint -= OnGetPoint;
        EventsManager.GameOverEvent -= OnGameOver;
    }
}
