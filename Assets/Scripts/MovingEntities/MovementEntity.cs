
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


    public void Setup(AsteroidsInstaller.MovementEntityData movementEntityData)
    {
        AccelerationSpeed = movementEntityData.AccelerationSpeed;
        MaxSpeed = movementEntityData.MaxSpeed;
        DecelerationSpeed = movementEntityData.DecelerationSpeed;
        RotationSpeed = movementEntityData.RotationSpeed;
    }

    // public void SetupEntity(AsteroidsInstaller.MovementEntityData data)
    // {
    //     AccelerationSpeed = data.AccelerationSpeed
    // }
    public virtual void UpdateMovement(bool accelerate)
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

    public virtual void UpdateRotation(Vector2 input)
    {
        RotationTarget.Rotate(Vector3.forward * -input.x * RotationSpeed * Time.deltaTime);
    }
}