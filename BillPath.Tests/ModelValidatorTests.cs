using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Tests
{
    [TestClass]
    public class ModelValidatorTests
    {
        [TestMethod]
        public void TestValidateNull()
            => Assert.ThrowsException<ArgumentNullException>(() => new ModelValidator().Validate(null));

        [TestMethod]
        public void TestValidateEmptyModelInstance()
        {
            var validationResults = new ModelValidator().Validate(new EmptyModel());

            Assert.AreEqual(
                0,
                validationResults.Count());
        }
        public class EmptyModel
        {
        }

        [TestMethod]
        public void TestValidateModelInstanceWithAnnotatedPropertyWithoutError()
        {
            var validationResults = new ModelValidator().Validate(new ValidAnnotatedModel());

            Assert.AreEqual(
                0,
                validationResults.Count());
        }
        public class ValidAnnotatedModel
        {
            [RegularExpression(".*")]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestValidateModelInstanceWithAnnotatedPropertyWithError()
        {
            var validationResults = new ModelValidator().Validate(new InvalidAnnotatedModel());

            Assert.AreEqual(
                1,
                validationResults.Count());
        }
        public class InvalidAnnotatedModel
        {
            [Required]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestValidateInvalidModelAndGetErrorMessageSpecifiedByAttribute()
        {
            var validationResult = new ModelValidator().Validate(new InvalidAnnotatedWithErrorMessageModel()).Single();

            Assert.AreEqual(
                InvalidAnnotatedWithErrorMessageModel.ErrorMessage,
                validationResult.ErrorMessage);
        }
        public class InvalidAnnotatedWithErrorMessageModel
        {
            public const string ErrorMessage = "This is a test error message";

            [Required(ErrorMessage = ErrorMessage)]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestValidateInvalidModelAndGetLocalizedErrorMessageSpecifiedByAttribute()
        {
            var validationResult = new ModelValidator().Validate(new InvalidAnnotatedWithLocalizedErrorMessageModel()).Single();

            Assert.AreEqual(
                ModelValidatorResourceTests.TestErrorMessage,
                validationResult.ErrorMessage);
        }
        public class InvalidAnnotatedWithLocalizedErrorMessageModel
        {
            [Required(
                ErrorMessageResourceName = nameof(ModelValidatorResourceTests.TestErrorMessage),
                ErrorMessageResourceType = typeof(ModelValidatorResourceTests))]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestPartiallyInvalidAnnotatedModel()
        {
            var validationResult = new ModelValidator().Validate(new PartiallyInvalidAnnotatedModel()).Single();

            Assert.AreEqual(
                nameof(PartiallyInvalidAnnotatedModel.Property1),
                validationResult.MemberNames.Single());
        }
        public class PartiallyInvalidAnnotatedModel
        {
            [Required(
                ErrorMessageResourceName = nameof(ModelValidatorResourceTests.TestErrorMessage),
                ErrorMessageResourceType = typeof(ModelValidatorResourceTests))]
            public string Property1
            {
                get;
                set;
            }
            [RegularExpression(".*")]
            public string Property2
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestFormatIsAppliedToErrorMessage()
        {
            var validationResult = new ModelValidator().Validate(new InvalidWithErrorMessageFormatModel()).Single();

            Assert.AreEqual(
                string.Format(
                    InvalidWithErrorMessageFormatModel.ErrorMessageFormat,
                    nameof(InvalidWithErrorMessageFormatModel.Property)),
                validationResult.ErrorMessage);
        }
        public class InvalidWithErrorMessageFormatModel
        {
            public const string ErrorMessageFormat = "This is a test error message for {0}.";

            [Required(ErrorMessage = ErrorMessageFormat)]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestFormatIsAppliedToLocalizedErrorMessage()
        {
            var validationResult = new ModelValidator().Validate(new InvalidWithLocalizedErrorMessageFormatModel()).Single();

            Assert.AreEqual(
                string.Format(
                    ModelValidatorResourceTests.TestErrorMessageFormat,
                    nameof(InvalidWithLocalizedErrorMessageFormatModel.Property)),
                validationResult.ErrorMessage);
        }
        public class InvalidWithLocalizedErrorMessageFormatModel
        {
            [Required(
                ErrorMessageResourceName = nameof(ModelValidatorResourceTests.TestErrorMessageFormat),
                ErrorMessageResourceType = typeof(ModelValidatorResourceTests))]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestFormatIsAppliedToErrorMessageUsingProvidedDisplayName()
        {
            var validationResult = new ModelValidator().Validate(new InvalidWithErrorMessageFormatWithDisplayOnPropertyModel()).Single();

            Assert.AreEqual(
                string.Format(
                    InvalidWithErrorMessageFormatWithDisplayOnPropertyModel.ErrorMessageFormat,
                    InvalidWithErrorMessageFormatWithDisplayOnPropertyModel.PropertyDisplayName),
                validationResult.ErrorMessage);
        }
        public class InvalidWithErrorMessageFormatWithDisplayOnPropertyModel
        {
            public const string PropertyDisplayName = nameof(Property) + "Test";
            public const string ErrorMessageFormat = "This is a test error message for {0}.";

            [Display(Name = PropertyDisplayName)]
            [Required(ErrorMessage = ErrorMessageFormat)]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestFormatIsAppliedToErrorMessageUsingProvidedLocalizedDisplayName()
        {
            var validationResult = new ModelValidator().Validate(new InvalidWithErrorMessageFormatWithDisplayOnPropertyModel()).Single();

            Assert.AreEqual(
                string.Format(
                    InvalidWithErrorMessageFormatWithLocalizedDisplayOnPropertyModel.ErrorMessageFormat,
                    ModelValidatorResourceTests.TestPropertyName),
                validationResult.ErrorMessage);
        }
        public class InvalidWithErrorMessageFormatWithLocalizedDisplayOnPropertyModel
        {
            public const string ErrorMessageFormat = "This is a test error message for {0}.";

            [Display(
                Name = nameof(ModelValidatorResourceTests.TestPropertyName),
                ResourceType = typeof(ModelValidatorResourceTests))]
            [Required(ErrorMessage = ErrorMessageFormat)]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestValidationErrorsAreReturnedIfModelImplementsIValidatableObject()
        {
            var validationResult = new ModelValidator().Validate(new ValidatableObjectModel()).Single();

            Assert.AreEqual(ValidatableObjectModel.ErrorMessage, validationResult.ErrorMessage);
        }
        public class ValidatableObjectModel
            : IValidatableObject
        {
            public const string ErrorMessage = "This is a test error message";

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                Assert.AreSame(
                    this,
                    validationContext.ObjectInstance);
                yield return new ValidationResult(ErrorMessage);
            }
        }

        [TestMethod]
        public void TestValidationIsDoneForBothAnnotationsAndIValidatableObjectImplementation()
        {
            var validationResults = new ModelValidator().Validate(new InvalidAnntoatedValidatableObjectModel());

            Assert.AreEqual(
                2,
                validationResults.Count());
        }
        public class InvalidAnntoatedValidatableObjectModel
            : IValidatableObject
        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                yield return new ValidationResult(string.Empty);
            }
            [Required]
            public string Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestStaticPropertiesAreNotValidated()
        {
            var validationResults = new ModelValidator().Validate(new RequiredStaticPropertyObjectModel());

            Assert.IsFalse(validationResults.Any());
        }

        public class RequiredStaticPropertyObjectModel
        {
            [Required]
            public static object Property
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void TestPrivatePropertiesAreNotValidated()
        {
            var validationResults = new ModelValidator().Validate(new RequiredPrivatePropertyObjectModel());

            Assert.IsFalse(validationResults.Any());
        }

        public class RequiredPrivatePropertyObjectModel
        {
            [Required]
            private static object Property
            {
                get;
                set;
            }
        }
    }
}