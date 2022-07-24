using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressionBinding.FluentProxies.Abstractions;
using ExpressionBinding.Observers.Expressions;
using ExpressionBinding.Observers.Expressions.Abstractions;

namespace ExpressionBinding.FluentProxies
{
    public class FluentSingleExpressionProxy<TSource, TValue> : FluentExpressionObserverBuilder<TSource>, IFluentSingleExpressionProxy<TSource, TValue>
    {
        private readonly Expression<Func<TSource, TValue>> _expression;

        public FluentSingleExpressionProxy(TSource source, Expression<Func<TSource, TValue>> expression) : base(source)
        {
            _expression = expression;
            (this as IExpressionsObserver<TSource>).WhenChanged(expression);
        }

        public IFluentOneWayBinding<TValue> Bind()
        {
            return new FluentOneWayBinding<TSource, TValue>(_source, _expression);
        }
    }
}
