namespace Guting.GameOfLife
{
    public readonly struct LifeStatusContainer
    {
        private readonly IReadOnlySet<long> _alive;
        private readonly HashSet<long> _deadNeighbours;
        private readonly HashSet<Life> _forDeletion;

        public LifeStatusContainer(IEnumerable<long> alive)
        {
            _alive = new HashSet<long>(alive);
            int capacity = _alive.Count;
            _forDeletion = new HashSet<Life>(capacity);
            _deadNeighbours = new HashSet<long>(capacity);
        }

        public IReadOnlyCollection<long> DeadNeighbours => _deadNeighbours;

        public IReadOnlyCollection<Life> ForDeletion => _forDeletion;

        public void Add(long id) => _deadNeighbours.Add(id);

        public bool IsAlive(long id) => _alive.Contains(id);

        public void Remove(Life id) => _forDeletion.Add(id);
    }
}