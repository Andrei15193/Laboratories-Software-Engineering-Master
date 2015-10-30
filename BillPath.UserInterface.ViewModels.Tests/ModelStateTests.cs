﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BillPath.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.UserInterface.ViewModels.Tests
{
    [TestClass]
    public class ModelStateTests
    {
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
            var modelState = new ModelState(new AttributeValidation());

            Assert.IsFalse(modelState.IsValid);
            Assert.AreEqual(1, modelState.Errors[nameof(AttributeValidation.Value)].Count());
        }
        [TestMethod]
        public void TestRequiredAttributeValidationWithoutNull()
        {
            var viewModel = new ModelState(new AttributeValidation
            {
                Value = new object()
            });

            Assert.IsTrue(viewModel.IsValid);
            Assert.AreEqual(0, viewModel.Errors[nameof(AttributeValidation.Value)].Count());
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
            var viewModel = new ModelState(new ValidatableObject(new ValidationResult("Error", new[] { "Property" })));

            Assert.IsFalse(viewModel.IsValid);
            Assert.AreEqual(1, viewModel.Errors["Property"].Count());
            Assert.AreEqual(0, viewModel.Errors[string.Empty].Count());
        }
        [TestMethod]
        public void TestValidatableObjectWithoutError()
        {
            var viewModel = new ModelState(new ValidatableObject());

            Assert.IsTrue(viewModel.IsValid);
            Assert.AreEqual(0, viewModel.Errors[string.Empty].Count());
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
        private class ValidatableObjectWithDependentPropertiesViewModel
            : ViewModel<ValidatableObjectWithDependentProperties>
        {
            public ValidatableObjectWithDependentPropertiesViewModel()
                : base(new ValidatableObjectWithDependentProperties())
            {
            }

            public object Property1
            {
                get
                {
                    return Model.Property1;
                }
                set
                {
                    Model.Property1 = value;
                    OnPropertyChanged();
                }
            }
            public object Property2
            {
                get
                {
                    return Model.Property2;
                }
                set
                {
                    Model.Property2 = value;
                    OnPropertyChanged();
                }
            }
        }
        [TestMethod]
        public void TestValidatableObjectWithDependentProperties()
        {
            var viewModel = new ValidatableObjectWithDependentPropertiesViewModel();
            Assert.IsTrue(viewModel.IsValid);

            viewModel.Property1 = new object();
            Assert.IsFalse(viewModel.IsValid);
            Assert.AreEqual(1, viewModel.Errors[nameof(ValidatableObjectWithDependentProperties.Property1)].Count());
            Assert.AreEqual(1, viewModel.Errors[nameof(ValidatableObjectWithDependentProperties.Property2)].Count());
            Assert.AreEqual(0, viewModel.Errors[string.Empty].Count());
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
            var viewModel = new ModelState(new ValidatableObjectWithInstanceLevelErrors());

            Assert.IsFalse(viewModel.IsValid);
            Assert.AreEqual(1, viewModel.Errors[string.Empty].Count());
        }
    }
}