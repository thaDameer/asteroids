using UnityEngine;
using Zenject;

public class PlayerShip : ShootingMovementEntity
{
   
    private IController inputController;
    

    [Inject]
    public void Construct(IController controller)
    {
        inputController = controller;
    }
    
    void Update()
    {
        HandleRotation(inputController.InputAxis);
        HandleMovement(inputController.Accelerate);
        if (inputController.Shoot)
        {
            Shoot(transform.up);
        }
    }

    

    

}


public interface IPlayerShip
{
    public float AccelerationSpeed { get; set; }
    public float ShootSpeed { get; set; }
    
}

public interface IDamagable
{
    public int MaxHealth { get; set; }
    public void TakeDamage(int dmg);

    public void Died() { }
}