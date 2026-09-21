namespace _Game.Code.Difficulty
{
    public class DeterministicRandom
    {
        private uint _state;

        public DeterministicRandom(int seed)
        {
            _state = (uint)seed;

            if (_state == 0)
                _state = 1;
        }

        public int NextInt(int minInclusive, int maxExclusive)
        {
            _state ^= _state << 13;
            _state ^= _state >> 17;
            _state ^= _state << 5;

            uint value = _state;

            return minInclusive + (int)(value % (uint)(maxExclusive - minInclusive));
        }
    }
}