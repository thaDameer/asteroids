using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreGainedController : ControllerBase<ScoreGainedController.Parameters>
{
    [SerializeField] private TMP_Text gainedText;
    [SerializeField] private TMP_Text totalText;
    private IScoringService _scoringService;

    [Inject]
    public void Construct(IScoringService scoringService)
    {
        _scoringService = scoringService;
        scoringService.OnUpdateGainedScore += OnGainedScore;
    }
    
    [Serializable]
    public class Parameters
    {
        public int GainedScore;
        public int TotalScore;
        
    }
    
    private void OnGainedScore(int gainedScore)
    {
        if (gainedScore > 0)
        {
            Setup(new Parameters
            {
                GainedScore = gainedScore,
                TotalScore = _scoringService.TotalScore
            });
        }
        else
        {
            totalText.text = "";
            gainedText.text = "";
        }
    }
    public override void Setup(Parameters parameters)
    {
        base.Setup(parameters); 
        AnimateGainedScore(parameters);
        
    }

    private int previousScore;
    private Coroutine scoreCoroutine;
    private void AnimateGainedScore(ScoreGainedController.Parameters parameters)
    {
        if(scoreCoroutine!= null)
            StopCoroutine(scoreCoroutine);
        scoreCoroutine = StartCoroutine(GainedScore_CR());
        
        IEnumerator GainedScore_CR()
        {
            var currentValue = 0;
            
            yield return DOTween.To(() => currentValue, x => currentValue = x, base.parameters.GainedScore, 0.2f)
                .SetEase(Ease.OutQuint).OnUpdate((() =>
                {
                    gainedText.text = Mathf.RoundToInt(currentValue).ToString();
                })).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
            gainedText.text = "";
            
            totalText.text = base.parameters.TotalScore.ToString();


            currentValue = previousScore;

            DOTween.To(() => currentValue, x => currentValue = x, base.parameters.TotalScore, 0.5f)
                .SetEase(Ease.OutQuint).OnUpdate((() =>
                {
                    totalText.text = Mathf.RoundToInt(currentValue).ToString();
                    previousScore = currentValue;
                }));
            
            previousScore = parameters.TotalScore;
        }
    } 
}
