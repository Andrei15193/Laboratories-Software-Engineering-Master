using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BillPath.Models;

namespace BillPath.DataAccess.Xml
{
    public class ExpenseCategoryXmlTranslator
        : XmlTranslator<ExpenseCategory>
    {
        public override async Task<ExpenseCategory> ReadFromAsync(XmlReader xmlReader, CancellationToken cancellationToken)
        {
            if (!await xmlReader.ReadUntilAsync(nameof(ExpenseCategory).ToXmlName(), cancellationToken))
                return null;

            return new ExpenseCategory
            {
                Name = xmlReader.GetAttribute(nameof(ExpenseCategory.Name).ToXmlName()),
                Color = _ToArgbColor(xmlReader.GetAttribute(nameof(ExpenseCategory.Color).ToXmlName()))
            };
        }

        public override async Task WriteToAsync(XmlWriter xmlWriter, ExpenseCategory expenseCategory, CancellationToken cancellationToken)
        {
            if (xmlWriter == null)
                throw new ArgumentNullException(nameof(xmlWriter));
            if (expenseCategory == null)
                throw new ArgumentNullException(nameof(expenseCategory));

            await xmlWriter.WriteStartElementAsync(null, nameof(ExpenseCategory).ToXmlName(), null);
            cancellationToken.ThrowIfCancellationRequested();

            await xmlWriter.WriteAttributeStringAsync(
                null,
                nameof(ExpenseCategory.Name).ToXmlName(),
                null,
                expenseCategory.Name);
            await xmlWriter.WriteAttributeStringAsync(
                null,
                nameof(ExpenseCategory.Color).ToXmlName(),
                null,
                _ToArgbString(expenseCategory.Color));
            cancellationToken.ThrowIfCancellationRequested();

            await xmlWriter.WriteEndElementAsync();
        }

        private string _ToArgbString(ArgbColor color)
            => $"#{color.Alpha:X2}{color.Red:X2}{color.Green:X2}{color.Blue:X2}";
        private ArgbColor _ToArgbColor(string argbString)
        {
            var match = Regex.Match(
                argbString,
                $@"^\#
                    (?<{nameof(ArgbColor.Alpha)}>[0-9A-F]{{2}})
                    (?<{nameof(ArgbColor.Red)}>[0-9A-F]{{2}})
                    (?<{nameof(ArgbColor.Green)}>[0-9A-F]{{2}})
                    (?<{nameof(ArgbColor.Blue)}>[0-9A-F]{{2}})
                $",
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            if (!match.Success)
                throw new ArgumentException("Invalid color format", nameof(argbString));

            return new ArgbColor(
                byte.Parse(match.Groups[nameof(ArgbColor.Alpha)].Value, NumberStyles.HexNumber),
                byte.Parse(match.Groups[nameof(ArgbColor.Red)].Value, NumberStyles.HexNumber),
                byte.Parse(match.Groups[nameof(ArgbColor.Green)].Value, NumberStyles.HexNumber),
                byte.Parse(match.Groups[nameof(ArgbColor.Blue)].Value, NumberStyles.HexNumber));
        }
    }
}