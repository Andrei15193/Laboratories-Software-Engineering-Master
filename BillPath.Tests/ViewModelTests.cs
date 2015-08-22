﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BillPath.UserInterface.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BillPath.Tests
{
    [TestClass]
    public class ViewModelTests
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
            var viewModel = new ViewModel<AttributeValidation>(new AttributeValidation());

            Assert.IsTrue(viewModel.HasErrors);
            Assert.AreEqual(1, viewModel.GetErrors(nameof(AttributeValidation.Value)).Count());
        }
        [TestMethod]
        public void TestRequiredAttributeValidationWithoutNull()
        {
            var viewModel = new ViewModel<AttributeValidation>(new AttributeValidation
            {
                Value = new object()
            });

            Assert.IsFalse(viewModel.HasErrors);
            Assert.AreEqual(0, viewModel.GetErrors(nameof(AttributeValidation.Value)).Count());
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
            var viewModel = new ViewModel<ValidatableObject>(new ValidatableObject(new ValidationResult("Error", new[] { "Property" })));

            Assert.IsTrue(viewModel.HasErrors);
            Assert.AreEqual(1, viewModel.GetErrors(null).Count());
        }
        [TestMethod]
        public void TestValidatableObjectWithoutError()
        {
            var viewModel = new ViewModel<ValidatableObject>(new ValidatableObject());

            Assert.IsFalse(viewModel.HasErrors);
            Assert.AreEqual(0, viewModel.GetErrors(null).Count());
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
            Assert.IsFalse(viewModel.HasErrors);

            viewModel.Property1 = new object();
            Assert.IsTrue(viewModel.HasErrors);
            Assert.AreEqual(1, viewModel.GetErrors(nameof(ValidatableObjectWithDependentProperties.Property1)).Count());
            Assert.AreEqual(1, viewModel.GetErrors(nameof(ValidatableObjectWithDependentProperties.Property2)).Count());
            Assert.AreEqual(2, viewModel.GetErrors(null).Count());
        }
    }
}