using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oval
{
    /// <summary>
    /// Provides a number of static methods for building AnyConstraints. AnyConstraints
    /// aggregate multiple <see cref="IConstraint{TValue}"/>s and a set of
    /// <see cref="Requirement"/>s. Any of the aggregated
    /// <see cref="IConstraint{TValue}"/>s can satisfy the <see cref="Requirement"/>s.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The aggregated <see cref="IConstraint{TValue}"/> are invoked one at a time, in
    /// the same order they have been provided to the builder, until one that is
    /// satisfied has been found. When this happens all following
    /// <see cref="IConstraint{TValue}"/>s are ignored and it is considered that the
    /// AnyConstraint <see cref="Requirement"/>s are meet. This is similar to evaluating
    /// <see cref="bool"/> expression.
    /// </para>
    /// <para>
    /// In case none of the aggregated <see cref="IConstraint{TValue}"/> are fulfiled,
    /// all <see cref="Requirement"/>s that have been provided to the builder are
    /// returned in the same order they have been added regardless of which overloads
    /// were used to do so.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// AnyConstraints are useful to aggregating multiple constraints in a similar
    /// fashion in which <see cref="bool"/> expressions are aggregated using logical
    /// disjunction. In essence, if any of the aggregated
    /// <see cref="IConstraint{TValue}"/> is fulfiled then all <see cref="Requirement"/>s
    /// of the AnyConstraint are meet. If all of the aggregated
    /// <see cref="IConstraint{TValue}"/> are not satisfied then then all
    /// <see cref="Requirement"/>s of the AnyConstraint are not satisfied and only these
    /// are returned by
    /// <see cref="IConstraint{TValue}.CheckAsync(TValue, CancellationToken)"/>.
    /// </para>
    /// <para>
    /// In order to build such a constraint one must call a number of methods. Please note
    /// that the builder can be called in a fluent fashion.
    /// </para>
    /// <code>
    /// var constraint = AnyConstraint
    ///     .From&lt;string&gt;(value =&gt; value == null)
    ///     .Or(value =&gt; value.StartsWith("Test"))
    ///     .Fulfils(new Requirement("Value must be either null or start with \"Test\""))
    ///     .AsOneConstraint();
    /// 
    /// var requirements = await constraint.CheckAsync("not valid");
    /// 
    /// Assert.AreEqual(
    ///     "Value must be either null or start with \"Test\"",
    ///     requirements.Single().Text,
    ///     ignoreCase: false);
    /// </code>
    /// </example>
    public static class AnyConstraint
    {
        /// <summary>
        /// Represents a predicate for checking whether value satisfies a constraint.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="value">
        /// The value to check whether it fulfils the constraint.
        /// </param>
        /// <returns>
        /// Returns true if the constraint is satisfied; false otherwise.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate bool Predicate<TValue>(TValue value);

        /// <summary>
        /// Represents an asynchronous predicate for checking whether value satisfies a
        /// constraint.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="value">
        /// The value to check whether it fulfils the constraint.
        /// </param>
        /// <param name="cancellationToken">
        /// A token that can be checked to whether cancel the check.
        /// </param>
        /// <returns>
        /// Returns an awaitable task which in turn results in true if the constraint is
        /// satisfied; false otherwise.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate Task<bool> AsyncPredicate<TValue>(TValue value, CancellationToken cancellationToken);

        /// <summary>
        /// Represents a function that provides just one <see cref="Requirement"/>.
        /// </summary>
        /// <returns>
        /// Returns a single <see cref="Requirement"/> that an object must meet.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate Requirement RequirementProvider();

        /// <summary>
        /// Represents a function that provides a number of <see cref="Requirement"/>s.
        /// </summary>
        /// <returns>
        /// Returns a number of <see cref="Requirement"/>s that an object must meet.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate IEnumerable<Requirement> RequirementsProvider();

        /// <summary>
        /// Initializes an AnyConstraint builder to which additional constraints can be
        /// added and eventually a collection of <see cref="Requirement"/>s that any of
        /// them fulfil.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="constraint">
        /// The constraint to initialize the builder with.
        /// </param>
        /// <returns>
        /// Returns an AnyConstraint builder that can be used to construct an
        /// AnyConstraint.
        /// </returns>
        public static IConstraintEnumerationBuilder<TValue> From<TValue>(IConstraint<TValue> constraint)
            => new AnyConstraintBuilder<TValue>(constraint);

        /// <summary>
        /// Initializes an AnyConstraint builder to which additional constraints can be
        /// added and eventually a collection of <see cref="Requirement"/>s that any of
        /// them fulfil.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="predicate">
        /// A predicate that indicates whether the object fulfils or not a constraint.
        /// </param>
        /// <returns>
        /// Returns an AnyConstraint builder that can be used to construct an
        /// AnyConstraint.
        /// </returns>
        public static IConstraintEnumerationBuilder<TValue> From<TValue>(Predicate<TValue> predicate)
            => new AnyConstraintBuilder<TValue>(_GetConstraintFrom(predicate));

        /// <summary>
        /// Initializes an AnyConstraint builder to which additional constraints can be
        /// added and eventually a collection of <see cref="Requirement"/>s that any of
        /// them fulfil.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="asyncPredicate">
        /// An asynchronous predicate that indicates whether the object fulfils or not a
        /// constraint.
        /// </param>
        /// <returns>
        /// Returns an AnyConstraint builder that can be used to construct an
        /// AnyConstraint.
        /// </returns>
        public static IConstraintEnumerationBuilder<TValue> From<TValue>(AsyncPredicate<TValue> asyncPredicate)
            => new AnyConstraintBuilder<TValue>(_GetConstraintFrom(asyncPredicate));

        /// <summary>
        /// Represents the interface of an AnyConstraint builder which allows adding
        /// <see cref="IConstraint{TValue}"/>s and eventually transitioning to a builder
        /// which allows adding of <see cref="Requirement"/>s and eventually creating
        /// the <see cref="IConstraint{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface IConstraintEnumerationBuilder<TValue>
        {
            /// <summary>
            /// Adds an <see cref="IConstraint{TValue}"/> to the collection to the
            /// builder.
            /// </summary>
            /// <param name="constraint">
            /// The <see cref="IConstraint{TValue}"/> to aggregate.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Or")]
            IConstraintEnumerationBuilder<TValue> Or(IConstraint<TValue> constraint);

            /// <summary>
            /// Adds an <see cref="IConstraint{TValue}"/> that is created from the
            /// predicate.
            /// </summary>
            /// <param name="predicate">
            /// A predicate which tells whether a constraint is satisfied.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Or")]
            IConstraintEnumerationBuilder<TValue> Or(Predicate<TValue> predicate);

            /// <summary>
            /// Adds an <see cref="IConstraint{TValue}"/> that is created from the
            /// predicate.
            /// </summary>
            /// <param name="asyncPredicate">
            /// An asynchronous predicate which tells whether a constraint is satisfied.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Or")]
            IConstraintEnumerationBuilder<TValue> Or(AsyncPredicate<TValue> asyncPredicate);

            /// <summary>
            /// Add the <see cref="Requirement"/> to the builder and transitions it into
            /// the state where only <see cref="Requirement"/>s can be added and
            /// eventually have the the <see cref="IConstraint{TValue}"/> created.
            /// </summary>
            /// <param name="requirement">
            /// A <see cref="Requirement"/> that the AnyConstraint will return in case
            /// all none of the aggregated <see cref="IConstraint{TValue}"/>s are
            /// satisfied.
            /// </param>
            /// <returns>
            /// Returns a continuation builder which can be used to specify additional
            /// <see cref="Requirement"/>s and eventually create an
            /// <see cref="IConstraint{TValue}"/>.
            /// </returns>
            IRequirementEnumerationBuilder<TValue> Fulfils(Requirement requirement);

            /// <summary>
            /// Add the <see cref="Requirement"/> to the builder and transitions it into
            /// the state where only <see cref="Requirement"/>s can be added and
            /// eventually have the the <see cref="IConstraint{TValue}"/> created.
            /// </summary>
            /// <param name="requirementProvider">
            /// A function that provides a <see cref="Requirement"/> that the
            /// AnyConstraint will return in case all none of the aggregated
            /// <see cref="IConstraint{TValue}"/>s are satisfied.
            /// </param>
            /// <returns>
            /// Returns a continuation builder which can be used to specify additional
            /// <see cref="Requirement"/>s and eventually create an
            /// <see cref="IConstraint{TValue}"/>.
            /// </returns>
            /// <remarks>
            /// The function can be used to lookup the requirement text in a resource
            /// dictionary and return a localized version of it.
            /// </remarks>
            IRequirementEnumerationBuilder<TValue> Fulfils(RequirementProvider requirementProvider);

            /// <summary>
            /// Add the <see cref="Requirement"/> to the builder and transitions it into
            /// the state where only <see cref="Requirement"/>s can be added and
            /// eventually have the the <see cref="IConstraint{TValue}"/> created.
            /// </summary>
            /// <param name="requirementsProvider">
            /// A function that provides a number of <see cref="Requirement"/> that the
            /// AnyConstraint will return in case all none of the aggregated
            /// <see cref="IConstraint{TValue}"/>s are satisfied.
            /// </param>
            /// <returns>
            /// Returns a continuation builder which can be used to specify additional
            /// <see cref="Requirement"/>s and eventually create an
            /// <see cref="IConstraint{TValue}"/>.
            /// </returns>
            /// <remarks>
            /// The function can be used to lookup the text of each requirement in a
            /// resource dictionary and return a localized versions for them.
            /// </remarks>
            IRequirementEnumerationBuilder<TValue> Fulfils(RequirementsProvider requirementsProvider);
        }

        /// <summary>
        /// Represents a continuation builder to which more <see cref="Requirement"/>s
        /// can be added and eventuallu have an <see cref="IConstraint{TValue}"/>
        /// created.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface IRequirementEnumerationBuilder<TValue>
        {
            /// <summary>
            /// Add the <see cref="Requirement"/> to the builder and transitions it into
            /// the state where only <see cref="Requirement"/>s can be added and
            /// eventually have the the <see cref="IConstraint{TValue}"/> created.
            /// </summary>
            /// <param name="requirement">
            /// A <see cref="Requirement"/> that the AnyConstraint will return in case
            /// all none of the aggregated <see cref="IConstraint{TValue}"/>s are
            /// satisfied.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "And")]
            IRequirementEnumerationBuilder<TValue> And(Requirement requirement);

            /// <summary>
            /// Add the <see cref="Requirement"/> to the builder and transitions it into
            /// the state where only <see cref="Requirement"/>s can be added and
            /// eventually have the the <see cref="IConstraint{TValue}"/> created.
            /// </summary>
            /// <param name="requirementProvider">
            /// A function that provides a <see cref="Requirement"/> that the
            /// AnyConstraint will return in case all none of the aggregated
            /// <see cref="IConstraint{TValue}"/>s are satisfied.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            /// <remarks>
            /// The function can be used to lookup the requirement text in a resource
            /// dictionary and return a localized version of it.
            /// </remarks>
            [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "And")]
            IRequirementEnumerationBuilder<TValue> And(RequirementProvider requirementProvider);

            /// <summary>
            /// Add the <see cref="Requirement"/> to the builder and transitions it into
            /// the state where only <see cref="Requirement"/>s can be added and
            /// eventually have the the <see cref="IConstraint{TValue}"/> created.
            /// </summary>
            /// <param name="requirementsProvider">
            /// A function that provides a number of <see cref="Requirement"/> that the
            /// AnyConstraint will return in case all none of the aggregated
            /// <see cref="IConstraint{TValue}"/>s are satisfied.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            /// <remarks>
            /// The function can be used to lookup the text of each requirement in a
            /// resource dictionary and return a localized versions for them.
            /// </remarks>
            [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "And")]
            IRequirementEnumerationBuilder<TValue> And(RequirementsProvider requirementsProvider);

            /// <summary>
            /// Constructs an AnyConstraint from the provided
            /// <see cref="IConstraint{TValue}"/>s and <see cref="Requirement"/>s.
            /// </summary>
            /// <returns>
            /// Returns an <see cref="IConstraint{TValue}"/> implementation for the
            /// AnyConstraint specification.
            /// </returns>
            IConstraint<TValue> AsOneConstraint();
        }

        private static readonly IEnumerable<Requirement> _defaultRequirements =
            Enumerable.Repeat(new Requirement("The is an error"), 1);

        private static IConstraint<TValue> _GetConstraintFrom<TValue>(Predicate<TValue> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Constraint.From<TValue>(
                value => predicate(value)
                ? Enumerable.Empty<Requirement>()
                : _defaultRequirements);
        }
        private static IConstraint<TValue> _GetConstraintFrom<TValue>(AsyncPredicate<TValue> asyncPredicate)
        {
            if (asyncPredicate == null)
                throw new ArgumentNullException(nameof(asyncPredicate));

            return Constraint.From<TValue>(
                async (value, cancellationToken) =>
                {
                    var task = asyncPredicate(value, cancellationToken);
                    if (task == null)
                        return Enumerable.Empty<Requirement>();

                    return await task
                        ? Enumerable.Empty<Requirement>()
                        : _defaultRequirements;
                });
        }

        private sealed class AnyConstraintBuilder<TValue>
            : IConstraintEnumerationBuilder<TValue>, IRequirementEnumerationBuilder<TValue>
        {
            private sealed class AnyConstraint
                : Constraint<TValue>
            {
                private readonly IEnumerable<IConstraint<TValue>> _constraints;
                private readonly RequirementsProvider _requirementsProvider;

                public AnyConstraint(IEnumerable<IConstraint<TValue>> constraints, RequirementsProvider requirementsProvider)
                {
                    if (constraints == null)
                        throw new ArgumentNullException(nameof(constraints));
                    if (constraints.Any(constraint => constraint == null))
                        throw new ArgumentException(
                            "Must not contain any null constraints",
                            nameof(constraints));

                    if (requirementsProvider == null)
                        throw new ArgumentNullException(nameof(requirementsProvider));

                    _constraints = constraints;
                    _requirementsProvider = requirementsProvider;
                }

                protected override async Task<IEnumerable<Requirement>> OnCheckAsync(TValue value, CancellationToken cancellationToken)
                {
                    if (await _HasProblem(value, cancellationToken))
                    {
                        var requirements = _requirementsProvider();

                        if (requirements == null)
                            return Enumerable.Empty<Requirement>();
                        else
                            return requirements.Where(requirement => requirement != null);
                    }
                    else
                        return Enumerable.Empty<Requirement>();
                }

                private async Task<bool> _HasProblem(TValue value, CancellationToken cancellationToken)
                {
                    var hasProblem = true;

                    using (var constraint = _constraints.GetEnumerator())
                        while (hasProblem && constraint.MoveNext())
                        {
                            var task = constraint.Current.CheckAsync(value, cancellationToken);
                            if (task == null)
                                hasProblem = false;
                            else
                            {
                                var requirements = await task;
                                hasProblem = requirements.Any();
                            }
                        }

                    return hasProblem;
                }
            }

            private readonly ICollection<IConstraint<TValue>> _constraints;
            private readonly IList<RequirementsProvider> _requirementsProviders;

            public AnyConstraintBuilder(IConstraint<TValue> constraint)
            {
                _constraints = new List<IConstraint<TValue>>();
                _requirementsProviders = new List<RequirementsProvider>();

                _Add(constraint);
            }
            private void _Add(IConstraint<TValue> constraint)
            {
                if (constraint == null)
                    throw new ArgumentNullException(nameof(constraint));

                _constraints.Add(constraint);
            }

            public IConstraintEnumerationBuilder<TValue> Or(IConstraint<TValue> constraint)
            {
                _Add(constraint);
                return this;
            }
            public IConstraintEnumerationBuilder<TValue> Or(Predicate<TValue> predicate)
                => Or(_GetConstraintFrom(predicate));
            public IConstraintEnumerationBuilder<TValue> Or(AsyncPredicate<TValue> asyncPredicate)
                => Or(_GetConstraintFrom(asyncPredicate));

            public IRequirementEnumerationBuilder<TValue> Fulfils(Requirement requirement)
            {
                if (requirement == null)
                    throw new ArgumentNullException(nameof(requirement));

                _requirementsProviders.Add(() => Enumerable.Repeat(requirement, 1));
                return this;
            }
            public IRequirementEnumerationBuilder<TValue> And(Requirement requirement)
                => Fulfils(requirement);

            public IRequirementEnumerationBuilder<TValue> Fulfils(RequirementProvider requirementProvider)
            {
                if (requirementProvider == null)
                    throw new ArgumentNullException(nameof(requirementProvider));

                _requirementsProviders.Add(() => Enumerable.Repeat(requirementProvider(), 1));
                return this;
            }
            public IRequirementEnumerationBuilder<TValue> And(RequirementProvider requirementProvider)
                => Fulfils(requirementProvider);

            public IRequirementEnumerationBuilder<TValue> Fulfils(RequirementsProvider requirementsProvider)
            {
                if (requirementsProvider == null)
                    throw new ArgumentNullException(nameof(requirementsProvider));

                _requirementsProviders.Add(requirementsProvider);
                return this;
            }
            public IRequirementEnumerationBuilder<TValue> And(RequirementsProvider requirementsProvider)
                => Fulfils(requirementsProvider);

            public IConstraint<TValue> AsOneConstraint()
                => new AnyConstraint(
                    _constraints,
                    () => _requirementsProviders
                        .SelectMany(requirementsProvider =>
                        {
                            var requirements = requirementsProvider();

                            if (requirements == null)
                                return Enumerable.Empty<Requirement>();

                            return requirements
                                .Where(requirement => requirement != null)
                                .ToList();
                        })
                        .ToList());
        }
    }
}