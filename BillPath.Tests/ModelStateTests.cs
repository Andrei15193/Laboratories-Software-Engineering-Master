using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class ModelStateTests
    {
        private sealed class ModelContextMock
            : ModelState
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
                () => new ModelState(null));

        [TestMethod]
        public void TestGettingModelPropertyValueThroughContext()
        {
            var expectedPropertyValue = new object();
            var modelMock =
                new ModelMock
                {
                    Property = expectedPropertyValue
                };
            dynamic modelContext = new ModelState(modelMock);

            object actualPropertyValue = modelContext.Property;

            Assert.AreSame(expectedPropertyValue, actualPropertyValue);
        }

        [TestMethod]
        public void TestTryingToGetTheValueOfAPropertyNotDefinedOnTheModelThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelState(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.PropertyThatDoesNotExist);
        }

        [TestMethod]
        public void TestTryingToGetTheValueOfAPropertyThatIsSetOnlyThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelState(modelMock);

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
            dynamic modelContext = new ModelState(modelMock);

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
            dynamic modelContext = new ModelState(modelMock);

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
            dynamic modelContext = new ModelState(modelMock);

            modelContext.Property = propertyValue;

            Assert.AreSame(propertyValue, modelMock.Property);
        }

        [TestMethod]
        public void TestTryingToSetTheValueOfAPropertyNotDefinedOnTheModelThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelState(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.PropertyThatDoesNotExist = null);
        }

        [TestMethod]
        public void TestTryingToSetTheValueOfAPropertyThatIsGetOnlyThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelState(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.GetOnlyProperty = null);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChanged()
        {
            var raiseCount = 0;
            dynamic modelContext = new ModelState(new ModelMock());
            ((INotifyPropertyChanged)modelContext).PropertyChanged += (sender, e) => raiseCount++;

            modelContext.Property = new object();

            Assert.AreEqual(1, raiseCount);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChangedWithTheContextAsSender()
        {
            object actualSender = null;
            dynamic modelContext = new ModelState(new ModelMock());
            ((INotifyPropertyChanged)modelContext).PropertyChanged += (sender, e) => actualSender = sender;

            modelContext.Property = new object();

            Assert.AreSame(modelContext, actualSender);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChangedWithTheCorrespondingPropertyName()
        {
            string actualPropertyName = null;
            dynamic modelContext = new ModelState(new ModelMock());
            ((INotifyPropertyChanged)modelContext).PropertyChanged += (sender, e) => actualPropertyName = e.PropertyName;

            modelContext.Property = new object();

            Assert.AreEqual("Property", actualPropertyName);
        }

        [TestMethod]
        public void TestPropertiesOnContextAreCaseSensitive()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelState(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelContext.property = null);
        }

        [TestMethod]
        public void TestModelPropertyReturnsTheSameModel()
        {
            var modelMock = new ModelMock();
            dynamic modelContext = new ModelState(modelMock);

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


        private class AttributeValidation
        {
            [Required]
            public object Value
            {
                get;
                set;
            }
        }
        [TestMethod]
        public void TestRequiredAttributeValidationWithNull()
        {
            dynamic modelState = new ModelState(new AttributeValidation());

            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors.Value));
        }
        [TestMethod]
        public void TestRequiredAttributeValidationWithoutNull()
        {
            dynamic modelState = new ModelState(
                new AttributeValidation
                {
                    Value = new object()
                });

            Assert.IsTrue(modelState.IsValid);
            Assert.AreEqual(0, Enumerable.Count(modelState.Errors.Value));
        }

        private class ValidatableObject
            : IValidatableObject
        {
            private readonly IEnumerable<ValidationResult> _validationResults;

            public ValidatableObject(IEnumerable<ValidationResult> validationResults = null)
            {
                _validationResults = validationResults ?? Enumerable.Empty<ValidationResult>();
            }
            public ValidatableObject(params ValidationResult[] validationResults)
                : this(validationResults.AsEnumerable())
            {
            }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                return _validationResults;
            }

            public object Property
            {
                get;
                set;
            }
        }
        [TestMethod]
        public void TestValidatableObjectWithError()
        {
            dynamic modelState = new ModelState(new ValidatableObject(new ValidationResult("Error", new[] { "Property" })));

            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors.Property));
            Assert.AreEqual(0, Enumerable.Count(modelState.Errors));
        }
        [TestMethod]
        public void TestValidatableObjectWithoutError()
        {
            dynamic modelState = new ModelState(new ValidatableObject());

            Assert.IsTrue(modelState.IsValid);
            Assert.AreEqual(0, Enumerable.Count(modelState.Errors));
        }

        private class ValidatableObjectWithDependentProperties
            : IValidatableObject
        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Property1 != null || Property2 != null)
                    yield return new ValidationResult(string.Empty, new[] { nameof(Property1), nameof(Property2) });
            }

            public object Property1
            {
                get;
                set;
            }
            public object Property2
            {
                get;
                set;
            }
        }
        private class ValidatableObjectWithDependentPropertiesModelState
            : ModelState
        {
            public ValidatableObjectWithDependentPropertiesModelState()
                : base(new ValidatableObjectWithDependentProperties())
            {
            }

            public object Property1
            {
                get
                {
                    return ((ValidatableObjectWithDependentProperties)Model).Property1;
                }
                set
                {
                    ((ValidatableObjectWithDependentProperties)Model).Property1 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Property1)));
                }
            }
            public object Property2
            {
                get
                {
                    return ((ValidatableObjectWithDependentProperties)Model).Property2;
                }
                set
                {
                    ((ValidatableObjectWithDependentProperties)Model).Property2 = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Property2)));
                }
            }
        }
        [TestMethod]
        public void TestValidatableObjectWithDependentProperties()
        {
            dynamic modelState = new ValidatableObjectWithDependentPropertiesModelState();
            Assert.IsTrue(modelState.IsValid);

            modelState.Property1 = new object();
            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors.Property1));
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors.Property2));
            Assert.AreEqual(0, Enumerable.Count(modelState.Errors));
        }

        public class ValidatableObjectWithInstanceLevelErrors
            : IValidatableObject
        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                yield return new ValidationResult("");
            }
        }
        [TestMethod]
        public void TestValidatableObjectWithInstanceLevelErrors()
        {
            dynamic modelState = new ModelState(new ValidatableObjectWithInstanceLevelErrors());

            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors));
        }
    }
}