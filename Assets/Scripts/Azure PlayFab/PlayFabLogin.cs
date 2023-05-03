using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabLogin : MonoBehaviour
{
    private static PlayFabLogin _instance;

    public string playerName;
    public static PlayFabLogin Instance => _instance;

    public List<int> scoreList = new List<int>();
    public List<string> nameList = new List<string>();



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this);
        
        Login();
    }

    #region 登录信息
    
    public void Login()
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true
            }
        };
        
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("success Login");
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
    }
    
    #endregion

    #region 提交分数

    public void SubmitScore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = "HighScores",
                    Value = score
                }
            }
        };
        
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnSubmitScore, OnError);
    }

    public void OnSubmitScore(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("submit success");
        GetLeaderboardData();
    }

    #endregion

    #region 获取分数

    public void GetLeaderboardData()
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "HighScores",
            StartPosition = 0,
            MaxResultsCount = 8
        };
        
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderBoard, OnError);
    }

    private void OnGetLeaderBoard(GetLeaderboardResult result)
    {
        scoreList.Clear();
        nameList.Clear();

        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
            scoreList.Add(item.StatValue);
            nameList.Add(item.DisplayName);
        }
        
        EventsManager.CallUpLeaderboardUpdate();
    }

    #endregion

    #region 获取用户名

    public void SubmitName(string displayName)
    {
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = displayName
        };
        
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSubmitName, OnError);
    }

    private void OnSubmitName(UpdateUserTitleDisplayNameResult result)
    {
        playerName = result.DisplayName;
    }

    #endregion
    
    
    private void OnError(PlayFabError err)
    {
        Debug.LogWarning("something went wrong");
        Debug.LogError("Here's something debug information");
        Debug.LogError(err.GenerateErrorReport());
    }
}
