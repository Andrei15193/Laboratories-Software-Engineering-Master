using System;
using System.Text.RegularExpressions;

namespace Andrei15193.DtoGen.Definition
{
    public class DtoAttributeDefinition
    {
        public DtoAttributeDefinition(string name, DtoAttributePrimitiveType primitiveType, DtoMultiplicity multiplicity = default(DtoMultiplicity))
        {
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException("name");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "name");
            if (!Regex.IsMatch(name, @"^[_a-zA-Z]\w*$"))
                throw new ArgumentException("Must be a valid identifier", "name");

            _name = name.Trim();
            _primitiveType = primitiveType;
            _dtoDefinition = null;
            _multiplicity = multiplicity;
        }
        public DtoAttributeDefinition(string name, DtoDefinition dtoDefinition, DtoMultiplicity multiplicity = default(DtoMultiplicity))
        {
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException("name");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "name");
            if (!Regex.IsMatch(name, @"^[_a-zA-Z]\w*$"))
                throw new ArgumentException("Must be a valid identifier", "name");

            if (dtoDefinition == null)
                throw new ArgumentNullException("dtoDefinition");

            _name = name.Trim();
            _primitiveType = default(DtoAttributePrimitiveType);
            _dtoDefinition = dtoDefinition;
            _multiplicity = multiplicity;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string TypeName
        {
            get
            {
                if (_dtoDefinition != null)
                    return _dtoDefinition.Name;
                else
                    return _primitiveType.ToString();
            }
        }
        public DtoDefinition DtoDefinition
        {
            get
            {
                return _dtoDefinition;
            }
        }

        public DtoMultiplicity Multiplicity
        {
            get
            {
                return _multiplicity;
            }
        }

        public DtoAttributePrimitiveType? PrimitiveType
        {
            get
            {
                if (_dtoDefinition == null)
                    return _primitiveType;
                else
                    return null;
            }        
        }

        private readonly string _name;

        private readonly DtoDefinition _dtoDefinition;
        private readonly DtoAttributePrimitiveType _primitiveType;

        private readonly DtoMultiplicity _multiplicity;
    }
}