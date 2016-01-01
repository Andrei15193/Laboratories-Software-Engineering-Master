using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public abstract class ExpenseCategoryXmlRepository
        : IExpenseCategoryRepository
    {
        private const string _rootElementName = "expenses";
        private static readonly XmlTranslator<ExpenseCategory> _xmlTranslator = new ExpenseCategoryXmlTranslator();

        protected Task<Stream> GetReadStreamAsync()
            => GetReadStreamAsync(CancellationToken.None);
        protected abstract Task<Stream> GetReadStreamAsync(CancellationToken cancellationToken);

        protected Task<Stream> GetWriteStreamAsync()
            => GetWriteStreamAsync(CancellationToken.None);
        protected abstract Task<Stream> GetWriteStreamAsync(CancellationToken cancellationToken);

        protected virtual int StreamBufferSize
            => 2048;

        public Task<IEnumerable<ExpenseCategory>> GetAllAsync()
            => GetAllAsync(CancellationToken.None);
        public async Task<IEnumerable<ExpenseCategory>> GetAllAsync(CancellationToken cancellationToken)
        {
            var expenseCategories = new List<ExpenseCategory>();

            using (var expenseCategoryXmlReader = await _GetXmlReader(cancellationToken))
            {
                var expenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                while (expenseCategory != null)
                {
                    expenseCategories.Add(expenseCategory);
                    await expenseCategoryXmlReader.ReadAsync();
                    expenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                }
            }

            return expenseCategories;
        }

        public Task SaveAsync(ExpenseCategory expenseCategory)
            => SaveAsync(expenseCategory, CancellationToken.None);
        public async Task SaveAsync(ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            if (expenseCategory == null)
                throw new ArgumentNullException(nameof(expenseCategory));

            using (var temporaryStream = new MemoryStream())
            {
                using (var expenseCategoryXmlReader = await _GetXmlReader(cancellationToken))
                using (var expenseCategoryXmlWriter = _GetXmlWriter(temporaryStream))
                {
                    await expenseCategoryXmlWriter.WriteStartElementAsync(null, _rootElementName, null);
                    cancellationToken.ThrowIfCancellationRequested();

                    var existingExpenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                    while (existingExpenseCategory != null)
                    {
                        await _xmlTranslator.WriteToAsync(expenseCategoryXmlWriter, existingExpenseCategory, cancellationToken);

                        await expenseCategoryXmlReader.ReadAsync();
                        existingExpenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                    }
                    await _xmlTranslator.WriteToAsync(expenseCategoryXmlWriter, expenseCategory, cancellationToken);

                    await expenseCategoryXmlWriter.WriteEndElementAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                temporaryStream.Seek(0, SeekOrigin.Begin);
                using (var resultStream = await GetWriteStreamAsync(cancellationToken))
                    await temporaryStream.CopyToAsync(resultStream, StreamBufferSize, cancellationToken);
            }
        }

        public Task RemoveAsync(string name)
            => RemoveAsync(name, CancellationToken.None);
        public async Task RemoveAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(name));

            using (var temporaryStream = new MemoryStream())
            {
                using (var expenseCategoryXmlReader = await _GetXmlReader(cancellationToken))
                using (var expenseCategoryXmlWriter = _GetXmlWriter(temporaryStream))
                {
                    await expenseCategoryXmlWriter.WriteStartElementAsync(null, _rootElementName, null);
                    cancellationToken.ThrowIfCancellationRequested();

                    var expenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                    while (expenseCategory != null)
                    {
                        if (!name.Equals(expenseCategory.Name, StringComparison.OrdinalIgnoreCase))
                            await _xmlTranslator.WriteToAsync(expenseCategoryXmlWriter, expenseCategory, cancellationToken);

                        await expenseCategoryXmlReader.ReadAsync();
                        expenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                    }

                    await expenseCategoryXmlWriter.WriteEndElementAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                temporaryStream.Seek(0, SeekOrigin.Begin);
                using (var resultStream = await GetWriteStreamAsync(cancellationToken))
                    await temporaryStream.CopyToAsync(resultStream, StreamBufferSize, cancellationToken);
            }
        }

        public Task UpdateAsync(string expenseCategoryName, ExpenseCategory expenseCategory)
            => UpdateAsync(expenseCategoryName, expenseCategory, CancellationToken.None);
        public async Task UpdateAsync(string expenseCategoryName, ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(expenseCategoryName))
                if (expenseCategoryName == null)
                    throw new ArgumentNullException(nameof(expenseCategoryName));
                else
                    throw new ArgumentException("Cannot be empty or white space!", nameof(expenseCategoryName));
            if (expenseCategory == null)
                throw new ArgumentNullException(nameof(expenseCategory));

            using (var temporaryStream = new MemoryStream())
            {
                using (var expenseCategoryXmlReader = await _GetXmlReader(cancellationToken))
                using (var expenseCategoryXmlWriter = _GetXmlWriter(temporaryStream))
                {
                    await expenseCategoryXmlWriter.WriteStartElementAsync(null, _rootElementName, null);
                    cancellationToken.ThrowIfCancellationRequested();

                    var existingExpenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                    while (existingExpenseCategory != null)
                    {
                        if (expenseCategoryName.Equals(existingExpenseCategory.Name, StringComparison.OrdinalIgnoreCase))
                            await _xmlTranslator.WriteToAsync(expenseCategoryXmlWriter, expenseCategory, cancellationToken);
                        else
                            await _xmlTranslator.WriteToAsync(expenseCategoryXmlWriter, existingExpenseCategory, cancellationToken);

                        await expenseCategoryXmlReader.ReadAsync();
                        existingExpenseCategory = await _xmlTranslator.ReadFromAsync(expenseCategoryXmlReader, cancellationToken);
                    }

                    await expenseCategoryXmlWriter.WriteEndElementAsync();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                temporaryStream.Seek(0, SeekOrigin.Begin);
                using (var resultStream = await GetWriteStreamAsync(cancellationToken))
                    await temporaryStream.CopyToAsync(resultStream, StreamBufferSize, cancellationToken);
            }
        }

        private async Task<XmlReader> _GetXmlReader(CancellationToken cancellationToken)
            => XmlReader.Create(
                await GetReadStreamAsync(cancellationToken),
                new XmlReaderSettings
                {
                    Async = true,
                    CloseInput = true,
                    ConformanceLevel = ConformanceLevel.Auto
                });
        private XmlWriter _GetXmlWriter(Stream stream)
            => XmlWriter.Create(
                stream,
                new XmlWriterSettings
                {
                    Async = true,
                    CloseOutput = false,
                    ConformanceLevel = ConformanceLevel.Document,
                    Indent = false,
                    NewLineOnAttributes = false,
                    OmitXmlDeclaration = false,
                    WriteEndDocumentOnClose = true
                });
    }
}