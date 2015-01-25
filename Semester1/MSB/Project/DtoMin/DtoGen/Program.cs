using Andrei15193.DtoGen.Definition;
using Andrei15193.DtoGen.Translator;
using Andrei15193.DtoGen.Translator.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace Andrei15193.DtoGen
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");
            if (args.Length == 0)
                Console.WriteLine("There are no input files.");
            else
            {
                ITextWriterFactory textWriterFactory = new CSharpFileStreamWriterFactory((from arg in args
                                                                                          let splitResult = arg.Split(new[] { ':' }, 2)
                                                                                          where splitResult.Length == 2 && "-folderPath".Equals(splitResult[0].Trim())
                                                                                          select splitResult[1]).FirstOrDefault() ?? Environment.CurrentDirectory);
                CSharpDtoDefinitionTranslator cSharpDtoDefinitionTranslato = new CSharpDtoDefinitionTranslator();

                foreach (FileInfo file in args.Where(File.Exists).Select(arg => new FileInfo(arg)))
                    if (".xml".Equals(file.Extension, StringComparison.OrdinalIgnoreCase) || ".dto".Equals(file.Extension, StringComparison.OrdinalIgnoreCase))
                        try
                        {
                            try
                            {
                                using (Stream fileStream = file.OpenRead())
                                {
                                    DtoDefinitions dtoDefinitions;

                                    if (".xml".Equals(file.Extension, StringComparison.OrdinalIgnoreCase))
                                        dtoDefinitions = DtoDefinitions.FromXml(XmlReader.Create(fileStream, new XmlReaderSettings { CloseInput = false }));
                                    else
                                        dtoDefinitions = DtoDefinitions.FromDtoSpecification(new StreamReader(fileStream).ReadToEnd());

                                    Console.WriteLine(string.Format("Generating DTOs from {0}", file.Name));

                                    cSharpDtoDefinitionTranslato.TranslateAsync(dtoDefinitions, textWriterFactory).Wait();

                                    Console.WriteLine(string.Format("Finished generating DTOs from {0}", file.Name));
                                }
                            }
                            catch (InvalidOperationException invalidOperationException)
                            {
                                XmlException xmlException = invalidOperationException.InnerException as XmlException;
                                if (xmlException != null)
                                    throw xmlException;

                                XmlSchemaException xmlSchemaException = invalidOperationException.InnerException as XmlSchemaException;
                                if (xmlSchemaException != null)
                                    throw xmlSchemaException;

                                throw invalidOperationException;
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(string.Format("File {0} is invalid: {1}", file.Name, exception.Message));
                        }
                    else
                        Console.WriteLine(string.Format("File {0} cannot be a valid dto specification {1}", file.Name, file.DirectoryName));
            }
        }
    }
}