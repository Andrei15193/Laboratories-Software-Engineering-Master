using System;
using System.ComponentModel;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class ModelContextTests
    {
        private sealed class ModelContextMock
            : ModelContext
        {
            public ModelContextMock(object model)
                : base(model)
            {
            }

            new public object Model
            {
                get
                {
                    return base.Model;
                }
                set
                {
                    base.Model = value;
                }
            }
        }

        private sealed class ModelMock
        {
            public object Property
            {
                get;
                set;
            }
            public string StringProperty
            {
                get;
                set;
            }
            public object GetOnlyProperty
            {
                get
                {
                    return new object();
                }
            }
            public object SetOnlyProperty
            {
                set
                {
                }
            }
        }

        [TestMethod]
        public void TestModelCannotBeNull()
            => Assert.ThrowsException<ArgumentNullException>(
                () => new ModelContext(null));

        [TestMethod]
        public void TestGettingModelPropertyValueThroughContext()
        {
            var expectedPropertyValue = new object();
            var modelMock =
                new ModelMock
                {
                    Property = expectedPropertyValue
                };
            dynamic modelContext = new ModelContext(modelMock);

            object actualPropertyValue = modelContext.Property;

            Assert.AreSame(expectedPropertyValue, actualPropertyValue);
        }

        [TestMethod]
        public void TestTryingToGetTheValueOfAPropertyNotDefinedOnTheModelThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelContext(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.PropertyThatDoesNotExist);
        }

        [TestMethod]
        public void TestTryingToGetTheValueOfAPropertyThatIsSetOnlyThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelContext(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.SetOnlyProperty);
        }

        [TestMethod]
        public void TestTryingToGetAStringPropertyIntoAnObjectDoesNotThrowException()
        {
            var expectedPropertyValue = "test property value";
            var modelMock =
                new ModelMock
                {
                    StringProperty = expectedPropertyValue
                };
            dynamic modelContext = new ModelContext(modelMock);

            object actualPropertyValue = modelContext.StringProperty;

            Assert.AreSame(expectedPropertyValue, actualPropertyValue);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyThroughTheContextAlsoChangesTheModel()
        {
            var propertyValue = new object();
            var modelMock =
                new ModelMock
                {
                    Property = null
                };
            dynamic modelContext = new ModelContext(modelMock);

            modelContext.Property = propertyValue;

            Assert.AreSame(propertyValue, modelMock.Property);
        }

        [TestMethod]
        public void TestSettingAStringValueOnAnObjectProperty()
        {
            var propertyValue = "test property value";
            var modelMock =
                new ModelMock
                {
                    Property = null
                };
            dynamic modelContext = new ModelContext(modelMock);

            modelContext.Property = propertyValue;

            Assert.AreSame(propertyValue, modelMock.Property);
        }

        [TestMethod]
        public void TestTryingToSetTheValueOfAPropertyNotDefinedOnTheModelThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelContext(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.PropertyThatDoesNotExist = null);
        }

        [TestMethod]
        public void TestTryingToSetTheValueOfAPropertyThatIsGetOnlyThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelContext(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.GetOnlyProperty = null);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChanged()
        {
            var raiseCount = 0;
            dynamic modelContext = new ModelContext(new ModelMock());
            ((INotifyPropertyChanged)modelContext).PropertyChanged += (sender, e) => raiseCount++;

            modelContext.Property = new object();

            Assert.AreEqual(1, raiseCount);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChangedWithTheContextAsSender()
        {
            object actualSender = null;
            dynamic modelContext = new ModelContext(new ModelMock());
            ((INotifyPropertyChanged)modelContext).PropertyChanged += (sender, e) => actualSender = sender;

            modelContext.Property = new object();

            Assert.AreSame(modelContext, actualSender);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChangedWithTheCorrespondingPropertyName()
        {
            string actualPropertyName = null;
            dynamic modelContext = new ModelContext(new ModelMock());
            ((INotifyPropertyChanged)modelContext).PropertyChanged += (sender, e) => actualPropertyName = e.PropertyName;

            modelContext.Property = new object();

            Assert.AreEqual("Property", actualPropertyName);
        }

        [TestMethod]
        public void TestPropertiesOnContextAreCaseSensitive()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelContext(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.property = null);
        }

        [TestMethod]
        public void TestModelPropertyReturnsTheSameModel()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelContext(modelMock);

            var actualModel = modelContext.Model;

            Assert.AreSame(modelMock, actualModel);
        }

        [TestMethod]
        public void TestSettingTheModelPropertyChangesTheUnderlyingModel()
        {
            var expectedModel = new ModelMock();
            dynamic modelContext = new ModelContextMock(new ModelMock());

            modelContext.Model = expectedModel;
            var actualModel = modelContext.Model;

            Assert.AreSame(expectedModel, actualModel);
        }

        [TestMethod]
        public void TestTryingToSetTheModelToNullThrowsException()
        {
            dynamic modelContext = new ModelContextMock(new ModelMock());

            Assert.ThrowsException<ArgumentNullException>(() => modelContext.Model = null);
        }
    }
}