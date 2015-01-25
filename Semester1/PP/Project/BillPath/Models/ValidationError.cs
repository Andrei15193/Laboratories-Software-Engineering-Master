using System;

namespace BillPath.Models
{
    public struct ValidationError
        : IEquatable<ValidationError>
    {
        public ValidationError(Type declaringType, string propertyName, string errorId)
        {
            if (declaringType == null)
                throw new ArgumentNullException("declaringType ");

            if (string.IsNullOrWhiteSpace(errorId))
                if (errorId == null)
                    throw new ArgumentNullException("errorId");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "errorId");

            _declaringType = declaringType;
            _propertyName = (propertyName ?? string.Empty).Trim();
            _errorId = errorId.Trim();
        }

        public Type DeclaringType
        {
            get
            {
                return _declaringType;
            }
        }
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }
        public string ErrorId
        {
            get
            {
                return _errorId;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is ValidationError && Equals((ValidationError)obj));
        }
        public override int GetHashCode()
        {
            if (_errorId == null)
                return 0;
            else
                return _errorId.GetHashCode();
        }

        public bool Equals(ValidationError other)
        {
            return string.Equals(_errorId, other._errorId, StringComparison.OrdinalIgnoreCase);
        }

        private readonly Type _declaringType;
        private readonly string _propertyName;
        private readonly string _errorId;
    }
}