using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;
using Object = UnityEngine.Object;

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

    [Space] [SerializeField] private ProjectileSettings GlobalProjectileSettings;
    

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
        public LargeMeteor largeMeteor;
        public MediumMeteor mediumMeteor;
        public SmallMeteor smallMeteor;
        public SpaceShip spaceShip;
    }
    public override void InstallBindings()
    {
        Container.BindInstance(shipData);
        Container.BindInstance(LargeMeteor);
        Container.BindInstance(MediumMeteor);
        Container.BindInstance(SmallMeteor);
        Container.BindInstance(gameLevels);
        Container.BindInstance(gameAssets);
        Container.BindInstance(GlobalProjectileSettings);
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
    public float MeteorSize;
    public int Score;
    public AsteroidsInstaller.MovementEntityData MovementEntityData;
}
[Serializable]
public class SpaceShipData
{
    public ProjectileData ProjectileData;
    public LayerMask LayerMask;
    public float minSize, maxSize;
    public AsteroidsInstaller.MovementEntityData MovementEntityData;
    public int Score;
}
[Serializable]
public class LargeMeteorData : MeteorData<LargeMeteorData> {}
[Serializable]
public class MediumMeteorData : MeteorData<MediumMeteorData>{}
[Serializable]
public class SmallMeteorData : MeteorData<SmallMeteorData>{}

[Serializable]
public class ProjectileSettings
{
    public float projectileDuration = 1;
}