using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.ApplicationModel.Resources.Core;

namespace BillPath.Strings.Tests
{
    [TestClass]
    public class ResourceKeyTests
    {
        [TestMethod]
        public void TestResourceKeyIsDefinedForAllLanguages()
        {
            var languagesByModelResourceKeys =
                (from modelType in typeof(Models.Expense).GetTypeInfo().Assembly.DefinedTypes
                 where !string.IsNullOrWhiteSpace(modelType.Namespace)
                       && modelType.Namespace.EndsWith(".Models")
                 from modelResourcePair in ResourceManager.Current.MainResourceMap
                 where modelResourcePair
                    .Key
                    .StartsWith($"{typeof(Models.Expense).GetTypeInfo().Assembly.GetName().Name}/{modelType.Name}")
                 from modelResourceCandidate in modelResourcePair.Value.Candidates
                 let modelResourceLanguages =
                     (from modelResourceCandidateQualifier in modelResourceCandidate.Qualifiers
                      where "language".Equals(
                          modelResourceCandidateQualifier.QualifierName,
                          StringComparison.OrdinalIgnoreCase)
                      select modelResourceCandidateQualifier.QualifierValue)
                     .DefaultIfEmpty("en")
                 from modelResourceLanguage in modelResourceLanguages.Distinct(StringComparer.OrdinalIgnoreCase)
                 select new
                 {
                     Key = modelResourcePair.Key,
                     Language = modelResourceLanguage
                 })
                .ToLookup(resource => resource.Key, resource => resource.Language);

            var languages = new HashSet<string>(
                languagesByModelResourceKeys.OrderByDescending(Enumerable.Count).First(),
                StringComparer.OrdinalIgnoreCase);

            var missingResources = from languagesByModelResourceKey in languagesByModelResourceKeys
                                   where languagesByModelResourceKey.Count() != languages.Count
                                   let missingLanguages = languages.Except(
                                       languagesByModelResourceKey,
                                       StringComparer.OrdinalIgnoreCase)
                                   where missingLanguages.Any()
                                   select new
                                   {
                                       Key = languagesByModelResourceKey.Key,
                                       Languages = missingLanguages
                                   };

            if (missingResources.Any())
                Assert.Fail(
                    new StringBuilder()
                    .AppendLine("The following resource keys are missing from all languages")
                    .AppendLine(
                        string.Join(
                            Environment.NewLine,
                            from missingResource in missingResources
                            select string.Format(
                                "Resource key: {0}, language{1}: {2}",
                                missingResource.Key,
                                missingResource.Languages.Count() > 1 ? "s" : string.Empty,
                                string.Join(
                                    ", ",
                                    missingResource.Languages))))
                    .ToString());
        }
    }
}