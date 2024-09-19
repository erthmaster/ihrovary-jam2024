using Game.Managers;
using GameJam.Behaviours;
using UnityEngine;
using Zenject;

namespace GameJam
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
        }
    }
}