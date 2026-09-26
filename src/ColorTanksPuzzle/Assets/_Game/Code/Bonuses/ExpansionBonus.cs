using System;
using _Game.Code.Players;
using _Game.Code.Spawners;
using VContainer;

namespace _Game.Code.Bonuses
{
    public class ExpansionBonus : IBonus
    {
        private readonly WaitingAreaCellSpawner _waitingAreaCellSpawner;
        private readonly TankDispatcher _tankDispatcher;

        [Inject]
        public ExpansionBonus(WaitingAreaCellSpawner waitingAreaCellSpawner, TankDispatcher tankDispatcher)
        {
            _waitingAreaCellSpawner =
                waitingAreaCellSpawner ?? throw new ArgumentNullException(nameof(waitingAreaCellSpawner));

            _tankDispatcher = tankDispatcher ?? throw new ArgumentNullException(nameof(tankDispatcher));
        }

        public event Action<IBonus> Activated;
        
        public Bonus Bonus => Bonus.Expansion;

        public void Activate()
        {
            _waitingAreaCellSpawner.AddCell();
            _tankDispatcher.IncreaseMaxCellsCount();
        }
    }
}