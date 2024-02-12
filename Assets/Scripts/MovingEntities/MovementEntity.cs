
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovementEntity : MonoBehaviour
{
    [field: SerializeField]public MovementEntityData MovementEntityData { get; set; }
    public float AccelerationSpeed => MovementEntityData.AccelerationSpeed;
    public float MaxSpeed => MovementEntityData.MaxSpeed;
    public float DecelerationSpeed => MovementEntityData.DecelerationSpeed;
    [field: SerializeField]public Transform RotationTarget { get; set; }
    
    public float RotationSpeed => MovementEntityData.RotationSpeed;
    public abstract Vector3 MovementDirection { get; set; }
    protected float currentSpeed;
    
    
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