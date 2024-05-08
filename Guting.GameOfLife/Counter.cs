namespace Guting.GameOfLife
{
    public struct Counter
    {
        private ulong _value;

        public Counter(ulong value = 0)
        {
            _value = value;
        }

        public ulong Count => _value;

        public void Decrease() => _value--;

        public void Increase() => _value++;
    }
}