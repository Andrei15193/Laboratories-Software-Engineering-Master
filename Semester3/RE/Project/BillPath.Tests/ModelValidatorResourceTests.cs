//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------------------
// <auto-generatedInfo>
// 	This code was generated by ResW File Code Generator (http://reswcodegen.codeplex.com)
// 	ResW File Code Generator was written by Christian Resma Helle
// 	and is under GNU General Public License version 2 (GPLv2)
// 
// 	This code contains a helper class exposing property representations
// 	of the string resources defined in the specified .ResW file
// 
// 	Generated: 10/31/2015 16:16:24
// </auto-generatedInfo>
// --------------------------------------------------------------------------------------------------
namespace BillPath.Tests
{
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Core;


    public partial class ModelValidatorResourceTests
    {

        private static ResourceLoader resourceLoader;

        static ModelValidatorResourceTests()
        {
            string executingAssemblyName;
            executingAssemblyName = Windows.UI.Xaml.Application.Current.GetType().AssemblyQualifiedName;
            string[] executingAssemblySplit;
            executingAssemblySplit = executingAssemblyName.Split(',');
            executingAssemblyName = executingAssemblySplit[1];
            string currentAssemblyName;
            currentAssemblyName = typeof(ModelValidatorResourceTests).AssemblyQualifiedName;
            string[] currentAssemblySplit;
            currentAssemblySplit = currentAssemblyName.Split(',');
            currentAssemblyName = currentAssemblySplit[1];
            if (CoreWindow.GetForCurrentThread() != null)
            {
                if (executingAssemblyName.Equals(currentAssemblyName))
                {
                    resourceLoader = ResourceLoader.GetForCurrentView("ModelValidatorResourceTests");
                }
                else
                {
                    resourceLoader = ResourceLoader.GetForCurrentView(currentAssemblyName + "/ModelValidatorResourceTests");
                }
            }
            else
            {
                resourceLoader = ResourceLoader.GetForViewIndependentUse("ModelValidatorResourceTests");
            }
        }

        /// <summary>
        /// Localized resource similar to "This is a test error message"
        /// </summary>
        public static string TestErrorMessage
        {
            get
            {
                return resourceLoader.GetString("TestErrorMessage");
            }
        }

        /// <summary>
        /// Localized resource similar to "This is a test error message for {0}."
        /// </summary>
        public static string TestErrorMessageFormat
        {
            get
            {
                return resourceLoader.GetString("TestErrorMessageFormat");
            }
        }

        /// <summary>
        /// Localized resource similar to "PropertyTest"
        /// </summary>
        public static string TestPropertyName
        {
            get
            {
                return resourceLoader.GetString("TestPropertyName");
            }
        }
    }
}
