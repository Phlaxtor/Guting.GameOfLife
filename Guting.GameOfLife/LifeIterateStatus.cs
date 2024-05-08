namespace Guting.GameOfLife
{
    public readonly struct LifeIterateStatus
    {
        private readonly IReadOnlyCollection<Life> _added;
        private readonly IReadOnlyCollection<Life> _existing;
        private readonly ulong _iterations;
        private readonly IReadOnlyCollection<Life> _removed;

        public LifeIterateStatus(IReadOnlyCollection<Life> added, IReadOnlyCollection<Life> existing, IReadOnlyCollection<Life> removed, ulong iterations)
        {
            _added = added;
            _removed = removed;
            _existing = existing;
            _iterations = iterations;
        }

        public IReadOnlyCollection<Life> Added => _added;
        public IReadOnlyCollection<Life> Existing => _existing;
        public ulong Iterations => _iterations;
        public IReadOnlyCollection<Life> Removed => _removed;
    }
}