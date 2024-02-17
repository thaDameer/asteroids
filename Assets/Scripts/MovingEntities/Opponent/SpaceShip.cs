using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpaceShip : ShootingMovementEntity, IDestructibleOpponent
{
    public override Vector3 MovementDirection { get; set; }
    private Queue<Vector2> positions;
    public Vector2 DestroyedPos { get; set; }
    public int Score { get; set; }


    public void Setup(SpaceShipData spaceShipData)
    {
        Setup(spaceShipData.MovementEntityData);
        Setup(spaceShipData.ProjectileData);
        Score = spaceShipData.Score;
    }

    public void SetMovementPositions(List<Vector2> positions)
    {
        
    }
    
    public class Factory : PlaceholderFactory<SpaceShip> { }

}
