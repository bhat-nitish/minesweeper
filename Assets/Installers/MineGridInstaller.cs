using MineSweeper.Game.Minesweeper;
using Minesweeper.View;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MineGridInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMineSpriteRenderer>().To<MineSpriteRenderer>().AsSingle();
            Container.Bind<IMineGridPresenter>().To<MineGridPresenter>().AsSingle();
            Container.Bind<IMineGridModel>().To<MineGridModel>().AsSingle();
        }
    }
}