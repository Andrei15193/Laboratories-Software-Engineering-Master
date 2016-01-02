using System;
using System.Collections.Generic;
using System.Linq;
using JJC.Psharp.Lang;

namespace FoodRecipe.Helpers
{
    public static class PrologHelper
    {
        public static IEnumerable<TPredicate> FindAll<TPredicate>(params object[] args)
            where TPredicate : Predicate, new()
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            var prologInterface = new PrologInterface();
            prologInterface.AddCallingAssembly();

            var predicate = new TPredicate();

            if (args.Length != predicate.arity())
                throw new ArgumentException("The number of args does not match the arity of the predicate.", nameof(args));

            predicate.setArgument(
                args.Select(arg => arg as Term ?? new CsObjectTerm(arg)).ToArray(),
                new ReturnCs(prologInterface));
            prologInterface.SetPredicate(predicate);

            if (prologInterface.Call())
                do
                    yield return predicate;
                while (prologInterface.Redo());
        }

        public static IEnumerable<object> FindAll<TPredicate>(Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>().Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3)
                .Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).Select(predicate => _ExtractTerm(selector(predicate)));
        }
        public static IEnumerable<object> FindAll<TPredicate>(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9, object arg10, object arg11, object arg12, object arg13, object arg14, object arg15, object arg16, Func<TPredicate, object> selector)
            where TPredicate : Predicate, new()
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return FindAll<TPredicate>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).Select(predicate => _ExtractTerm(selector(predicate)));
        }

        private static object _ExtractTerm(object obj)
            => (obj as Term)?.ToCsObject() ?? obj;
    }
}