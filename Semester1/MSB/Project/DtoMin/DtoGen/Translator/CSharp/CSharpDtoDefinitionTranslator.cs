using Andrei15193.DtoGen.Definition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andrei15193.DtoGen.Translator.CSharp
{
    public class CSharpDtoDefinitionTranslator
        : IDtoDefinitionTranslator
    {
        public async Task TranslateAsync(DtoDefinitions dtoDefinitions, ITextWriterFactory textWriterFactory)
        {
            if (dtoDefinitions == null)
                throw new ArgumentNullException("dtoDefinitions");
            if (textWriterFactory == null)
                throw new ArgumentNullException("textWriterFactory");

            await _WriteDtos(dtoDefinitions, textWriterFactory);
            await _GenerateDataContractDtoSerializers(dtoDefinitions, textWriterFactory);
            await _GenerateXmlDtoSerializers(dtoDefinitions, textWriterFactory);
        }

        private async Task _GenerateDataContractDtoSerializers(DtoDefinitions dtoDefinitions, ITextWriterFactory textWriterFactory)
        {
            byte indent = 0;
            
            foreach (DtoDefinition rootDtoDefinition in dtoDefinitions.RootDtoDefinitions)
                using (TextWriter textWriter = textWriterFactory.GetTextWriterForDtoSerializer(rootDtoDefinition))
                {
                    if (!string.IsNullOrWhiteSpace(dtoDefinitions.Namespace))
                    {
                        await textWriter.WriteAsync("namespace ").ConfigureAwait(false);
                        await textWriter.WriteLineAsync(dtoDefinitions.Namespace).ConfigureAwait(false);

                        await textWriter.WriteLineAsync("{").ConfigureAwait(false);
                        indent = 1;
                    }

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("public partial class ").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("DataContractSerializer").ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                    indent += 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("public ").ConfigureAwait(false);

                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteAsync(" Deserialize").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("(string filePath)").ConfigureAwait(false);


                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                    indent += 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("global::System.Runtime.Serialization.DataContractSerializer dataContractSerializer = new global::System.Runtime.Serialization.DataContractSerializer(typeof(").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("));").ConfigureAwait(false);

                    await textWriter.WriteLineAsync(string.Empty).ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("using (global::System.IO.FileStream fileStream = new global::System.IO.FileStream(filePath, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read))").ConfigureAwait(false);

                    indent += 1;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("return (").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(")dataContractSerializer.ReadObject(fileStream);").ConfigureAwait(false);

                    indent -= 2;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("}").ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("public void").ConfigureAwait(false);
                    await textWriter.WriteAsync(" Serialize").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteAsync("(string filePath, ").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteAsync(' ').ConfigureAwait(false);
                    await textWriter.WriteAsync(_LowerCaseFirstCharacter(rootDtoDefinition.Name)).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(")").ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                    indent += 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("global::System.Runtime.Serialization.DataContractSerializer dataContractSerializer = new global::System.Runtime.Serialization.DataContractSerializer(typeof(").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("));").ConfigureAwait(false);

                    await textWriter.WriteLineAsync(string.Empty).ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("using (global::System.IO.FileStream fileStream = new global::System.IO.FileStream(filePath, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read))").ConfigureAwait(false);

                    indent += 1;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("dataContractSerializer.WriteObject(fileStream, ").ConfigureAwait(false);
                    await textWriter.WriteAsync(_LowerCaseFirstCharacter(rootDtoDefinition.Name)).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(");").ConfigureAwait(false);

                    indent -= 2;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("}").ConfigureAwait(false);

                    indent -= 1;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("}").ConfigureAwait(false);

                    indent -= 1;
                    if (!string.IsNullOrWhiteSpace(dtoDefinitions.Namespace))
                        await textWriter.WriteLineAsync("}").ConfigureAwait(false);
                }
        }

        private async Task _GenerateXmlDtoSerializers(DtoDefinitions dtoDefinitions, ITextWriterFactory textWriterFactory)
        {
            byte indent = 0;

            foreach (DtoDefinition rootDtoDefinition in dtoDefinitions.RootDtoDefinitions)
                using (TextWriter textWriter = textWriterFactory.GetTextWriterForDtoSerializer(rootDtoDefinition))
                {
                    if (!string.IsNullOrWhiteSpace(dtoDefinitions.Namespace))
                    {
                        await textWriter.WriteAsync("namespace ").ConfigureAwait(false);
                        await textWriter.WriteLineAsync(dtoDefinitions.Namespace).ConfigureAwait(false);

                        await textWriter.WriteLineAsync("{").ConfigureAwait(false);
                        indent = 1;
                    }

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("public partial class ").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("XmlSerializer").ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                    indent += 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("public ").ConfigureAwait(false);

                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteAsync(" Deserialize").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("(string filePath)").ConfigureAwait(false);


                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                    indent += 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("global::System.Xml.Serialization.XmlSerializer xmlSerializer = new global::System.Xml.Serialization.XmlSerializer(typeof(").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("));").ConfigureAwait(false);

                    await textWriter.WriteLineAsync(string.Empty).ConfigureAwait(false);
                    
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("using (global::System.IO.FileStream fileStream = new global::System.IO.FileStream(filePath, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read))").ConfigureAwait(false);

                    indent += 1;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("return (").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(")xmlSerializer.Deserialize(fileStream);").ConfigureAwait(false);

                    indent -= 2;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("}").ConfigureAwait(false);
                    
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("public void").ConfigureAwait(false);
                    await textWriter.WriteAsync(" Serialize").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteAsync("(string filePath, ").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteAsync(' ').ConfigureAwait(false);
                    await textWriter.WriteAsync(_LowerCaseFirstCharacter(rootDtoDefinition.Name)).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(")").ConfigureAwait(false);
                    
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                    indent += 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("global::System.Xml.Serialization.XmlSerializer xmlSerializer = new global::System.Xml.Serialization.XmlSerializer(typeof(").ConfigureAwait(false);
                    await textWriter.WriteAsync(rootDtoDefinition.Name).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("));").ConfigureAwait(false);

                    await textWriter.WriteLineAsync(string.Empty).ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("using (global::System.IO.FileStream fileStream = new global::System.IO.FileStream(filePath, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read))").ConfigureAwait(false);

                    indent += 1;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("xmlSerializer.Serialize(fileStream, ").ConfigureAwait(false);
                    await textWriter.WriteAsync(_LowerCaseFirstCharacter(rootDtoDefinition.Name)).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(");").ConfigureAwait(false);

                    indent -= 2;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("}").ConfigureAwait(false);

                    indent -= 1;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("}").ConfigureAwait(false);

                    indent -= 1;
                    if (!string.IsNullOrWhiteSpace(dtoDefinitions.Namespace))
                        await textWriter.WriteLineAsync("}").ConfigureAwait(false);
                }
        }

        private static async Task _WriteDtos(DtoDefinitions dtoDefinitions, ITextWriterFactory textWriterFactory)
        {
            foreach (DtoDefinition dtoDefinition in dtoDefinitions)
                using (TextWriter textWriter = textWriterFactory.GetTextWriterForDto(dtoDefinition))
                    await _WriteDto(dtoDefinition, textWriter, dtoDefinitions.Namespace);
        }

        private static async Task<byte> _WriteDto(DtoDefinition dtoDefinition, TextWriter textWriter, string @namespace = null)
        {
            byte indent = 0;

            if (!string.IsNullOrWhiteSpace(@namespace))
            {
                await textWriter.WriteAsync("namespace ").ConfigureAwait(false);
                await textWriter.WriteLineAsync(@namespace).ConfigureAwait(false);

                await textWriter.WriteLineAsync("{").ConfigureAwait(false);
                indent = 1;
            }

            await _IndentAsync(textWriter, indent).ConfigureAwait(false);
            await textWriter.WriteAsync("[global::System.Runtime.Serialization.DataContract(Name = \"").ConfigureAwait(false);
            await textWriter.WriteAsync(_LowerCaseFirstCharacter(dtoDefinition.Name)).ConfigureAwait(false);
            await textWriter.WriteLineAsync("\")]").ConfigureAwait(false);

            await _IndentAsync(textWriter, indent).ConfigureAwait(false);
            await textWriter.WriteAsync("[global::System.Xml.Serialization.XmlRoot(\"").ConfigureAwait(false);
            await textWriter.WriteAsync(_LowerCaseFirstCharacter(dtoDefinition.Name)).ConfigureAwait(false);
            await textWriter.WriteLineAsync("\")]").ConfigureAwait(false);

            await _IndentAsync(textWriter, indent).ConfigureAwait(false);
            await textWriter.WriteAsync("public partial class ").ConfigureAwait(false);
            await textWriter.WriteLineAsync(dtoDefinition.Name).ConfigureAwait(false);

            await _IndentAsync(textWriter, indent).ConfigureAwait(false);
            await textWriter.WriteLineAsync("{").ConfigureAwait(false);

            indent += 1;

            IList<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();

            foreach (DtoAttributeDefinition dtoAttributeDefinition in dtoDefinition.Attributes)
            {
                KeyValuePair<string, string> field = new KeyValuePair<string, string>(_GetFieldName(dtoAttributeDefinition.Name), _GetDtoAttributeTypeName(dtoAttributeDefinition));

                await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                await textWriter.WriteAsync("[global::System.Runtime.Serialization.DataMember(Name = \"").ConfigureAwait(false);
                await textWriter.WriteAsync(_LowerCaseFirstCharacter(dtoAttributeDefinition.Name)).ConfigureAwait(false);
                await textWriter.WriteLineAsync("\")]").ConfigureAwait(false);
                if (dtoAttributeDefinition.Multiplicity == DtoMultiplicity.Collection)
                {
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("[global::System.Xml.Serialization.XmlArray(\"").ConfigureAwait(false);
                    await textWriter.WriteAsync(_LowerCaseFirstCharacter(dtoAttributeDefinition.Name)).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("\")]").ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("[global::System.Xml.Serialization.XmlArrayItem(\"").ConfigureAwait(false);
                    await textWriter.WriteAsync(_LowerCaseFirstCharacter(dtoAttributeDefinition.TypeName)).ConfigureAwait(false);

                    await textWriter.WriteAsync("\", typeof(").ConfigureAwait(false);
                    await textWriter.WriteAsync(_GetDtoAttributeTypeName(dtoAttributeDefinition, true)).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("))]").ConfigureAwait(false);
                }
                else
                {
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    if (dtoAttributeDefinition.PrimitiveType.HasValue)
                        await textWriter.WriteAsync("[global::System.Xml.Serialization.XmlAttribute(\"").ConfigureAwait(false);
                    else
                        await textWriter.WriteAsync("[global::System.Xml.Serialization.XmlElement(\"").ConfigureAwait(false);
                    await textWriter.WriteAsync(_LowerCaseFirstCharacter(dtoAttributeDefinition.Name)).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("\")]").ConfigureAwait(false);
                }

                await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                await textWriter.WriteAsync("public ").ConfigureAwait(false);

                await textWriter.WriteAsync(field.Value).ConfigureAwait(false);

                await textWriter.WriteAsync(" ").ConfigureAwait(false);
                await textWriter.WriteLineAsync(dtoAttributeDefinition.Name).ConfigureAwait(false);

                await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                indent += 1;

                if (dtoAttributeDefinition.Multiplicity == DtoMultiplicity.Collection)
                {
                    fields.Add(field);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("get").ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("{").ConfigureAwait(false);

                    indent += 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("if (").ConfigureAwait(false);
                    await textWriter.WriteAsync(field.Key).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(" == null)").ConfigureAwait(false);

                    indent += 1;
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync(field.Key).ConfigureAwait(false);
                    await textWriter.WriteAsync(" = new ").ConfigureAwait(false);
                    await textWriter.WriteAsync(field.Value).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("();").ConfigureAwait(false);
                    indent -= 1;

                    await textWriter.WriteLineAsync().ConfigureAwait(false);
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("return ").ConfigureAwait(false);
                    await textWriter.WriteAsync(field.Key).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(";").ConfigureAwait(false);

                    indent -= 1;

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("}").ConfigureAwait(false);
                }
                else
                {
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("get;").ConfigureAwait(false);

                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteLineAsync("set;").ConfigureAwait(false);
                }

                indent -= 1;

                await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                await textWriter.WriteLineAsync("}").ConfigureAwait(false);
            }

            if (fields.Any())
            {
                await textWriter.WriteLineAsync().ConfigureAwait(false);

                foreach (KeyValuePair<string, string> field in fields)
                {
                    await _IndentAsync(textWriter, indent).ConfigureAwait(false);
                    await textWriter.WriteAsync("private ").ConfigureAwait(false);
                    await textWriter.WriteAsync(field.Value).ConfigureAwait(false);
                    await textWriter.WriteAsync(" ").ConfigureAwait(false);
                    await textWriter.WriteAsync(field.Key).ConfigureAwait(false);
                    await textWriter.WriteLineAsync(" = null;").ConfigureAwait(false);
                }
            }

            indent -= 1;

            await _IndentAsync(textWriter, indent).ConfigureAwait(false);
            await textWriter.WriteLineAsync("}").ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(@namespace))
                await textWriter.WriteLineAsync("}").ConfigureAwait(false);
            return indent;
        }

        private static string _LowerCaseFirstCharacter(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException("name");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "name");

            StringBuilder fieldNameBuilder = new StringBuilder(name.Trim());

            fieldNameBuilder[0] = char.ToLower(fieldNameBuilder[0]);

            return fieldNameBuilder.ToString();
        }

        private static string _GetFieldName(string name)
        {
            return ("_" + _LowerCaseFirstCharacter(name));
        }

        private static string _GetDtoAttributeTypeName(DtoAttributeDefinition attributeDefinition, bool ignoreMultiplicity = false)
        {
            if (attributeDefinition == null)
                throw new ArgumentNullException("attributeDefinition");

            StringBuilder typeNameStringBuilder = new StringBuilder();

            if (!ignoreMultiplicity && attributeDefinition.Multiplicity == DtoMultiplicity.Collection)
                typeNameStringBuilder.Append("global::System.Collections.Generic.List<");

            switch (attributeDefinition.PrimitiveType)
            {
                case DtoAttributePrimitiveType.Text:
                    typeNameStringBuilder.Append("string");
                    break;

                case DtoAttributePrimitiveType.Int:
                    typeNameStringBuilder.Append("int");
                    break;

                case DtoAttributePrimitiveType.Float:
                    typeNameStringBuilder.Append("double");
                    break;

                case DtoAttributePrimitiveType.DateTime:
                    typeNameStringBuilder.Append("global::System.DateTime");
                    break;

                default:
                    typeNameStringBuilder.Append(attributeDefinition.DtoDefinition.Name);
                    break;
            }

            if (!ignoreMultiplicity && attributeDefinition.Multiplicity == DtoMultiplicity.Collection)
                typeNameStringBuilder.Append(">");

            return typeNameStringBuilder.ToString();
        }

        private static Task _IndentAsync(TextWriter textWriter, byte indent)
        {
            if (textWriter == null)
                throw new ArgumentNullException("textWriter");

            return textWriter.WriteAsync(new string(' ', indent * 4));
        }
    }
}





























