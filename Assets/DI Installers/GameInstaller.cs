using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LevelManager _levelManager;
    public override void InstallBindings()
    {
        Container.Bind<IController>().To<KeyboardControls>().AsSingle();
     
        Container.Bind<ILevelManager>().To<LevelManager>().FromInstance(_levelManager);
    }
}