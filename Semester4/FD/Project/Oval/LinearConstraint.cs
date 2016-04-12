using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oval
{
    /// <summary>
    /// Represents a sequence of <see cref="IConstraint{TValue}"/> where each is checked
    /// in the same order they are added. When checking whether all constraints are
    /// satisfied, a collection containing <see cref="Requirement"/>s from all
    /// unsatisfied constraints is returned.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The LinearConstraint may not check all aggregated
    /// <see cref="IConstraint{TValue}"/> if
    /// <see cref="ILinearConstraintBuilder{TValue}.CheckedAndFollowedBy(IConstraint{TValue})"/>
    /// or
    /// <see cref="ILinearConstraintBuilder{TValue}.CheckedAndEndedWith(IConstraint{TValue})"/>
    /// have been called. The purpose of these methods is to separate
    /// <see cref="IConstraint{TValue}"/>s in batches where each batch is checked one at
    /// a time. Once a batch that contains <see cref="IConstraint{TValue}"/>s that are
    /// not satisfied the check is stopped at the end of that batch and any
    /// <see cref="Requirement"/> that has been returned thus far is returned by the
    /// linear constraint.
    /// </para>
    /// <para>
    /// This is helpful when there is a set of <see cref="IConstraint{TValue}"/>s that
    /// validate a model in-memory and then there is another set of
    /// <see cref="IConstraint{TValue}"/> that check whether the provided model outside
    /// of the application (e.g.: uniqueness of a username within a database).
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// The LinearConstraint is useful in grouping sequences of
    /// <see cref="IConstraint{TValue}"/>s in batches and then checking each batch to see
    /// if the constraints in that batch are satisfied. If a batch contains unsatisfied
    /// constraints then no further batches are checked and all
    /// <see cref="Requirement"/>s that must be meet are returned.
    /// </para>
    /// <code>
    /// var constraint = LinearConstraint
    ///     .StartingWith&lt;string&gt;(
    ///         value =&gt;
    ///         {
    ///             if (value != null)
    ///                 return Enumerable.Empty&lt;Requirement&gt;();
    /// 
    ///             return new[] { new Requirement("Cannot be null") };
    ///         })
    ///     .CheckedAndFollowedBy(
    ///         value =&gt;
    ///         {
    ///             if (value.Length &lt; 255)
    ///                 return Enumerable.Empty&lt;Requirement&gt;();
    /// 
    ///             return new[] { new Requirement("Cannot exceed 255 characters") };
    ///         })
    ///     .AndEndedWith(
    ///         value =&gt;
    ///         {
    ///             if (value.StartsWith("Test"))
    ///                 return Enumerable.Empty&lt;Requirement&gt;();
    /// 
    ///             return new[] { new Requirement("Must start with \"Test\"") };
    ///         });
    /// 
    /// var requirements = await constraint.CheckAsync("not valid");
    /// 
    /// Assert.AreEqual(
    ///     "Must start with \"Test\"",
    ///     requirements.Single().Text,
    ///     ignoreCase: false);
    /// </code>
    /// </example>
    public static class LinearConstraint
    {
        /// <summary>
        /// Represents the interface of a LinearConstraint builder.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public interface ILinearConstraintBuilder<TValue>
        {
            /// <summary>
            /// Adds a <see cref="IConstraint{TValue}"/> to the current batch.
            /// </summary>
            /// <param name="constraint">
            /// The <see cref="IConstraint{TValue}"/> to add.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            ILinearConstraintBuilder<TValue> FollowedBy(IConstraint<TValue> constraint);

            /// <summary>
            /// Creates an <see cref="IConstraint{TValue}"/> from the delegate and adds
            /// it to the current batch.
            /// </summary>
            /// <param name="callback">
            /// The callback that checks which requirements must be meet for a given
            /// value.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            ILinearConstraintBuilder<TValue> FollowedBy(Constraint.Callback<TValue> callback);

            /// <summary>
            /// Creates an <see cref="IConstraint{TValue}"/> from the delegate and adds
            /// it to the current batch.
            /// </summary>
            /// <param name="asyncCallback">
            /// The callback that asynchronously checks which requirements must be meet
            /// for a given value.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            ILinearConstraintBuilder<TValue> FollowedBy(Constraint.AsyncCallback<TValue> asyncCallback);

            /// <summary>
            /// Ends the current batch and starts a new one adding the provided
            /// <see cref="IConstraint{TValue}"/> to it.
            /// </summary>
            /// <param name="constraint">
            /// The <see cref="IConstraint{TValue}"/> to add to the new batch.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            ILinearConstraintBuilder<TValue> CheckedAndFollowedBy(IConstraint<TValue> constraint);

            /// <summary>
            /// Ends the current batch and starts a new one adding an
            /// <see cref="IConstraint{TValue}"/> that was created from the callback.
            /// </summary>
            /// <param name="callback">
            /// The callback that checks which requirements must be meet for a given
            /// value.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            ILinearConstraintBuilder<TValue> CheckedAndFollowedBy(Constraint.Callback<TValue> callback);

            /// <summary>
            /// Ends the current batch and starts a new one adding an
            /// <see cref="IConstraint{TValue}"/> that was created from the async
            /// callback.
            /// </summary>
            /// <param name="asyncCallback">
            /// The callback that asynchronously checks which requirements must be meet
            /// for a given value.
            /// </param>
            /// <returns>
            /// Returns the builder whose method has been called.
            /// </returns>
            ILinearConstraintBuilder<TValue> CheckedAndFollowedBy(Constraint.AsyncCallback<TValue> asyncCallback);

            /// <summary>
            /// Adds the provided constraint and constructs a linear constraint.
            /// </summary>
            /// <param name="constraint">
            /// The <see cref="IConstraint{TValue}"/> which is the last in the sequence.
            /// </param>
            /// <returns>
            /// Returns an <see cref="IConstraint{TValue}"/> for the linear constraint.
            /// </returns>
            IConstraint<TValue> AndEndedWith(IConstraint<TValue> constraint);

            /// <summary>
            /// Adds a last <see cref="IConstraint{TValue}"/> created from the provided
            /// callback and constructs a linear constraint.
            /// </summary>
            /// <param name="callback">
            /// The callback that checks which requirements must be meet for a given
            /// value.
            /// </param>
            /// <returns>
            /// Returns an <see cref="IConstraint{TValue}"/> for the linear constraint.
            /// </returns>
            IConstraint<TValue> AndEndedWith(Constraint.Callback<TValue> callback);

            /// <summary>
            /// Adds a last <see cref="IConstraint{TValue}"/> created from the provided
            /// async callback and constructs a linear constraint.
            /// </summary>
            /// <param name="asyncCallback">
            /// The callback that asynchronously checks which requirements must be meet
            /// for a given value.
            /// </param>
            /// <returns>
            /// Returns an <see cref="IConstraint{TValue}"/> for the linear constraint.
            /// </returns>
            IConstraint<TValue> AndEndedWith(Constraint.AsyncCallback<TValue> asyncCallback);

            /// <summary>
            /// Adds a last batch containing just the provided
            /// <see cref="IConstraint{TValue}"/> and then constructs a linear
            /// constraint.
            /// </summary>
            /// <param name="constraint">
            /// The <see cref="IConstraint{TValue}"/> which is in the last batch.
            /// </param>
            /// <returns>
            /// Returns an <see cref="IConstraint{TValue}"/> for the linear constraint.
            /// </returns>
            IConstraint<TValue> CheckedAndEndedWith(IConstraint<TValue> constraint);

            /// <summary>
            /// Adds a last batch containing just an <see cref="IConstraint{TValue}"/>
            /// created from the provided callback and constructs a linear constraint.
            /// </summary>
            /// <param name="callback">
            /// The callback that checks which requirements must be meet for a given
            /// value.
            /// </param>
            /// <returns>
            /// Returns an <see cref="IConstraint{TValue}"/> for the linear constraint.
            /// </returns>
            IConstraint<TValue> CheckedAndEndedWith(Constraint.Callback<TValue> callback);

            /// <summary>
            /// Adds a last batch containing just an <see cref="IConstraint{TValue}"/>
            /// created from the provided async callback and constructs a linear
            /// constraint.
            /// </summary>
            /// <param name="asyncCallback">
            /// The callback that asynchronously checks which requirements must be meet
            /// for a given value.
            /// </param>
            /// <returns>
            /// Returns an <see cref="IConstraint{TValue}"/> for the linear constraint.
            /// </returns>
            IConstraint<TValue> CheckedAndEndedWith(Constraint.AsyncCallback<TValue> asyncCallback);
        }

        /// <summary>
        /// Creates a LinearConstraint builder containing the provided
        /// <see cref="IConstraint{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="constraint">
        /// The <see cref="IConstraint{TValue}"/> with which to initialize the builder
        /// with.
        /// </param>
        /// <returns>
        /// Returns a LinearConstraint builder that can be used to create a
        /// LinearConstraint.
        /// </returns>
        public static ILinearConstraintBuilder<TValue> StartingWith<TValue>(IConstraint<TValue> constraint)
            => new LinearConstraintBuilder<TValue>(constraint);

        /// <summary>
        /// Creates a LinearConstraint builder containing an
        /// <see cref="IConstraint{TValue}"/> that has been created from the provided
        /// callback.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="callback">
        /// The callback that checks which requirements must be meet for a given value.
        /// </param>
        /// <returns>
        /// Returns a LinearConstraint builder that can be used to create a
        /// LinearConstraint.
        /// </returns>
        public static ILinearConstraintBuilder<TValue> StartingWith<TValue>(Constraint.Callback<TValue> callback)
            => new LinearConstraintBuilder<TValue>(Constraint.From(callback));

        /// <summary>
        /// Creates a LinearConstraint builder containing an
        /// <see cref="IConstraint{TValue}"/> that has been created from the provided
        /// async callback.
        /// </summary>
        /// <typeparam name="TValue">
        /// The type of the object that will be checked.
        /// </typeparam>
        /// <param name="asyncCallback">
        /// The callback that asynchronously checks which requirements must be meet for a
        /// given value.
        /// </param>
        /// <returns>
        /// Returns a LinearConstraint builder that can be used to create a
        /// LinearConstraint.
        /// </returns>
        public static ILinearConstraintBuilder<TValue> StartingWith<TValue>(Constraint.AsyncCallback<TValue> asyncCallback)
            => new LinearConstraintBuilder<TValue>(Constraint.From(asyncCallback));

        private sealed class LinearConstraintBuilder<TValue>
            : ILinearConstraintBuilder<TValue>
        {
            private sealed class LinearConstraint
                : Constraint<TValue>
            {
                private readonly IEnumerable<IEnumerable<IConstraint<TValue>>> _constraintBatches;

                public LinearConstraint(IEnumerable<IEnumerable<IConstraint<TValue>>> constraintBatches)
                {
                    if (constraintBatches == null)
                        throw new ArgumentNullException(nameof(constraintBatches));

                    _constraintBatches = constraintBatches;
                }

                protected override async Task<IEnumerable<Requirement>> OnCheckAsync(TValue value, CancellationToken cancellationToken)
                {
                    var requirements = Enumerable.Empty<Requirement>();

                    using (var constraintBatch = _constraintBatches.GetEnumerator())
                        while (!requirements.Any() && constraintBatch.MoveNext())
                        {
                            requirements = await _GetRequirementsFrom(
                                constraintBatch.Current,
                                value,
                                cancellationToken);
                            cancellationToken.ThrowIfCancellationRequested();
                        }

                    return requirements;
                }
                private static async Task<IEnumerable<Requirement>> _GetRequirementsFrom(IEnumerable<IConstraint<TValue>> constraints, TValue value, CancellationToken cancellationToken)
                {
                    var requirements = new List<Requirement>(0);

                    foreach (var constraint in constraints)
                        requirements.AddRange(await constraint.CheckAsync(value, cancellationToken));

                    return requirements;
                }
            }

            private List<Queue<IConstraint<TValue>>> _constraintBatches;

            public LinearConstraintBuilder(IConstraint<TValue> constraint)
            {
                _constraintBatches = new List<Queue<IConstraint<TValue>>> { new Queue<IConstraint<TValue>>() };
                _Add(constraint);
            }

            public ILinearConstraintBuilder<TValue> FollowedBy(IConstraint<TValue> constraint)
            {
                _Add(constraint);
                return this;
            }

            public ILinearConstraintBuilder<TValue> FollowedBy(Constraint.Callback<TValue> callback)
                => FollowedBy(Constraint.From(callback));

            public ILinearConstraintBuilder<TValue> FollowedBy(Constraint.AsyncCallback<TValue> asyncCallback)
                => FollowedBy(Constraint.From(asyncCallback));

            public ILinearConstraintBuilder<TValue> CheckedAndFollowedBy(IConstraint<TValue> constraint)
            {
                _constraintBatches.Add(new Queue<IConstraint<TValue>>());
                return FollowedBy(constraint);
            }

            public ILinearConstraintBuilder<TValue> CheckedAndFollowedBy(Constraint.Callback<TValue> callback)
                => CheckedAndFollowedBy(Constraint.From(callback));

            public ILinearConstraintBuilder<TValue> CheckedAndFollowedBy(Constraint.AsyncCallback<TValue> asyncCallback)
                => CheckedAndFollowedBy(Constraint.From(asyncCallback));

            public IConstraint<TValue> AndEndedWith(IConstraint<TValue> constraint)
            {
                _Add(constraint);
                return new LinearConstraint(_constraintBatches);
            }

            public IConstraint<TValue> AndEndedWith(Constraint.Callback<TValue> callback)
                => AndEndedWith(Constraint.From(callback));

            public IConstraint<TValue> AndEndedWith(Constraint.AsyncCallback<TValue> asyncCallback)
                => AndEndedWith(Constraint.From(asyncCallback));

            public IConstraint<TValue> CheckedAndEndedWith(IConstraint<TValue> constraint)
            {
                _constraintBatches.Add(new Queue<IConstraint<TValue>>());
                return AndEndedWith(constraint);
            }

            public IConstraint<TValue> CheckedAndEndedWith(Constraint.Callback<TValue> callback)
                => CheckedAndEndedWith(Constraint.From(callback));

            public IConstraint<TValue> CheckedAndEndedWith(Constraint.AsyncCallback<TValue> asyncCallback)
                => CheckedAndEndedWith(Constraint.From(asyncCallback));

            private void _Add(IConstraint<TValue> constraint)
            {
                if (constraint == null)
                    throw new ArgumentNullException(nameof(constraint));

                _constraintBatches.Last().Enqueue(constraint);
            }
        }
    }
}