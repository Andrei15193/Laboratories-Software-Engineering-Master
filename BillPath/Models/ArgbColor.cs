using System;
using System.Runtime.Serialization;

namespace BillPath.Models
{
    [DataContract]
    public struct ArgbColor
        : IEquatable<ArgbColor>
    {
        public ArgbColor(byte alpha, byte red, byte green, byte blue)
        {
            Alpha = alpha;
            Red = red;
            Green = green;
            Blue = blue;
        }

        [DataMember]
        public byte Alpha
        {
            get;
            private set;
        }
        [DataMember]
        public byte Red
        {
            get;
            private set;
        }
        [DataMember]
        public byte Green
        {
            get;
            private set;
        }
        [DataMember]
        public byte Blue
        {
            get;
            private set;
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