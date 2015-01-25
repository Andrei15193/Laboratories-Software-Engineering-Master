using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Andrei15193.DtoEditorClassifier
{
    internal static class DtoEditorClassifierClassificationDefinition
    {
        /// <summary>
        /// Defines the "Andrei15193.DtoMinEditorClassifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Andrei15193.DtoMinEditorClassifier")]
        internal static ClassificationTypeDefinition DtoEditorClassifierType = null;
    }
}