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
// 	Generated: 10/18/2015 23:32:43
// </auto-generatedInfo>
// --------------------------------------------------------------------------------------------------
namespace BillPath.Modern.Strings
{
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Core;
    
    
    public partial class DashboardPage
    {
        
        private static ResourceLoader resourceLoader;
        
        static DashboardPage()
        {
            string executingAssemblyName;
            executingAssemblyName = Windows.UI.Xaml.Application.Current.GetType().AssemblyQualifiedName;
            string[] executingAssemblySplit;
            executingAssemblySplit = executingAssemblyName.Split(',');
            executingAssemblyName = executingAssemblySplit[1];
            string currentAssemblyName;
            currentAssemblyName = typeof(DashboardPage).AssemblyQualifiedName;
            string[] currentAssemblySplit;
            currentAssemblySplit = currentAssemblyName.Split(',');
            currentAssemblyName = currentAssemblySplit[1];
            if (CoreWindow.GetForCurrentThread() != null)
            {
                if (executingAssemblyName.Equals(currentAssemblyName))
                {
                    resourceLoader = ResourceLoader.GetForCurrentView("DashboardPage");
                }
                else
                {
                    resourceLoader = ResourceLoader.GetForCurrentView(currentAssemblyName + "/DashboardPage");
                }
            }
            else
            {
                if (executingAssemblyName.Equals(currentAssemblyName))
                {
                    resourceLoader = ResourceLoader.GetForViewIndependentUse("DashboardPage");
                }
                else
                {
                    resourceLoader = ResourceLoader.GetForViewIndependentUse(currentAssemblyName + "/DashboardPage");
                }
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add"
        /// </summary>
        public static string AddButton_ToolTipService_ToolTip
        {
            get
            {
                return resourceLoader.GetString("AddButton/ToolTipService/ToolTip");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add Expense"
        /// </summary>
        public static string AddExpenseButton_Text
        {
            get
            {
                return resourceLoader.GetString("AddExpenseButton/Text");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add Expense Category"
        /// </summary>
        public static string AddExpenseCategoryButton_Text
        {
            get
            {
                return resourceLoader.GetString("AddExpenseCategoryButton/Text");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add Income"
        /// </summary>
        public static string AddIncomeButton_Text
        {
            get
            {
                return resourceLoader.GetString("AddIncomeButton/Text");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Charts"
        /// </summary>
        public static string ChartsButton_ToolTipService_ToolTip
        {
            get
            {
                return resourceLoader.GetString("ChartsButton/ToolTipService/ToolTip");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Delete selection"
        /// </summary>
        public static string DeleteButton_ToolTipService_ToolTip
        {
            get
            {
                return resourceLoader.GetString("DeleteButton/ToolTipService/ToolTip");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Edit selected item"
        /// </summary>
        public static string EditButton_ToolTipService_ToolTip
        {
            get
            {
                return resourceLoader.GetString("EditButton/ToolTipService/ToolTip");
            }
        }
    }
}