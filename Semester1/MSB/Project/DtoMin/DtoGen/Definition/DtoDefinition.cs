using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Andrei15193.DtoGen.Definition
{
    public class DtoDefinition
    {
        public DtoDefinition(string name, IEnumerable<DtoAttributeDefinition> attributes)
        {
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException("name");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "name");
            if (!Regex.IsMatch(name, @"^[_a-zA-Z]\w*$"))
                throw new ArgumentException("Must be a valid identifier", "name");

            if (attributes == null)
                throw new ArgumentNullException("attributes");

            _name = name.Trim();
            _attributes = attributes.ToList();

            if (_attributes.Any(attribute => _attributes.Where(otherAttribute => otherAttribute != attribute).Any(otherAttribute => attribute.Name.Equals(otherAttribute.Name, StringComparison.OrdinalIgnoreCase))))
                throw new ArgumentException("Attribute names must be unique within a dto", "attributes");
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public IEnumerable<DtoAttributeDefinition> Attributes
        {
            get
            {
                return _attributes;
            }
        }

        private readonly string _name;
        private readonly IEnumerable<DtoAttributeDefinition> _attributes;
    }
}