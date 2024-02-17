
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovementEntity : MonoBehaviour
{
    public float AccelerationSpeed { get; set; }
    public float MaxSpeed { get; set; }
    public float DecelerationSpeed { get; set; }
    public float RotationSpeed { get; set; }
    [field: SerializeField]public Transform RotationTarget { get; set; }
    
    public abstract Vector3 MovementDirection { get; set; }
    protected float currentSpeed;


    protected void Setup(AsteroidsInstaller.MovementEntityData movementEntityData)
    {
        AccelerationSpeed = movementEntityData.AccelerationSpeed;
        MaxSpeed = movementEntityData.MaxSpeed;
        DecelerationSpeed = movementEntityData.DecelerationSpeed;
        RotationSpeed = movementEntityData.RotationSpeed;
    }

    public virtual void OnBoundaryTeleportStart()
    {
        
    }
    public virtual void OnBoundaryTeleportEnd()
    {
        
    }
    protected virtual void UpdateMovement(bool accelerate)
    {
        if (accelerate)
        {
            currentSpeed = Mathf.Min(currentSpeed + AccelerationSpeed * Time.deltaTime, MaxSpeed);
        }
        else
        {
            currentSpeed = Mathf.Max(currentSpeed - DecelerationSpeed * Time.deltaTime, 0f);
        }
        
        transform.position += MovementDirection * currentSpeed * Time.deltaTime;
    }

    protected virtual void UpdateRotation(Vector2 input)
    {
        RotationTarget.Rotate(Vector3.forward * -input.x * RotationSpeed * Time.deltaTime);
    }
}