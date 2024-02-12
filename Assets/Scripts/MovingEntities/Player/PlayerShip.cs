using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class PlayerShip : ShootingMovementEntity,IDestructable
{

    private IController inputController;

    public void Setup()
    {
        MaxHealth = 1;
        CurrentHealth = 1;
    }

    private Vector3 currentDirection;
    public override Vector3 MovementDirection
    {
        get
        {

            if (inputController.Accelerate)
            {
                currentDirection = transform.up;
            }

            return currentDirection;
        }
        set => currentDirection = value;
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


    public SpriteRenderer SpriteRenderer { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public void TakeDamage(int dmg)
    {
        
    }
    
}


public interface IDestructable
{
    [field: SerializeField]public SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField]public abstract int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

   
    public void TakeDamage(int dmg);

    public void Died() { }
}