using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class PlayerShip : ShootingMovementEntity,IDestructable
{
    [SerializeField]private Rigidbody2D rb2d;
    private IControllerService _inputControllerService;

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

            if (_inputControllerService.Accelerate)
            {
                currentDirection = Vector2.Lerp(transform.up,velocity.normalized,0.9f).normalized;
            }

            return currentDirection;
        }
        set => currentDirection = value;
    }

    [Inject]
    public void Construct(IControllerService controllerService)
    {
        _inputControllerService = controllerService;
    }

    private Vector2 velocity => ((Vector2)transform.position - lastPos) / Time.deltaTime;

    private Vector2 lastPos;
    void Update()
    {
        var currentPos = transform.position;
        UpdateRotation(_inputControllerService.InputAxis);
        Debug.Log(velocity.magnitude);
        
        
        if (_inputControllerService.Shoot)
        {
            Shoot();
        }

        lastPos = currentPos;
    }

    private void FixedUpdate()
    {
        UpdateMovement(_inputControllerService.Accelerate);
    }

    public override void UpdateMovement(bool accelerate)
    {
        if (!accelerate)
        {
            if(rb2d.velocity.magnitude ==0)return;
            Vector2 oppositeDirection;
            oppositeDirection = -rb2d.velocity.normalized;
            rb2d.AddForce(oppositeDirection * DecelerationSpeed);
            return;
        }
        rb2d.AddForce(transform.up * AccelerationSpeed);
        rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, MaxSpeed);
    }
    
    public SpriteRenderer SpriteRenderer { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;
    }
    
    public class Factory : PlaceholderFactory<PlayerShip> { }
}