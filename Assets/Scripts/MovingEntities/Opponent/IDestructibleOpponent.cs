using System;
using UnityEngine;

public interface IDestructibleOpponent
{
    public bool IsDead { get; set; }
    public Vector2 DestroyedPos { get; }
    public int Score { get; set; }
    
}