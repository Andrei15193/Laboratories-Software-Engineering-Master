using Andrei15193.DtoGen.Definition;
using System.IO;
using System.Threading.Tasks;

namespace Andrei15193.DtoGen.Translator
{
    public interface IDtoDefinitionTranslator
    {
        Task TranslateAsync(DtoDefinitions dtoDefinitions, ITextWriterFactory textWriterFactory);
    }
}