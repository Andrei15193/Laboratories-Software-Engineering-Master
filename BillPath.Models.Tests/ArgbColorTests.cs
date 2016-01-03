using System.IO;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public class ArgbColorTests
    {
        [TestMethod]
        public void TestConstructorAssignsArgbValuesAccordingly()
        {
            for (byte start = 0; start < byte.MaxValue / 4; start += 4)
                _AssertCreation(start, (byte)(start + 1), (byte)(start + 2), (byte)(start + 3));
        }

        private void _AssertCreation(byte alpha, byte red, byte green, byte blue)
        {
            var argbColor = new ArgbColor(alpha, red, green, blue);

            Assert.AreEqual(alpha, argbColor.Alpha);
            Assert.AreEqual(red, argbColor.Red);
            Assert.AreEqual(green, argbColor.Green);
            Assert.AreEqual(blue, argbColor.Blue);
        }

        [TestMethod]
        public void TestColorIsEqualToItself()
        {
            var argbColor = new ArgbColor(2, 3, 4, 5);

            Assert.AreEqual(argbColor, argbColor);
            Assert.AreEqual(argbColor.GetHashCode(), argbColor.GetHashCode());
        }
        [TestMethod]
        public void TestTwoColorsConstructedWithTheSameParameterValuesAreEqual()
        {
            byte alpha = 3;
            byte red = 4;
            byte green = 5;
            byte blue = 6;
            var argbColor1 = new ArgbColor(alpha, red, green, blue);
            var argbColor2 = new ArgbColor(alpha, red, green, blue);

            Assert.AreEqual(argbColor1, argbColor2);
            Assert.IsTrue(argbColor1.Equals(argbColor2));

            Assert.IsTrue(argbColor1 == argbColor2);
            Assert.IsFalse(argbColor1 != argbColor2);

            Assert.AreEqual(argbColor1.GetHashCode(), argbColor2.GetHashCode());
        }
        [TestMethod]
        public void TestTwoColorsHavingAlphaDifferentAreNotEqual()
        {
            byte red = 5;
            byte green = 6;
            byte blue = 7;
            var argbColor1 = new ArgbColor(1, red, green, blue);
            var argbColor2 = new ArgbColor(2, red, green, blue);

            Assert.AreNotEqual(argbColor1, argbColor2);
            Assert.IsFalse(argbColor1.Equals(argbColor2));

            Assert.IsFalse(argbColor1 == argbColor2);
            Assert.IsTrue(argbColor1 != argbColor2);
        }
        [TestMethod]
        public void TestTwoColorsHavingRedDifferentAreNotEqual()
        {
            byte alpha = 6;
            byte green = 7;
            byte blue = 8;
            var argbColor1 = new ArgbColor(alpha, 1, green, blue);
            var argbColor2 = new ArgbColor(alpha, 2, green, blue);

            Assert.AreNotEqual(argbColor1, argbColor2);
            Assert.IsTrue(argbColor1 != argbColor2);
        }
        [TestMethod]
        public void TestTwoColorsHavingGreenDifferentAreNotEqual()
        {
            byte alpha = 7;
            byte red = 8;
            byte blue = 9;
            var argbColor1 = new ArgbColor(alpha, red, 1, blue);
            var argbColor2 = new ArgbColor(alpha, red, 2, blue);

            Assert.AreNotEqual(argbColor1, argbColor2);
            Assert.IsTrue(argbColor1 != argbColor2);
        }
        [TestMethod]
        public void TestTwoColorsHavingBlueDifferentAreNotEqual()
        {
            byte alpha = 8;
            byte red = 9;
            byte green = 10;
            var argbColor1 = new ArgbColor(alpha, red, green, 1);
            var argbColor2 = new ArgbColor(alpha, red, green, 2);

            Assert.AreNotEqual(argbColor1, argbColor2);
            Assert.IsTrue(argbColor1 != argbColor2);
        }
    }
}