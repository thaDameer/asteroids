using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "AsteroidsInstaller", menuName = "Installers/AsteroidsInstaller")]
public class AsteroidsInstaller : ScriptableObjectInstaller<AsteroidsInstaller>
{
    [SerializeField] private PlayerMovementVariables playerMovementSettings;
    [SerializeField] private GameAssets gameAssets;
    
    
    [Serializable]
    public class PlayerMovementVariables
    {
        [field: SerializeField]public float AccelerationSpeed { get; private set; }
        [field: SerializeField]public float DecelerationSpeed { get; private set; }
        [field: SerializeField]public float MaxSpeed { get; private set; }
        [field: SerializeField]public float RotationSpeed { get; private set; }
    }
    
    [Serializable]
    public class GameAssets
    {
        public PlayerShip PlayerShipPrefab;
        public Meteor LargeMeteor;
        public Meteor MediumMeteor;
        public Meteor SmallMeteor;
        
    }
    public override void InstallBindings()
    {
        Container.BindInstance(playerMovementSettings);
        Container.BindInstance(gameAssets);
    }
}