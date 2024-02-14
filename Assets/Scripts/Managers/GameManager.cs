using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    private List<IObserver> observers = new List<IObserver>();
    private GameState currentState;

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
        currentState = gameState;
        foreach (var observer in observers)
        {
            observer.Notify(currentState);
        }
    }
    

    #endregion
}

public interface IObserver
{
    public void Notify(GameState gameState);
}

public enum GameState
{
    MainMenu,
    Playing,
    LevelCompleted,
    Death
}
public interface IGameManager
{
    public void RegisterObserver(IObserver observer);
    public void UnregisterObserver(IObserver observer);
    public void SetState(GameState gameState);


}
