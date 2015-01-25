using Andrei15193.DtoGen.Definition;
using System.IO;

namespace Andrei15193.DtoGen.Translator
{
    public interface ITextWriterFactory
    {
        TextWriter GetTextWriterForDto(DtoDefinition dtoDefinition);

        TextWriter GetTextWriterForDtoSerializer(DtoDefinition rootDtoDefinition);
    }
}