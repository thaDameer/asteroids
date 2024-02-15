using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObserverService : MonoBehaviour, IObserverService
{
    private List<IObserver> observers = new List<IObserver>();
    
    
    private void Start()
    {
        SetState(GameState.MainMenu);
    }

   
    #region Oberver Logic

    public void RegisterObserver(IObserver observer)
    {
        if(!observers.Contains(observer))
            observers.Add(observer);
    }

    public void UnregisterObserver(IObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }
    
    public void SetState(GameState gameState)
    {
        if(CurrentState == gameState)return;
        
        CurrentState = gameState;
        foreach (var observer in observers)
        {
            observer.Notify(CurrentState);
        }
    }

    public GameState CurrentState { get; set; }

    #endregion
}

public interface IObserver
{
    public void Notify(GameState gameState);
}

public enum GameState
{
    MainMenu,
    StartRun,
    Playing,
    LevelCompleted,
    Death,
    GameOver
}
public interface IObserverService
{
    public void RegisterObserver(IObserver observer);
    public void UnregisterObserver(IObserver observer);
    public void SetState(GameState gameState);
    public GameState CurrentState { get; set; }
    
}

public interface IScoring
{
    public int LivesLeft { get; set; }
}
