using System;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameInstaller : MonoInstaller
{
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoringService scoringService;
    [SerializeField] private ObserverService observerService;
    [SerializeField] private LevelService levelService;
    [SerializeField] private EnemyService enemyService;
    [SerializeField] private ProjectileSpawner projectileSpawner;

    private AsteroidsInstaller.GameAssets settings;
    [Inject]
    public void Construct(AsteroidsInstaller.GameAssets settings)
    {
        this.settings = settings;
    }

    public override void InstallBindings()
    {
        Container.Bind<IObserverService>().To<ObserverService>().FromInstance(observerService);
        Container.Bind<IScoringService>().To<ScoringService>().FromInstance(scoringService);
        Container.Bind<IControllerService>().To<KeyboardControls>().AsSingle();
        Container.Bind<ILevelService>().To<LevelService>().FromInstance(levelService);
        Container.Bind<IProjectileSpawner>().To<ProjectileSpawner>().FromInstance(projectileSpawner);
        Container.Bind<Camera>().FromMethod(GetMainCamera).AsSingle();
        Container.Bind<IShipLifeCounter>().To<GameManager>().FromInstance(gameManager);
        Container.Bind<IEnemyService>().To<EnemyService>().FromInstance(enemyService);
        
        Container.BindFactory<PlayerShip, PlayerShip.Factory>().FromComponentInNewPrefab(settings.PlayerShipPrefab);
        Container.BindFactory<LargeMeteor, LargeMeteor.Factory>().FromComponentInNewPrefab(settings.largeMeteor);
        Container.BindFactory<MediumMeteor, MediumMeteor.Factory>().FromComponentInNewPrefab(settings.mediumMeteor);
        Container.BindFactory<SmallMeteor, SmallMeteor.Factory>().FromComponentInNewPrefab(settings.smallMeteor);
        Container.BindFactory<SpaceShip, SpaceShip.Factory>().FromComponentInNewPrefab(settings.spaceShip);
        
    }
    private Camera GetMainCamera(InjectContext context)
    {
        return Camera.main;
    }
}