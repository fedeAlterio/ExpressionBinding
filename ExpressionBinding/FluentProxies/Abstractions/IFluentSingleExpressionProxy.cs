using ExpressionBinding.Observers.Expressions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionBinding.FluentProxies.Abstractions
{
    public interface IFluentSingleExpressionProxy<TSource, TValue> : IExpressionsObserver<TSource>
    {
        IFluentOneWayBinding<TValue> Bind();
    }
}
