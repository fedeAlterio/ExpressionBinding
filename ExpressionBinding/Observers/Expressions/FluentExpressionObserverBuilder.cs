using ExpressionBinding.Observers.Expressions.Abstractions;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Expressions
{
    public class FluentExpressionObserverBuilder<TSource> : ExpressionObserver<TSource>, IExpressionsObserver<TSource>
    {
        protected readonly TSource _source;

        public FluentExpressionObserverBuilder(TSource source)
        {
            _source = source;
        }

        IExpressionsObserver<TSource> IExpressionsObserver<TSource>.With<TBindingStrategy>()
        {
            AddObservationStrategy<TBindingStrategy>();
            return this;
        }

        IExpressionsObserver<TSource> IExpressionsObserver<TSource>.With<TBindingStrategy>(TBindingStrategy bindingStrategy)
        {
            AddObservationStrategy(bindingStrategy);
            return this;
        }

        public IDisposable Do(Action onExpressionChanged)
        {
            return Bind(_source, onExpressionChanged);
        }

        IExpressionsObserver<TSource> IExpressionsObserver<TSource>.WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression)
        {
            Observe(expression);
            return this;
        }
    }
}
