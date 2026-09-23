using _Game.Code.Generators;
using _Game.Code.Spawners;
using VContainer.Unity;

namespace _Game.Code.Infrastructure
{
    public class GameEntryPoint : IInitializable
    {
        private readonly PixelArtGenerator _pixelArtGenerator;
        private readonly WaitingAreaCellSpawner _waitingAreaCellSpawner;

        public GameEntryPoint(PixelArtGenerator pixelArtGenerator, WaitingAreaCellSpawner waitingAreaCellSpawner)
        {
            _pixelArtGenerator = pixelArtGenerator;
            _waitingAreaCellSpawner = waitingAreaCellSpawner;
        }

        public void Initialize()
        {
            _pixelArtGenerator.Generate();
            _waitingAreaCellSpawner.SpawnStartCells();
        }
    }
}