using System;
using UnityEngine;

public interface IDestructibleOpponent
{
  
    public Vector2 DestroyedPos { get; }
    public int Score { get; set; }
    
}