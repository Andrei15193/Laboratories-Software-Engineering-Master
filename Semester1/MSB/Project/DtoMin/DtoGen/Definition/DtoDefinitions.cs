using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Andrei15193.DtoGen.Definition
{
    public class DtoDefinitions
        : IReadOnlyCollection<DtoDefinition>
    {
        public static DtoDefinitions FromXml(XmlReader xmlReader)
        {
            XDocument dtoDefinitionXmlDocument = XDocument.Load(xmlReader);

            dtoDefinitionXmlDocument.Validate(_GetDtoDefinitionXmlSchemas(), null);

            return new DtoDefinitionsFactory().Create(dtoDefinitionXmlDocument);
        }

        public static DtoDefinitions FromDtoSpecification(string dtoSpecification)
        {
            if (string.IsNullOrWhiteSpace(dtoSpecification))
                if (dtoSpecification == null)
                    throw new ArgumentNullException("dtoSpecification");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "dtoSpecification");

            Dictionary<string, Lazy<DtoDefinition>> dtoDefinitions = new Dictionary<string, Lazy<DtoDefinition>>(StringComparer.OrdinalIgnoreCase);

            for (Match match = Regex.Match(dtoSpecification, @"\s*dto\s+(?<dtoName>[_a-z]\w*)\s*
                                                                \{
                                                                    (?<dtoAttribute>\s*
                                                                        (?<dtoAttributeName>[_a-z]\w*)\s*:\s*(?<dtoAttributeType>[_a-z]\w*)\s*(?<isCollection>\*?)\s*
                                                                    )*
                                                                \}\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture); match.Success; match = match.NextMatch())
                dtoDefinitions.Add(match.Groups["dtoName"].Value, new Lazy<DtoDefinition>(_GetDtoDefinition(match, dtoDefinitions)));

            return new DtoDefinitions(dtoDefinitions.Values.Select(dtoDefinition => dtoDefinition.Value),
                                      Regex.Match(dtoSpecification,
                                                  @"\A[\r\t ]*namespace[\r\t ]+(?<namespace>[_a-z]\w*(\.[_a-z]\w*)*)[\r\t ]*$",
                                                  RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Multiline)
                                           .Groups["namespace"]
                                           .Value);
        }

        private static Func<DtoDefinition> _GetDtoDefinition(Match match, IReadOnlyDictionary<string, Lazy<DtoDefinition>> dtoDefinitions)
        {
            return () => new DtoDefinition(match.Groups["dtoName"].Value,
                                           match.Groups["dtoAttribute"].Captures.OfType<Capture>().Select((capture, captureIndex) => captureIndex)
                                                .Select(captureIndex =>
                                                {
                                                    DtoAttributePrimitiveType primitiveType;

                                                    if (Enum.TryParse<DtoAttributePrimitiveType>(match.Groups["dtoAttributeType"].Captures[captureIndex].Value, true, out primitiveType))
                                                        return new DtoAttributeDefinition(match.Groups["dtoAttributeName"].Captures[captureIndex].Value,
                                                                                          primitiveType,
                                                                                          (match.Groups["isCollection"].Captures[captureIndex].Length == 0 ? DtoMultiplicity.Single : DtoMultiplicity.Collection));
                                                    else
                                                        return new DtoAttributeDefinition(match.Groups["dtoAttributeName"].Captures[captureIndex].Value,
                                                                                          dtoDefinitions[match.Groups["dtoAttributeType"].Captures[captureIndex].Value].Value,
                                                                                          (match.Groups["isCollection"].Captures[captureIndex].Length == 0 ? DtoMultiplicity.Single : DtoMultiplicity.Collection));
                                                }));
        }

        private static XmlSchemaSet _GetDtoDefinitionXmlSchemas()
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            using (Stream stream = typeof(Program).Assembly.GetManifestResourceStream("Andrei15193.DtoGen.DtoGenSchema.xsd"))
                schemas.Add(XmlSchema.Read(stream, null));

            return schemas;
        }

        public DtoDefinitions(IEnumerable<DtoDefinition> dtoDefinitions, string @namespace = null)
        {
            if (dtoDefinitions == null)
                throw new ArgumentNullException("dtoDefinitions");

            _dtoDefinitions = dtoDefinitions.ToList();
            _namespace = (string.IsNullOrWhiteSpace(@namespace) ? null : @namespace.Trim());

            if (_dtoDefinitions.Any(dtoDefinition => _dtoDefinitions.Where(otherDtoDefinition => otherDtoDefinition != dtoDefinition).Any(otherDtoDefinition => otherDtoDefinition.Name.Equals(dtoDefinition.Name, StringComparison.OrdinalIgnoreCase))))
                throw new ArgumentException("Dto names must be unique within a dto definition!", "dtoDefinitions");
        }

        public string Namespace
        {
            get
            {
                return _namespace;
            }
        }

        public IEnumerable<DtoDefinition> RootDtoDefinitions
        {
            get
            {
                return from dto in this
                       where this.All(otherDto => otherDto == dto || otherDto.Attributes.All(otherDtoAttribute => !dto.Equals(otherDtoAttribute.DtoDefinition)))
                       select dto;
            }
        }

        public int Count
        {
            get
            {
                return _dtoDefinitions.Count;
            }
        }

        public IEnumerator<DtoDefinition> GetEnumerator()
        {
            return _dtoDefinitions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private readonly string _namespace;
        private readonly IReadOnlyCollection<DtoDefinition> _dtoDefinitions;

        private class DtoDefinitionsFactory
        {
            internal DtoDefinitions Create(XDocument dtoDefinitionXmlDocument)
            {
                _dtoDefs = dtoDefinitionXmlDocument.Root
                                                   .Elements("{http://storage.andrei15193.ro/public/dtoGenSchema.xsd}dto")
                                                   .ToDictionary(dtoXElement => dtoXElement.Attribute("name").Value,
                                                                 dtoXElement => new Lazy<DtoDefinition>(() => _GetDtoDefinition(dtoXElement)));

                XAttribute namespaceXAttribute = dtoDefinitionXmlDocument.Root.Attribute("namespace");

                return new DtoDefinitions(_dtoDefs.Values.Select(lazyDtoDefinition => lazyDtoDefinition.Value), (namespaceXAttribute == null ? null : namespaceXAttribute.Value));
            }

            private DtoDefinition _GetDtoDefinition(XElement dtoXElement)
            {
                return new DtoDefinition(dtoXElement.Attribute("name").Value,
                                         dtoXElement.Elements("{http://storage.andrei15193.ro/public/dtoGenSchema.xsd}attribute").Select(_GetDtoAttributeDefinition));
            }

            private DtoAttributeDefinition _GetDtoAttributeDefinition(XElement dtoAttributeXElement)
            {
                try
                {
                    DtoAttributePrimitiveType primitiveType;

                    if (Enum.TryParse<DtoAttributePrimitiveType>(dtoAttributeXElement.Attribute("type").Value, true, out primitiveType))
                        return new DtoAttributeDefinition(dtoAttributeXElement.Attribute("name").Value,
                                                          primitiveType,
                                                          _GetDtoAttributeMultiplicity(dtoAttributeXElement));
                    else
                        return new DtoAttributeDefinition(dtoAttributeXElement.Attribute("name").Value,
                                                          _dtoDefs[dtoAttributeXElement.Attribute("type").Value].Value,
                                                          _GetDtoAttributeMultiplicity(dtoAttributeXElement));
                }
                catch (KeyNotFoundException keyNotFoundException)
                {
                    throw new ArgumentException("Refering undeclared DTO " + dtoAttributeXElement.Attribute("type").Value, keyNotFoundException);
                }
            }

            private static DtoMultiplicity _GetDtoAttributeMultiplicity(XElement dtoAttributeXElement)
            {
                DtoMultiplicity multiplicity;
                XAttribute multiplicityXAttribute = dtoAttributeXElement.Attribute("multiplicity");

                if (multiplicityXAttribute == null || !Enum.TryParse<DtoMultiplicity>(multiplicityXAttribute.Value, true, out multiplicity))
                    multiplicity = default(DtoMultiplicity);

                return multiplicity;
            }

            private IDictionary<string, Lazy<DtoDefinition>> _dtoDefs = null;
        }
    }
}