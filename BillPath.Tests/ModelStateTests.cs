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
        private sealed class ModelStateMock
            : ModelState<object>
        {
            public ModelStateMock(object model)
                : base(model)
            {
            }

            new public object Model
            {
                get
                {
                    return base.Model;
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
            => Assert.ThrowsException<ArgumentNullException>(() => ModelStates.GetFor(model: null));

        [TestMethod]
        public void TestGettingModelPropertyValueThroughContext()
        {
            var expectedPropertyValue = new object();
            var modelMock =
                new ModelMock
                {
                    Property = expectedPropertyValue
                };
            dynamic modelState = ModelStates.GetFor(modelMock);

            object actualPropertyValue = modelState.Property;

            Assert.AreSame(expectedPropertyValue, actualPropertyValue);
        }

        [TestMethod]
        public void TestTryingToGetTheValueOfAPropertyNotDefinedOnTheModelThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelState = ModelStates.GetFor(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelState.PropertyThatDoesNotExist);
        }

        [TestMethod]
        public void TestTryingToGetTheValueOfAPropertyThatIsSetOnlyThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelState = ModelStates.GetFor(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelState.SetOnlyProperty);
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
            dynamic modelState = ModelStates.GetFor(modelMock);

            object actualPropertyValue = modelState.StringProperty;

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
            dynamic modelState = ModelStates.GetFor(modelMock);

            modelState.Property = propertyValue;

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
            dynamic modelState = ModelStates.GetFor(modelMock);

            modelState.Property = propertyValue;

            Assert.AreSame(propertyValue, modelMock.Property);
        }

        [TestMethod]
        public void TestTryingToSetTheValueOfAPropertyNotDefinedOnTheModelThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelState = ModelStates.GetFor(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelState.PropertyThatDoesNotExist = null);
        }

        [TestMethod]
        public void TestTryingToSetTheValueOfAPropertyThatIsGetOnlyThrowsAnException()
        {
            var modelMock = new ModelMock();
            dynamic modelState = ModelStates.GetFor(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelState.GetOnlyProperty = null);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChanged()
        {
            var raiseCount = 0;
            dynamic modelState = ModelStates.GetFor(new ModelMock());
            ((INotifyPropertyChanged)modelState).PropertyChanged += (sender, e) => raiseCount++;

            modelState.Property = new object();

            Assert.AreEqual(1, raiseCount);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChangedWithTheContextAsSender()
        {
            object actualSender = null;
            dynamic modelState = ModelStates.GetFor(new ModelMock());
            ((INotifyPropertyChanged)modelState).PropertyChanged += (sender, e) => actualSender = sender;

            modelState.Property = new object();

            Assert.AreSame(modelState, actualSender);
        }

        [TestMethod]
        public void TestSettingTheValueOfAPropertyRaisesPropertyChangedWithTheCorrespondingPropertyName()
        {
            string actualPropertyName = null;
            dynamic modelState = ModelStates.GetFor(new ModelMock());
            ((INotifyPropertyChanged)modelState).PropertyChanged += (sender, e) => actualPropertyName = e.PropertyName;

            modelState.Property = new object();

            Assert.AreEqual("Property", actualPropertyName);
        }

        [TestMethod]
        public void TestPropertiesOnContextAreCaseSensitive()
        {
            var modelMock = new ModelMock();
            dynamic modelState = ModelStates.GetFor(modelMock);

            Assert.ThrowsException<RuntimeBinderException>(() => modelState.property = null);
        }

        [TestMethod]
        public void TestModelPropertyReturnsTheSameModel()
        {
            var modelMock = new ModelMock();
            dynamic modelState = ModelStates.GetFor(modelMock);

            var actualModel = modelState.Model;

            Assert.AreSame(modelMock, actualModel);
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
            dynamic modelState = ModelStates.GetFor(new AttributeValidation());

            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors.Value));
        }
        [TestMethod]
        public void TestRequiredAttributeValidationWithoutNull()
        {
            dynamic modelState = ModelStates.GetFor(
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
            dynamic modelState = ModelStates.GetFor(new ValidatableObject(new ValidationResult("Error", new[] { "Property" })));

            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors.Property));
            Assert.AreEqual(0, Enumerable.Count(modelState.Errors));
        }
        [TestMethod]
        public void TestValidatableObjectWithoutError()
        {
            dynamic modelState = ModelStates.GetFor(new ValidatableObject());

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
            : ModelState<ValidatableObjectWithDependentProperties>
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
            dynamic modelState = ModelStates.GetFor(new ValidatableObjectWithInstanceLevelErrors());

            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, Enumerable.Count(modelState.Errors));
        }

        private sealed class AggregateRootModel
        {
            public class ChildModel
            {
            }

            public ChildModel Property
            {
                get;
                set;
            }
        }
        [TestMethod]
        public void TestModelStateIsReturnedForNonPrimitiveProperties()
        {
            dynamic modelState = ModelStates.GetFor(new AggregateRootModel { Property = new AggregateRootModel.ChildModel() });

            dynamic propertyModelState = modelState.Property;

            Assert.IsInstanceOfType(propertyModelState, typeof(ModelState));
        }

        [TestMethod]
        public void TestObjectPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState(new object());

        [TestMethod]
        public void TestBytePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<byte>();
        [TestMethod]
        public void TestSBytePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<sbyte>();
        [TestMethod]
        public void TestInt16PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<short>();
        [TestMethod]
        public void TestUInt16PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<ushort>();
        [TestMethod]
        public void TestInt32PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<int>();
        [TestMethod]
        public void TestUInt32PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<uint>();
        [TestMethod]
        public void TestInt64PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<long>();
        [TestMethod]
        public void TestUInt64PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<ulong>();

        [TestMethod]
        public void TestSinglePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<float>();
        [TestMethod]
        public void TestDoublePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<double>();
        [TestMethod]
        public void TestDecimalPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<decimal>();

        [TestMethod]
        public void TestCharPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<char>();
        [TestMethod]
        public void TestStringPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState(string.Empty);

        [TestMethod]
        public void TestDateTimePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<DateTime>();
        [TestMethod]
        public void TestDateTimeOffsetPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<DateTimeOffset>();

        [TestMethod]
        public void TestNullableBytePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<byte?>(default(byte));
        [TestMethod]
        public void TestNullableSBytePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<sbyte?>(default(sbyte));
        [TestMethod]
        public void TestNullableInt16PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<short?>(default(short));
        [TestMethod]
        public void TestNullableUInt16PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<ushort?>(default(ushort));
        [TestMethod]
        public void TestNullableInt32PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<int?>(default(int));
        [TestMethod]
        public void TestNullableUInt32PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<uint?>(default(uint));
        [TestMethod]
        public void TestNullableInt64PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<long?>(default(long));
        [TestMethod]
        public void TestNullableUInt64PropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<ulong?>(default(ulong));

        [TestMethod]
        public void TestNullableSinglePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<float?>(default(float));
        [TestMethod]
        public void TestNullableDoublePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<double?>(default(double));
        [TestMethod]
        public void TestNullableDecimalPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<decimal?>(default(decimal));

        [TestMethod]
        public void TestNullableCharPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<char?>(default(char));

        [TestMethod]
        public void TestNullableDateTimePropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<DateTime?>(default(DateTime));
        [TestMethod]
        public void TestNullableDateTimeOffsetPropertyIsNotWrappedInModelState()
            => _AssertIsNotWrappedInModelState<DateTimeOffset?>(default(DateTimeOffset));

        private void _AssertIsNotWrappedInModelState<T>(T propertyValue = default(T))
        {
            dynamic modelState = ModelStates.GetFor(new { Property = propertyValue });

            object value = modelState.Property;

            Assert.IsInstanceOfType(value, typeof(T));
        }

        private enum CustomEnum
        {
        }
        [TestMethod]
        public void TestCustomEnumPropertyIsWrappedInModelState()
            => _AssertIsNotWrappedInModelState<CustomEnum>();
        private delegate void CustomDelegate();
        [TestMethod]
        public void TestCustomDelegatePropertyIsWrappedInModelState()
            => _AssertIsNotWrappedInModelState(new CustomDelegate(delegate { }));

        private sealed class CustomClass
            : ICustomInterface
        {
        }
        private interface ICustomInterface
        {
        }
        private struct CustomStruct
        {
        }

        [TestMethod]
        public void TestCustomClassPropertyIsWrappedInModelState()
            => _AssertIsWrappedInModelState(new CustomClass());
        [TestMethod]
        public void TestCustomInterfacePropertyIsWrappedInModelState()
            => _AssertIsWrappedInModelState<ICustomInterface>(new CustomClass());
        [TestMethod]
        public void TestCustomStructPropertyIsWrappedInModelState()
            => _AssertIsWrappedInModelState<CustomStruct>();

        private void _AssertIsWrappedInModelState<T>(T propertyValue = default(T))
        {
            dynamic modelState = ModelStates.GetFor(new { Property = propertyValue });

            object value = modelState.Property;

            Assert.IsInstanceOfType(value, typeof(ModelState));
        }

        private sealed class CircularReferenceAggregate
        {
            public CircularReferenceAggregate CircularReferenceProperty
            {
                get;
                set;
            }
        }
        [TestMethod]
        public void TestCircularReferenceUsesSameViewStateForSameValues()
        {
            var model = new CircularReferenceAggregate();
            model.CircularReferenceProperty = model;
            dynamic modelState = ModelStates.GetFor(model);

            Assert.AreSame(modelState, modelState.CircularReferenceProperty);
        }
    }
}