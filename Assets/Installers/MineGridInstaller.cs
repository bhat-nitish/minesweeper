using MineSweeper.Game.Minesweeper;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MineGridInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
           // Container.Bind<IMineGridView>().To<MineGridView>().AsSingle().NonLazy();
            Container.Bind<IMineGridPresenter>().To<MineGridPresenter>().AsSingle();
            Container.Bind<IMineGridModel>().To<MineGridModel>().AsSingle();
        }
    }
}