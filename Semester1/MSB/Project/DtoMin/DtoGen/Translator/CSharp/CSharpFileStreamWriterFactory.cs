using Andrei15193.DtoGen.Definition;
using System;
using System.IO;

namespace Andrei15193.DtoGen.Translator.CSharp
{
    public class CSharpFileStreamWriterFactory
        : ITextWriterFactory
    {
        public CSharpFileStreamWriterFactory(string destinationFolderPath)
        {
            if (string.IsNullOrWhiteSpace(destinationFolderPath))
                if (destinationFolderPath == null)
                    throw new ArgumentNullException("destinationFolderPath");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "destinationFolderPath");

            _destinationFolderPath = destinationFolderPath;
        }

        public TextWriter GetTextWriterForDto(DtoDefinition dtoDefinition)
        {
            if (dtoDefinition == null)
                throw new ArgumentNullException("dtoDefinition");

            Directory.CreateDirectory(Path.Combine(_destinationFolderPath));

            return new StreamWriter(new FileStream(Path.Combine(_destinationFolderPath,
                                                                dtoDefinition.Name + ".cs"),
                                                   FileMode.Create,
                                                   FileAccess.ReadWrite,
                                                   FileShare.Read));
        }

        public TextWriter GetTextWriterForDtoSerializer(DtoDefinition rootDtoDefinition)
        {
            if (rootDtoDefinition == null)
                throw new ArgumentNullException("rootDtoDefinition");

            Directory.CreateDirectory(Path.Combine(_destinationFolderPath, _serializersDirectoryName));

            return new StreamWriter(new FileStream(Path.Combine(_destinationFolderPath,
                                                                _serializersDirectoryName,
                                                                rootDtoDefinition.Name + ".cs"),
                                                   FileMode.Create,
                                                   FileAccess.ReadWrite,
                                                   FileShare.Read));
        }

        private readonly string _destinationFolderPath;
        private const string _serializersDirectoryName = "Serialization";
    }
}