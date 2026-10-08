using _Game.Code.Spawners;
using VContainer;
using VContainer.Unity;

namespace _Game.Code.Infrastructure
{
    public class GameEntryPoint : IInitializable
    {
        private readonly PixelArtSpawner _pixelArtSpawner;
        private readonly WaitingAreaCellSpawner _waitingAreaCellSpawner;

        [Inject]
        public GameEntryPoint(PixelArtSpawner pixelArtSpawner, WaitingAreaCellSpawner waitingAreaCellSpawner)
        {
            _pixelArtSpawner = pixelArtSpawner;
            _waitingAreaCellSpawner = waitingAreaCellSpawner;
        }

        public void Initialize()
        {
            _pixelArtSpawner.Generate();
            _waitingAreaCellSpawner.SpawnStartCells();
        }
    }
}