using GameJam.Behaviours;
using GameJam.Board;
using GameJam.Managers;
using UnityEngine;
using Zenject;

namespace GameJam
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Managers
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BoardGenerator>().FromComponentInHierarchy().AsSingle();
            
            // Tags
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BoardDestroyer>().FromComponentInHierarchy().AsSingle();
        }
    }
}