using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour, IObserver,IShipLifeCounter
{
    private IObserverService _observerService;
    public int ShipsLeft { get; set; }
    public int MaxShips { get; set; }

    [Inject] private ILevelService _levelService;

    [Inject] private IScoringService _scoringService;
    public void ShipDestroyed()
    {
        ShipsLeft -= 1;
        OnLifeCountChanged?.Invoke(ShipsLeft);
        if (ShipsLeft == 0)
        {
            _observerService.SetState(GameState.GameOver);
        }
        else
        {
            _observerService.SetState(GameState.Playing);
        }
    }

    public Action<int> OnLifeCountChanged { get; set; }

    [Inject]
    public void Construct(IObserverService iObserverService)
    {
        _observerService = iObserverService;
        iObserverService.RegisterObserver(this);
    }

    private void Start()
    {
        _observerService.SetState(GameState.MainMenu);
       
    }

    
    public void Notify(GameState gameState)
    {
        if(gameState == GameState.StartRun)
            StartNewRun();
        
    }

    private void StartNewRun()
    {
        _levelService.CurrentLevel = 0;
        ShipsLeft = 3;
        MaxShips = 3;
        _scoringService.ResetScore();
        _scoringService.UpdateGainedScore(0);
        OnLifeCountChanged?.Invoke(ShipsLeft);
    }

   
 
}

public interface IShipLifeCounter
{
    public int ShipsLeft { get; set; }
    public int MaxShips { get; set; }
    public void ShipDestroyed();
    public Action<int> OnLifeCountChanged { get; set; }
    public static Action OnShipDestroyed;
}
