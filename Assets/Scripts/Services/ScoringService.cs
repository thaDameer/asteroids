using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringService : MonoBehaviour,IScoringService
{
    public void ResetScore()
    {
        TotalScore = 0;
    }

    public int TotalScore { get; set; }

    public void UpdateGainedScore(int gained)
    {
        TotalScore += gained;
        OnUpdateGainedScore?.Invoke(gained);
    }

    public Action<int> OnUpdateGainedScore { get; set; }
}

public interface IScoringService
{
    public void ResetScore();
    public int TotalScore { get; set; }
    public void UpdateGainedScore(int gained);
    public Action<int> OnUpdateGainedScore { get; set; }
} 