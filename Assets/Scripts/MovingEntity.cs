using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class MovingEntity : MonoBehaviour
{
    public abstract float AccelerationSpeed { get; set; }
    public abstract void UpdateMovement(Vector2 position);
    public abstract void UpdateRotation(Vector2 input);
}

public abstract class ShootingMovingEntity : MovingEntity
{
    public abstract float ShootSpeed { get; set; }
    public abstract void Shoot(Vector2 dir);
}