using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;

[CreateAssetMenu(fileName = "AsteroidsInstaller", menuName = "Installers/AsteroidsInstaller")]
public class AsteroidsInstaller : ScriptableObjectInstaller<AsteroidsInstaller>
{
    [SerializeField]private GameLevels gameLevels;

    [Space]
    [Header("Player Ship")]
    [SerializeField] private ShipData shipData;

    [Space] [Header("Enemies")] 
    [SerializeField] private LargeMeteorData LargeMeteor;
    [SerializeField] private MediumMeteorData MediumMeteor;
    [SerializeField] private SmallMeteorData SmallMeteor;
    [Space]
    [Space]
    [SerializeField] private GameAssets gameAssets;
    
    [Serializable]
    public class GameLevels
    {
        public LevelData[] Levels;
        [Serializable]
        public class LevelData
        {
            public int meteorsAmount;
            public int spaceShipsAmount;
        }
    }
    
    [Serializable]
    public class MovementEntityData
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
        [FormerlySerializedAs("largeOpponent")] [FormerlySerializedAs("LargeMeteor")] public LargeMeteor largeMeteor;
        [FormerlySerializedAs("mediumOpponent")] [FormerlySerializedAs("MediumMeteor")] public MediumMeteor mediumMeteor;
        [FormerlySerializedAs("smallOpponent")] [FormerlySerializedAs("SmallMeteor")] public SmallMeteor smallMeteor;
        
    }
    public override void InstallBindings()
    {
        Container.BindInstance(shipData);
        Container.BindInstance(LargeMeteor);
        Container.BindInstance(MediumMeteor);
        Container.BindInstance(SmallMeteor);
        Container.BindInstance(gameLevels);
        Container.BindInstance(gameAssets);
    }

}

[Serializable]
public class ShipData
{
    public ProjectileData ProjectileData;
    public AsteroidsInstaller.MovementEntityData MovementEntityData;
}

[Serializable]
public class MeteorData<T>
{
    public LayerMask TargetLayer;
    public float CollisionRadius;
    public int Score;
    public AsteroidsInstaller.MovementEntityData MovementEntityData;
}

[Serializable]
public class LargeMeteorData : MeteorData<LargeMeteorData> {}
[Serializable]
public class MediumMeteorData : MeteorData<MediumMeteorData>{}
[Serializable]
public class SmallMeteorData : MeteorData<SmallMeteorData>{}