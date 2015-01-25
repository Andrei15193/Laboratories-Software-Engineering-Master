using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace Andrei15193.DtoEditorClassifier
{
    internal class FileAndContentTypeDefinitions
    {
        [Export]
        [Name("dto")]
        [BaseDefinition("text")]
        internal static ContentTypeDefinition DtoContentTypeDefinition;

        [Export]
        [FileExtension(".dto")]
        [ContentType("dto")]
        internal static FileExtensionToContentTypeDefinition DtoFileExtensionDefinition;
    }

    #region Provider definition
    /// <summary>
    /// This class causes a classifier to be added to the set of classifiers. Since 
    /// the content type is set to "text", this classifier applies to all text files
    /// </summary>
    [Export(typeof(IClassifierProvider))]
    [ContentType("dto")]
    //[FileExtension(".dto")]
    internal class DtoMinEditorClassifierProvider
        : IClassifierProvider
    {
        /// <summary>
        /// Import the classification registry to be used for getting a reference
        /// to the custom classification type later.
        /// </summary>
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null; // Set via MEF

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty<DtoEditorClassifier>(delegate
            {
                return new DtoEditorClassifier(ClassificationRegistry);
            });
        }
    }
    #endregion //provider def

    #region Classifier
    /// <summary>
    /// Classifier that classifies all text as an instance of the OrinaryClassifierType
    /// </summary>
    internal class DtoEditorClassifier
        : IClassifier
    {
        private IClassificationType _classificationType;
        private static readonly IEnumerable<string> _keyWords = new SortedSet<string>(StringComparer.OrdinalIgnoreCase) { "dto", "int", "float", "text", "dateTime", "namespace" };

        internal DtoEditorClassifier(IClassificationTypeRegistryService registry)
        {
            _classificationType = registry.GetClassificationType("Andrei15193.DtoMinEditorClassifier");
        }

        /// <summary>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </summary>
        /// <param name="trackingSpan">The span currently being classified</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            //create a list to hold the results
            List<ClassificationSpan> classifications = new List<ClassificationSpan>();

            for (Match match = Regex.Match(span.GetText(), @"(?<isAttributeType>:\s+)?(?<attributeType>\w+)"); match.Success; match = match.NextMatch())
                if (_keyWords.Contains(match.Groups["attributeType"].Value) || match.Groups["isAttributeType"].Success)
                    classifications.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start.Add(match.Groups["attributeType"].Index), match.Groups["attributeType"].Length)), _classificationType));

            return classifications;
        }

#pragma warning disable 67
        // This event gets raised if a non-text change would affect the classification in some way,
        // for example typing /* would cause the classification to change in C# without directly
        // affecting the span.
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67
    }
    #endregion //Classifier
}