using System;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public struct ArgbColor
        : IEquatable<ArgbColor>
    {
        [DataMember(Name = nameof(Alpha))]
        private readonly byte _alpha;
        [DataMember(Name = nameof(Red))]
        private readonly byte _red;
        [DataMember(Name = nameof(Green))]
        private readonly byte _green;
        [DataMember(Name = nameof(Blue))]
        private readonly byte _blue;

        public ArgbColor(byte alpha, byte red, byte green, byte blue)
        {
            _alpha = alpha;
            _red = red;
            _green = green;
            _blue = blue;
        }

        public byte Alpha
        {
            get
            {
                return _alpha;
            }
        }
        public byte Red
        {
            get
            {
                return _red;
            }
        }
        public byte Green
        {
            get
            {
                return _green;
            }
        }
        public byte Blue
        {
            get
            {
                return _blue;
            }
        }

        public static bool operator ==(ArgbColor left, ArgbColor right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ArgbColor left, ArgbColor right)
        {
            return !left.Equals(right);
        }
        public bool Equals(ArgbColor other)
        {
            return Alpha == other.Alpha
                   && Red == other.Red
                   && Green == other.Green
                   && Blue == other.Blue;
        }
        public override bool Equals(object obj)
        {
            var argbColor = obj as ArgbColor?;
            return argbColor != null && Equals(argbColor.Value);
        }
        public override int GetHashCode()
        {
            return Alpha.GetHashCode()
                   ^ Red.GetHashCode()
                   ^ Green.GetHashCode()
                   ^ Blue.GetHashCode();
        }

        public override string ToString()
        {
            return $"{{{nameof(Alpha)} = {Alpha}, {nameof(Red)} = {Red}, {nameof(Green)} = {Green}, {nameof(Blue)} = {Blue}}}";
        }
    }
}