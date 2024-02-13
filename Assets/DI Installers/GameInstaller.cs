using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlayerShip _playerShip;
    [SerializeField] private LevelService levelService;
    [FormerlySerializedAs("assetsService")] [SerializeField] private BulletSpawner bulletSpawner;

    private AsteroidsInstaller.GameAssets settings;
    [Inject]
    public void Construct(AsteroidsInstaller.GameAssets settings)
    {
        this.settings = settings;
    }

    [Inject] private AsteroidsInstaller.PlayerMovementVariables _playerMovementVariables;
    public override void InstallBindings()
    {
        Container.Bind<IControllerService>().To<KeyboardControls>().AsSingle();
        Container.Bind<ILevelService>().To<LevelService>().FromInstance(levelService);
        Container.Bind<IBulletSpawner>().To<BulletSpawner>().FromInstance(bulletSpawner);

        
        Container.BindFactory<PlayerShip, PlayerShip.Factory>().FromComponentInNewPrefab(settings.PlayerShipPrefab);
        Container.BindFactory<ShootingMovementEntity,ShootingMovementEntity.Factory>().FromFactory<ShootingMovementEntity.Factory>();
        Container.BindFactory<Meteor, Meteor.LargeMeteor>().FromComponentInNewPrefab(settings.LargeMeteor);
        Container.BindFactory<Meteor, Meteor.MediumMeteor>().FromComponentInNewPrefab(settings.MediumMeteor);
        Container.BindFactory<Meteor, Meteor.SmallMeteor>().FromComponentInNewPrefab(settings.SmallMeteor);
        //Container.BindFactory<PlayerShip,PlayerShip.Factory>().FromFactory<PlayerShip.Factory>();
        // Container.BindFactory<Projectile, Projectile>()
        //     // This means that any time Asteroid.Factory.Create is called, it will instantiate
        //     // this prefab and then search it for the Asteroid component
        //     .FromComponentInNewPrefab(_settings.AsteroidPrefab)
        //     // We can also tell Zenject what to name the new gameobject here
        //     .WithGameObjectName("Asteroid")
    }
}