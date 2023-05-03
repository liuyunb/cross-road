using System;

public static class EventsManager
{
    public static event Action<int> GetPoint;

    public static void CallUpOnGetPoint(int point)
    {
        GetPoint?.Invoke(point);
    }   
    
    public static event Action GameOverEvent;

    public static void CallUpOnGameOver()
    {
        GameOverEvent?.Invoke();
    }

    public static event Action OnLeaderboardUpdate;
    
    public static void CallUpLeaderboardUpdate()
    {
        OnLeaderboardUpdate?.Invoke();
    }
}
