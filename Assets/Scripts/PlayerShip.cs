using UnityEngine;

public class PlayerShip : ShootingMovingEntity
{
    public override float AccelerationSpeed { get; set; }
    public override float ShootSpeed { get; set; }

    public void Construct(IPlayerShip playerShip)
    {
        AccelerationSpeed = playerShip.AccelerationSpeed;
        ShootSpeed = playerShip.ShootSpeed;
    }
    
    public override void UpdateMovement(Vector2 position)
    {
        
    }

    public override void UpdateRotation(Vector2 input)
    {

    }

  

    public override void Shoot(Vector2 dir)
    {
    
    }
}

public interface IPlayerShip
{
    public float AccelerationSpeed { get; set; }
    public float ShootSpeed { get; set; }
    
}
