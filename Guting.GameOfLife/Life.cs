using System.Diagnostics.CodeAnalysis;

namespace Guting.GameOfLife
{
    public readonly struct Life : IComparable<Life>, IEquatable<Life>
    {
        private const int s = 31;
        private readonly long _key;
        private readonly long _ll;
        private readonly long _lm;
        private readonly long _lr;
        private readonly long _ml;
        private readonly long _mr;
        private readonly long _ul;
        private readonly long _um;
        private readonly long _ur;
        private readonly int _x;
        private readonly int _y;

        public Life(int x, int y)
        {
            _x = x;
            _y = y;
            _key = GetKey(x, y);
            _ul = GetKey(x - 1, y + 1);
            _um = GetKey(x, y + 1);
            _ur = GetKey(x + 1, y + 1);
            _ml = GetKey(x - 1, y);
            _mr = GetKey(x + 1, y);
            _ll = GetKey(x - 1, y - 1);
            _lm = GetKey(x, y - 1);
            _lr = GetKey(x + 1, y - 1);
        }

        public Life(long key)
        {
            int x = GetX(key);
            int y = GetY(key);
            _x = x;
            _y = y;
            _key = key;
            _ul = GetKey(x - 1, y + 1);
            _um = GetKey(x, y + 1);
            _ur = GetKey(x + 1, y + 1);
            _ml = GetKey(x - 1, y);
            _mr = GetKey(x + 1, y);
            _ll = GetKey(x - 1, y - 1);
            _lm = GetKey(x, y - 1);
            _lr = GetKey(x + 1, y - 1);
        }

        public long Key => _key;
        public int X => _x;
        public int Y => _y;

        public static implicit operator Life(long key) => new Life(key);

        public static implicit operator long(Life life) => life.Key;

        public void AddDeadNeighbours(LifeStatusContainer status)
        {
            if (status.IsAlive(_ul) == false) status.Add(_ul);
            if (status.IsAlive(_um) == false) status.Add(_um);
            if (status.IsAlive(_ur) == false) status.Add(_ur);
            if (status.IsAlive(_ml) == false) status.Add(_ml);
            if (status.IsAlive(_mr) == false) status.Add(_mr);
            if (status.IsAlive(_ll) == false) status.Add(_ll);
            if (status.IsAlive(_lm) == false) status.Add(_lm);
            if (status.IsAlive(_lr) == false) status.Add(_lr);
        }

        public int CompareTo(Life other)
        {
            return _key.CompareTo(other._key);
        }

        public int CountAliveNeighbours(LifeStatusContainer status)
        {
            int count = 0;
            if (status.IsAlive(_ul)) count++;
            if (status.IsAlive(_um)) count++;
            if (status.IsAlive(_ur)) count++;
            if (status.IsAlive(_ml)) count++;
            if (status.IsAlive(_mr)) count++;
            if (status.IsAlive(_ll)) count++;
            if (status.IsAlive(_lm)) count++;
            if (status.IsAlive(_lr)) count++;
            return count;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Life other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(Life other)
        {
            return _key == other._key;
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }

        public override string ToString()
        {
            return $"X: {_x} Y: {_y}";
        }

        public string ToString(char delimiter, [StringSyntax("NumericFormat")] string? format)
        {
            if (string.IsNullOrEmpty(format))
            {
                return ToString(delimiter);
            }
            return _x.ToString(format) + delimiter + _y.ToString(format);
        }

        public string ToString(char delimiter)
        {
            return _x.ToString() + delimiter + _y.ToString();
        }

        public string ToString(string delimiter)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));
            return _x.ToString() + delimiter + _y.ToString();
        }

        public string ToString(string delimiter, [StringSyntax("NumericFormat")] string? format)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(delimiter, nameof(delimiter));
            if (string.IsNullOrEmpty(format))
            {
                return ToString(delimiter);
            }
            return _x.ToString(format) + delimiter + _y.ToString(format);
        }

        private static long GetKey(long x, long y)
        {
            return x | (y << s);
        }

        private static int GetX(long key)
        {
            return (int)(key & int.MaxValue);
        }

        private static int GetY(long key)
        {
            return (int)(key >> s);
        }
    }
}