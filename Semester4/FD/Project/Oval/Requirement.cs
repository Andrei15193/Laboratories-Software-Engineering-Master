using System;
using System.Collections.Generic;
using System.Linq;

namespace Oval
{
    /// <summary>
    /// Represents a requirement expressed in a textual meaner in regards with a number of members
    /// that must be meet in order for a <see cref="IConstraint{TValue}"/> to be satisfied.
    /// </summary>
    public sealed class Requirement
    {
        /// <summary>
        /// Creates a new <see cref="Requirement"/> with the provided textual
        /// requirement regarding the object itself and not only a particular member.
        /// </summary>
        /// <param name="text">
        /// A textual description of what must be meet.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when text is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when text is white space.
        /// </exception>
        public Requirement(string text)
            : this(text, null)
        {
        }
        /// <summary>
        /// Creates a new <see cref="Requirement"/> with the provided textual
        /// requirement regarding a number of members of an object.
        /// </summary>
        /// <param name="text">
        /// A textual description of what must be meet.
        /// </param>
        /// <param name="memberNames">
        /// The members that must fulfil the requirement.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when text is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when text is white space or
        /// memberNames contains null or white space items.
        /// </exception>
        public Requirement(string text, IEnumerable<string> memberNames)
        {
            if (string.IsNullOrWhiteSpace(text))
                if (text == null)
                    throw new ArgumentNullException(nameof(text));
                else
                    throw new ArgumentException(
                        "Text must not be white space",
                        nameof(text));
            if (memberNames != null && memberNames.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException(
                    "All member names must not be null or white space",
                    nameof(memberNames));

            Text = text;
            MemberNames = memberNames ?? Enumerable.Empty<string>();
        }
        /// <summary>
        /// Creates a new <see cref="Requirement"/> with the provided textual
        /// requirement regarding a number of members of an object.
        /// </summary>
        /// <param name="text">
        /// A textual description of what must be meet.
        /// </param>
        /// <param name="memberNames">
        /// The members that must fulfil the requirement.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when text is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when text is white space or
        /// memberNames contains null or white space items.
        /// </exception>
        public Requirement(string text, params string[] memberNames)
            : this(text, memberNames.AsEnumerable())
        {
        }

        /// <summary>
        /// Gets the requirement text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the member names for which the requirement applies. If the collection is empty
        /// then the requirement relates to the whole object rather than a number of members.
        /// </summary>
        public IEnumerable<string> MemberNames { get; }
    }
}