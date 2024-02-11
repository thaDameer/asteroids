
using UnityEngine;
[CreateAssetMenu(fileName = "Movement Entity Data",menuName = "Entity Data/Movement Entity Data")]
public class MovementEntityData : ScriptableObject
{
    [field: SerializeField]public float AccelerationSpeed { get; private set; }
    [field: SerializeField]public float DecelerationSpeed { get; private set; }
    [field: SerializeField]public float MaxSpeed { get; private set; }
    [field: SerializeField]public float RotationSpeed { get; private set; }
}
