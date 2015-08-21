using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillPath.Models
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.ivalidatableobject.validate(v=vs.110).aspx
    /// </summary>
    public interface IValidatableObject
    {
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}