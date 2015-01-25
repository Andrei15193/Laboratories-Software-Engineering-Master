using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace DtoEditorClassifier
{
    #region Format definition
    /// <summary>
    /// Defines an editor format for the Andrei15193.DtoMinEditorClassifier type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Andrei15193.DtoEditorClassifier")]
    [Name("Andrei15193.DtoMinEditorClassifier")]
    [UserVisible(true)] //this should be visible to the end user
    [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
    internal sealed class DtoEditorClassifierFormat
        : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Andrei15193.DtoMinEditorClassifier" classification type
        /// </summary>
        public DtoEditorClassifierFormat()
        {
            DisplayName = "Andrei15193.DtoMinEditorClassifier"; //human readable version of the name
            ForegroundColor = Colors.Red;
            IsBold = true;
            //TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
    #endregion //Format definition
}