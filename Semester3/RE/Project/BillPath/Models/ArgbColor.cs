using System;

namespace BillPath.Models
{
    public struct ArgbColor
        : IEquatable<ArgbColor>
    {
        private readonly byte _alpha;
        private readonly byte _red;
        private readonly byte _green;
        private readonly byte _blue;

        public ArgbColor(byte alpha, byte red, byte green, byte blue)
        {
            _alpha = alpha;
            _red = red;
            _green = green;
            _blue = blue;
        }

        public byte Alpha
            => _alpha;
        public byte Red
            => _red;
        public byte Green
            => _green;
        public byte Blue
            => _blue;

        public bool Equals(ArgbColor other)
            => Alpha == other.Alpha
               && Red == other.Red
               && Green == other.Green
               && Blue == other.Blue;
        public override bool Equals(object obj)
        {
            var argbColor = obj as ArgbColor?;
            return argbColor != null && Equals(argbColor.Value);
        }
        public override int GetHashCode()
            => Alpha.GetHashCode()
               ^ Red.GetHashCode()
               ^ Green.GetHashCode()
               ^ Blue.GetHashCode();

        public static bool operator ==(ArgbColor left, ArgbColor right)
            => left.Equals(right);
        public static bool operator !=(ArgbColor left, ArgbColor right)
            => !left.Equals(right);

        public override string ToString()
            => $"{{{nameof(Alpha)} = {Alpha}, {nameof(Red)} = {Red}, {nameof(Green)} = {Green}, {nameof(Blue)} = {Blue}}}";
    }
}