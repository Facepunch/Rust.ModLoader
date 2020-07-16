using System;

namespace Rust.ModLoader
{
    internal readonly struct RefreshItem : IEquatable<RefreshItem>
    {
        public string Name { get; }
        public string Path { get; }

        public RefreshItem(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public bool Equals(RefreshItem other)
        {
            return Name == other.Name && Path == other.Path;
        }

        public override bool Equals(object obj)
        {
            return obj is RefreshItem other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Path != null ? Path.GetHashCode() : 0);
            }
        }

        public static bool operator ==(RefreshItem left, RefreshItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RefreshItem left, RefreshItem right)
        {
            return !left.Equals(right);
        }
    }
}
