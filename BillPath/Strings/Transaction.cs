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
// 	Generated: 10/04/2015 10:59:10
// </auto-generatedInfo>
// --------------------------------------------------------------------------------------------------
namespace BillPath.Strings
{
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Core;
    
    
    public partial class Transaction
    {
        
        private static ResourceLoader resourceLoader;
        
        static Transaction()
        {
            string executingAssemblyName;
            executingAssemblyName = Windows.UI.Xaml.Application.Current.GetType().AssemblyQualifiedName;
            string[] executingAssemblySplit;
            executingAssemblySplit = executingAssemblyName.Split(',');
            executingAssemblyName = executingAssemblySplit[1];
            string currentAssemblyName;
            currentAssemblyName = typeof(Transaction).AssemblyQualifiedName;
            string[] currentAssemblySplit;
            currentAssemblySplit = currentAssemblyName.Split(',');
            currentAssemblyName = currentAssemblySplit[1];
            if (CoreWindow.GetForCurrentThread() != null)
            {
                if (executingAssemblyName.Equals(currentAssemblyName))
                {
                    resourceLoader = ResourceLoader.GetForCurrentView("Transaction");
                }
                else
                {
                    resourceLoader = ResourceLoader.GetForCurrentView(currentAssemblyName + "/Transaction");
                }
            }
            else
            {
                if (executingAssemblyName.Equals(currentAssemblyName))
                {
                    resourceLoader = ResourceLoader.GetForViewIndependentUse("Transaction");
                }
                else
                {
                    resourceLoader = ResourceLoader.GetForViewIndependentUse(currentAssemblyName + "/Transaction");
                }
            }
        }
        
        /// <summary>
        /// Localized resource similar to "The amount must have a currency."
        /// </summary>
        public static string Amount_MustHaveCurrency
        {
            get
            {
                return resourceLoader.GetString("Amount_MustHaveCurrency");
            }
        }
    }
}
