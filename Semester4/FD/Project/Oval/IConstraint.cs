using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Oval
{
    /// <summary>
    /// Represents the interface of a constraint.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the object that will be checked.
    /// </typeparam>
    public interface IConstraint<in TValue>
    {
        /// <summary>
        /// Checks whether the provided object satisfies the constraint. If not a number
        /// of requirements are returned which must be fulfiled by the object in order
        /// for the constraint to be satisfied.
        /// </summary>
        /// <param name="value">
        /// The object to check.
        /// </param>
        /// <returns>
        /// Returns a collection of <see cref="Requirement"/>s that must be
        /// satisfied by the object in order for the constraint to be satisfied.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IEnumerable<Requirement>> CheckAsync(TValue value);

        /// <summary>
        /// Checks whether the provided object satisfies the constraint. If not a number
        /// of requirements are returned which must be fulfiled by the object in order
        /// for the constraint to be satisfied.
        /// </summary>
        /// <param name="value">
        /// The object to check.
        /// </param>
        /// <param name="cancellationToken">
        /// A token that can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// Returns a collection of <see cref="Requirement"/>s that must be
        /// satisfied by the object in order for the constraint to be satisfied.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IEnumerable<Requirement>> CheckAsync(TValue value, CancellationToken cancellationToken);
    }
}