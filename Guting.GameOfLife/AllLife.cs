using System.Text;

namespace Guting.GameOfLife
{
    public readonly struct AllLife
    {
        private readonly IDictionary<long, Life> _allLife;
        private readonly Counter _counter;

        public AllLife()
        {
            _counter = new Counter(0);
            _allLife = new Dictionary<long, Life>();
        }

        public void Add(int x, int y)
        {
            Add(new Life(x, y));
        }

        public LifeIterateStatus Iterate()
        {
            _counter.Increase();
            var status = GetLifeStatus();
            Delete(status.ForDeletion);
            var added = TryAddDead(status.DeadNeighbours, status);
            var existing = new HashSet<Life>(_allLife.Values);
            return new LifeIterateStatus(added, existing, status.ForDeletion, _counter.Count);
        }

        public async Task Open(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await ReadFrom(file, ';', true);
                await file.FlushAsync();
            }
        }

        public async Task ReadFrom(Stream stream, char delimiter, bool hasHeader)
        {
            using (var reader = new StreamReader(stream, encoding: Encoding.UTF8, leaveOpen: true))
            {
                string? header = null;
                if (hasHeader)
                {
                    header = await reader.ReadLineAsync();
                }
                while (reader.EndOfStream == false)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(line)) continue;
                    var parts = line.Split(delimiter);
                    if (parts.Length == 1) continue;
                    int x = int.Parse(parts[0]);
                    int y = int.Parse(parts[1]);
                    Add(x, y);
                }
            }
        }

        public async Task Run(Func<LifeIterateStatus, Task<bool>> display, CancellationToken cancellationToken)
        {
            var continueRun = true;
            while (continueRun && cancellationToken.IsCancellationRequested == false)
            {
                var status = Iterate();
                continueRun = await display(status);
            }
        }

        public async Task Save(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                await WriteTo(file, ';', true);
                await file.FlushAsync();
            }
        }

        public async Task WriteTo(Stream stream, char delimiter, bool addHeader)
        {
            using (var writer = new StreamWriter(stream, encoding: Encoding.UTF8, leaveOpen: true))
            {
                if (addHeader)
                {
                    await writer.WriteAsync('X');
                    await writer.WriteAsync(delimiter);
                    await writer.WriteAsync('Y');
                    await writer.WriteLineAsync();
                }
                foreach (var item in _allLife.Values)
                {
                    await writer.WriteLineAsync(item.ToString(delimiter));
                }
            }
        }

        private void Add(Life life)
        {
            _allLife[life.Key] = life;
        }

        private void Add(long key)
        {
            Add(new Life(key));
        }

        private void Delete(long key)
        {
            _allLife.Remove(key);
        }

        private void Delete(IEnumerable<Life> keys)
        {
            foreach (var key in keys)
            {
                Delete(key);
            }
        }

        private LifeStatusContainer GetLifeStatus()
        {
            var status = new LifeStatusContainer(_allLife.Keys);
            foreach (var life in _allLife.Values)
            {
                var count = life.CountAliveNeighbours(status);
                if (count < 2) status.Remove(life);
                if (count > 3) status.Remove(life);
                life.AddDeadNeighbours(status);
            }
            return status;
        }

        private Life? TryAddDead(long key, LifeStatusContainer status)
        {
            var life = new Life(key);
            var count = life.CountAliveNeighbours(status);
            if (count == 3)
            {
                Add(life);
                return life;
            }
            return null;
        }

        private IReadOnlyCollection<Life> TryAddDead(IEnumerable<long> keys, LifeStatusContainer status)
        {
            var result = new List<Life>();
            foreach (var key in keys)
            {
                var life = TryAddDead(key, status);
                if (life.HasValue) result.Add(life.Value);
            }
            return result;
        }
    }
}