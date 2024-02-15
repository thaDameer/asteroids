using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour,IObserver
{

    
    [SerializeField] private Button playButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button backButton;

    [SerializeField] private CanvasGroup howToPlayPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    private void Awake()
    {
        howToPlayPanel.gameObject.SetActive(false);
  
    }

  
    private IObserverService _iObserverService;

    [Inject]
    public void Construct(IObserverService observerService)
    {
        _iObserverService = observerService;
        _iObserverService.RegisterObserver(this);
    }

    

    public void ClickedPlay()
    {
        _iObserverService.SetState(GameState.StartRun);
    }

    private TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore;
    public void ClickedHowToPlay()
    {
        howToPlayPanel.transform.localScale = Vector3.zero;
        howToPlayPanel.gameObject.SetActive(true);
        if (tweenerCore != null)
        {
            if(tweenerCore.IsPlaying())return;
        }

        howToPlayPanel.DOFade(1, 0.2f).SetEase(Ease.OutQuint);
        tweenerCore = howToPlayPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuint);
    }

    public void ClickedBackButton()
    {
        howToPlayPanel.transform.localScale = Vector3.one;
        if (tweenerCore != null)
        {
            if(tweenerCore.IsPlaying())return;
        }
        tweenerCore = howToPlayPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuint).OnComplete((() => howToPlayPanel.gameObject.SetActive(false)));
    }

    public void Notify(GameState gameState)
    {
        gameObject.SetActive(gameState == GameState.MainMenu);
    }

    private void Hide()
    {
        
    }
}
