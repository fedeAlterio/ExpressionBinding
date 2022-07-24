using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressionBinding.FluentProxies;
using ExpressionBinding.FluentProxies.Abstractions;

namespace ExpressionBinding.Extensions
{
    public static class ExpressionBindingFluentExtensions
    {
        public static IFluentSingleExpressionProxy<TSource, TValue> WhenChanged<TSource, TValue>(
            this TSource @this,
            Expression<Func<TSource, TValue>> expression)
        {
            return new FluentSingleExpressionProxy<TSource, TValue>(@this, expression);
        }
    }
}
