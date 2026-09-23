using System;
using _Game.Code.Spawners;
using VContainer;

namespace _Game.Code.Players
{
    public class BonusActivator : IDisposable
    {
        private readonly WaitingAreaCellSpawner _waitingAreaCellSpawner;
        private readonly InputReader _inputReader;

        [Inject]
        public BonusActivator(WaitingAreaCellSpawner waitingAreaCellSpawner, InputReader inputReader)
        {
            _waitingAreaCellSpawner = waitingAreaCellSpawner;
            _inputReader = inputReader;
            
            _inputReader.FirstBonusUsed += ActivateFirstBonus;
        }

        public void Dispose() => 
            _inputReader.FirstBonusUsed -= ActivateFirstBonus;

        private void ActivateFirstBonus() => 
            _waitingAreaCellSpawner.AddCell();
    }
}