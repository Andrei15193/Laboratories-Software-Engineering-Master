using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace PSharpCodeFileGenerator
{
    [Guid("D2BABFBA-BA52-4252-88D7-E90496535640")]
    public class PSharpCodeFileGenerator
        : IVsSingleFileGenerator
    {
        private readonly string _InstalationDirectory = @"C:\Program Files (x86)\PSharp";

        public int DefaultExtension(out string extension)
            => (extension = ".Predicates.cs").Length;

        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            var tempDirectory = new DirectoryInfo(Path.GetTempPath()).CreateSubdirectory("PSharpCodeFileGenerator");
            try
            {
                pcbOutput = 0;
                var inputFileInfo = new FileInfo(wszInputFilePath);
                if (!inputFileInfo.Exists || inputFileInfo.Extension != ".pl")
                    return -1;

                foreach (var psharpInstalledFileInfo in new DirectoryInfo(_InstalationDirectory).GetFiles())
                    psharpInstalledFileInfo.CopyTo(Path.Combine(tempDirectory.FullName, psharpInstalledFileInfo.Name));

                using (var compileProcess = Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-File \"{ Path.Combine(_InstalationDirectory, "compile.ps1") }\" \"{wszInputFilePath}\"",
                        WorkingDirectory = tempDirectory.FullName,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }))
                    compileProcess.WaitForExit();

                var resultFile = new StringBuilder();
                foreach (var generatedFile in tempDirectory.GetFiles("*.cs"))
                    foreach (var resultFileLine in File.ReadAllLines(generatedFile.FullName).Skip(6))
                        resultFile.AppendLine(resultFileLine);

                if (!string.IsNullOrWhiteSpace(wszDefaultNamespace))
                    resultFile.Replace("namespace JJC.Psharp.Predicates", $"namespace {wszDefaultNamespace}");

                resultFile.Replace("using Predicates = JJC.Psharp.Predicates;", "using JJC.Psharp.Predicates;");
                resultFile.Replace("new Predicates.", "new ");

                byte[] resultFileBytes = Encoding.UTF8.GetBytes(resultFile.ToString());
                rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(resultFileBytes.Length);
                Marshal.Copy(resultFileBytes, 0, rgbOutputFileContents[0], resultFileBytes.Length);
                pcbOutput = (uint)resultFileBytes.LongLength;

                return 0;
            }
            finally
            {
                tempDirectory.Delete(true);
            }
        }

        [ComRegisterFunction]
        private static void RegisterFunction(Type t)
        {
            var customToolGuid = ((GuidAttribute)t.GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;
            using (var registryKey = Registry.CurrentUser.CreateSubKey(GetRegistryKeyFor("PSharpCodeFileGenerator")))
            {
                registryKey.SetValue("", "Generates C# source files from P# source files.");
                registryKey.SetValue("CLSID", $"{{{customToolGuid}}}");
                registryKey.SetValue("GeneratesDesignTimeSource", 1);
            }
        }

        [ComUnregisterFunction]
        private static void UnregisterFunction(Type t)
            => Registry.CurrentUser.DeleteSubKey(GetRegistryKeyFor("PSharpCodeFileGenerator"), true);

        private static string GetRegistryKeyFor(string name)
            => $@"SOFTWARE\Microsoft\VisualStudio\14.0_Config\Generators\{{fae04ec1-301f-11d3-bf4b-00c04f79efbc}}\{name}";
    }
}