
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovementEntity : MonoBehaviour
{
    [field: SerializeField]public MovementEntityData MovementEntityData { get; set; }
    protected Rigidbody2D rb2D => GetComponent<Rigidbody2D>();
    public float AccelerationSpeed => MovementEntityData.AccelerationSpeed;
    public float MaxSpeed => MovementEntityData.MaxSpeed;
    public float DecelerationSpeed => MovementEntityData.DecelerationSpeed;
    public float RotationSpeed => MovementEntityData.RotationSpeed;

    protected float currentSpeed;
    public virtual void HandleMovement(bool accelerate)
    {
        if (accelerate)
        {
            currentSpeed = Mathf.Min(currentSpeed + AccelerationSpeed * Time.deltaTime, MaxSpeed);
        }
        else
        {
            currentSpeed = Mathf.Max(currentSpeed - DecelerationSpeed * Time.deltaTime, 0f);
        }

    
        Vector3 forwardDirection = transform.up;
        transform.position += forwardDirection * currentSpeed * Time.deltaTime;
    }

    public virtual void HandleRotation(Vector2 input)
    {
        transform.Rotate(Vector3.forward * -input.x * RotationSpeed * Time.deltaTime);
    }
}