using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class PlayerShip : ShootingMovementEntity,IDestructable
{

    [SerializeField]private Rigidbody2D rb2d;

    [SerializeField] private TrailRenderer _trailRenderer;
    
    [Inject]private IControllerService _inputControllerService;
    [Inject] private IObserverService _observerService;
    [Inject] private IShipLifeCounter _shipLifeCounter;
    [field: SerializeField]public SpriteRenderer SpriteRenderer { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public bool isShipDestroyed { get; private set; }
    private Vector3 currentDirection;

    private Vector2 velocity => ((Vector2)transform.position - lastPos) / Time.deltaTime;
    private Vector2 lastPos;
    [Inject]
    public void Construct(ShipData shipData)
    {
        Setup(shipData.ProjectileData);
        Setup(shipData.MovementEntityData);
    }
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
    
  
    void Update()
    {
        var currentPos = transform.position;
        UpdateRotation(_inputControllerService.InputAxis);
        
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

    protected override void UpdateMovement(bool accelerate)
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
    

    public void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;
        SpriteHelperClass.Flash(SpriteRenderer,Color.black,0.1f,2);
        Invoke("Died",0.1f);

    }

    public override void OnBoundaryTeleportStart()
    {
        base.OnBoundaryTeleportStart();
        _trailRenderer.Clear();
        _trailRenderer.gameObject.SetActive(false);
        _trailRenderer.emitting = false;
    }

    public override void OnBoundaryTeleportEnd()
    {
        base.OnBoundaryTeleportEnd();
        _trailRenderer.Clear();
        _trailRenderer.gameObject.SetActive(true);
        _trailRenderer.emitting = true;
    }

    public void Died()
    {
        if(isShipDestroyed) return;
        isShipDestroyed = true;
        _shipLifeCounter.ShipDestroyed();
        Destroy(gameObject);
    }
    

    public class Factory : PlaceholderFactory<PlayerShip> { }
    
}