using UnityEngine;
using Zenject;

public class PlayerShip : ShootingMovingEntity
{
    private IController inputController;
    public override float AccelerationSpeed { get; set; }
    public override float ShootSpeed { get; set; }

    [Inject]
    public void Construct(IController controller)
    {
        inputController = controller;
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
    
    public float rotationSpeed = 120f;
    public float acceleration = 5f;
    public float deceleration = 2f;
    public float maxSpeed = 10f;

    private float currentSpeed = 0f;

    void Update()
    {
        HandleRotation();
        HandleAcceleration();
    }

    private void HandleRotation()
    {
        if(inputController.InputAxis.x != 0)
            transform.Rotate(Vector3.forward * -inputController.InputAxis.x * rotationSpeed * Time.deltaTime);
    }

    private void HandleAcceleration()
    {
        if (inputController.Accelerate)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        }
        else
        {
         
            currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, 0f);
        }

    
        Vector3 forwardDirection = transform.up;
        transform.position += forwardDirection * currentSpeed * Time.deltaTime;
    }
}


public interface IPlayerShip
{
    public float AccelerationSpeed { get; set; }
    public float ShootSpeed { get; set; }
    
}
