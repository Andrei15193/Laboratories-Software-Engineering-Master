using Andrei15193.DtoGen.Definition;
using System.IO;
using System.Text;

namespace Andrei15193.DtoGen.Translator.Mock
{
    internal class MockTextWriterFactory
        : ITextWriterFactory
    {
        public TextWriter GetTextWriterForDto(DtoDefinition dtoDefinition)
        {
            return new StringWriter(_stringBuilder);
        }

        public TextWriter GetTextWriterForDtoSerializer(DtoDefinition rootDtoDefinition)
        {
            return new StringWriter(_stringBuilder);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
        
        private readonly StringBuilder _stringBuilder = new StringBuilder();
    }
}