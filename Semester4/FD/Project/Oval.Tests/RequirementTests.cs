using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oval.Tests
{
    [TestClass]
    public class RequirementTests
    {
        [TestMethod]
        public void TestCreateRequirementWithJustText()
        {
            var expectedRequirementText = "test";
            var actualRequirement = new Requirement(expectedRequirementText);

            Assert.AreEqual(
                expectedRequirementText,
                actualRequirement.Text,
                ignoreCase: false);
        }

        [TestMethod]
        public void TestCreateRequirementWithTextAndMemberNames()
        {
            var expectedRequirementText = "test";
            var expectedRequirementMemberNames = new[] { "member1", "member2" };
            var actualRequirement = new Requirement(expectedRequirementText, expectedRequirementMemberNames);

            Assert.AreEqual(
                expectedRequirementText,
                actualRequirement.Text,
                ignoreCase: false);
            Assert.IsTrue(expectedRequirementMemberNames.SequenceEqual(actualRequirement.MemberNames));
        }

        [TestMethod]
        public void TestCreateRequirementWithTextAndNullForMemberNames()
        {
            var expectedRequirementText = "test";
            var actualRequirement = new Requirement(expectedRequirementText, null);

            Assert.AreEqual(
                expectedRequirementText,
                actualRequirement.Text,
                ignoreCase: false);
            Assert.IsFalse(actualRequirement.MemberNames.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCreateRequirementWithNullText()
        {
            new Requirement(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateRequirementWithEmptyText()
        {
            new Requirement(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateRequirementWithWhiteSpaceText()
        {
            new Requirement(" ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateRequirementWithTextAndANullMemberName()
        {
            new Requirement(
                "test",
                new string[] { null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateRequirementWithTextAndAnEmptyMemberName()
        {
            new Requirement(
                "test",
                new[] { string.Empty });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreateRequirementWithTextAndAWhiteSpaceMemberName()
        {
            new Requirement(
                "test",
                new[] { " " });
        }
    }
}