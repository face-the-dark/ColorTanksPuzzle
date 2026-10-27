using _Game.Code.Players;
using _Game.Code.Spawners;
using _Game.Code.Utilities;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure
{
    public class GameEntryPoint : IInitializable
    {
        private readonly DeterministicRandom _random;
        private readonly PixelArtSpawner _pixelArtSpawner;
        private readonly WaitingAreaCellSpawner _waitingAreaCellSpawner;
        private readonly TankDispatcher _tankDispatcher;

        private readonly GameRunner _gameRunner;

        [Inject]
        public GameEntryPoint
        (
            DeterministicRandom random,
            PixelArtSpawner pixelArtSpawner,
            WaitingAreaCellSpawner waitingAreaCellSpawner,
            TankDispatcher tankDispatcher,
            GameRunner gameRunner
        )
        {
            _random = random;
            _pixelArtSpawner = pixelArtSpawner;
            _waitingAreaCellSpawner = waitingAreaCellSpawner;
            _tankDispatcher = tankDispatcher;

            _gameRunner = gameRunner;
        }

        public void Initialize()
        {
            _gameRunner.Initialize();

            _random.Initialize();
            _pixelArtSpawner.Generate();
            _waitingAreaCellSpawner.SpawnStartCells();
            _tankDispatcher.Initialize();
        }
    }
}