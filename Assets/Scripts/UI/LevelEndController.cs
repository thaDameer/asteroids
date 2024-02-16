using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelEndController : ControllerBase<LevelEndController.Parameters>,IObserver
{
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text gainedScoreText;
    [SerializeField] private TMP_Text levelReached;
    
    [Inject] private ILevelService levelService;
    [Inject] private IScoringService scoringService;
    private IObserverService _observerService;

    [Inject]
    public void Construct(IObserverService observerService)
    {
        _observerService = observerService;
        observerService.RegisterObserver(this);
    }
    
    [Serializable]
    public class Parameters
    {
        public int TotalGainedScore;
        public int LevelReached;
    }

    public override void Setup(Parameters parameters)
    {
        base.Setup(parameters);
        gainedScoreText.text = "TOTAL SCORE :" + base.parameters.TotalGainedScore;
        levelReached.text = "YOU REACHED LEVEL: " + (parameters.LevelReached+1);

    }  

    private void AnimateEndScreen()
    {
        canvasGroup.DOFade(1, 1.5f).SetEase(Ease.InQuint);
        
    }
    public void Notify(GameState gameState)
    {
        if (gameState == GameState.GameOver)
        {
            uiContainer.gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            Setup(new Parameters
            {
                LevelReached = levelService.CurrentLevel,
                TotalGainedScore = scoringService.TotalScore
            });
            AnimateEndScreen();
        }
        else
            uiContainer.gameObject.SetActive(false);
        
    }

    public void OnClickedRetry()
    {
        _observerService.SetState(GameState.MainMenu);
    }
}
