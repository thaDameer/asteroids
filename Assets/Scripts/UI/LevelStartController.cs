using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelStartController : ControllerBase<LevelStartController.Parameters>
{
    [SerializeField] protected TMP_Text levelLabel;
    [SerializeField] protected CanvasGroup canvasGroup;
    private ILevelService _levelService;

    [Inject]
    public void Construct(ILevelService levelService)
    {
        levelService.OnNewLevelStarted += UpdateLevelText;
    }
    [SerializeField]
    public class Parameters
    {
        public int Level;
    }

    private void Awake()
    {
        
        canvasGroup.alpha = 0;
    }


    public override void Setup(Parameters parameters)
    {
        base.Setup(parameters);
        levelLabel.text = "LEVEL " + (parameters.Level+1);
    }

    private void UpdateLevelText(int currentLevel)
    {
        Setup(new Parameters{Level =  currentLevel});
        StartCoroutine(DelayedHide_CR());
        IEnumerator DelayedHide_CR()
        {
            yield return canvasGroup.DOFade(1, 0.5f).SetEase(Ease.InQuint).WaitForCompletion();
            yield return new WaitForSeconds(3);
            canvasGroup.DOFade(0, 0.5f).SetEase(Ease.OutQuint);
        }
    }
}
