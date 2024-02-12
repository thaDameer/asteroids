using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class PlayerShip : ShootingMovementEntity
{

    private IController inputController;
    public override Vector3 MovementDirection
    {
        get => transform.up;
        set => transform.up = value;
    }

    [Inject]
    public void Construct(IController controller)
    {
        inputController = controller;
    }
    
    void Update()
    {
        UpdateRotation(inputController.InputAxis);
        UpdateMovement(inputController.Accelerate);
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

public interface IDestructable
{
    [field: SerializeField]public SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField]public abstract int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

   
    public void TakeDamage(int dmg);

    public void Died() { }
}