using _Game.Code.Generators;
using VContainer.Unity;

namespace _Game.Code.Infrastructure
{
    public class GameEntryPoint : IInitializable
    {
        private readonly PixelArtGenerator _pixelArtGenerator;

        public GameEntryPoint(PixelArtGenerator pixelArtGenerator) => 
            _pixelArtGenerator = pixelArtGenerator;

        public void Initialize() => 
            _pixelArtGenerator.Generate();
    }
}