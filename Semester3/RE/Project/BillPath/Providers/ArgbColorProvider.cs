using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BillPath.Models;
using Windows.UI;

namespace BillPath.Providers
{
    public class ArgbColorProvider
    {
        public IEnumerable<ArgbColor> ArgbColors { get; } =
            (from runtimeProperty in typeof(Colors).GetRuntimeProperties()
             where typeof(Color).Equals(runtimeProperty.PropertyType)
                 && (runtimeProperty.GetMethod?.IsPublic ?? false)
                 && (runtimeProperty.GetMethod?.IsStatic ?? false)
             let color = (Color)runtimeProperty.GetValue(null)
             select new ArgbColor(color.A, color.R, color.G, color.B))
            .ToList();
    }
}