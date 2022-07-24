using ExpressionBinding.Observers.Expressions.Abstractions;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Expressions
{
    public class EarlySourceExpressionObserver<TSource> : ExpressionObserverBase<TSource>, IExpressionObserver<TSource>
    {
        readonly TSource _source;

        public EarlySourceExpressionObserver(TSource source)
        {
            _source = source;
        }

        IExpressionObserver<TSource> IExpressionObserver<TSource>.With<TBindingStrategy>()
        {
            AddBindingStrategy<TBindingStrategy>();
            return this;
        }

        IExpressionObserver<TSource> IExpressionObserver<TSource>.With<TBindingStrategy>(TBindingStrategy bindingStrategy)
        {
            AddBindingStrategy(bindingStrategy);
            return this;
        }

        public IDisposable Do(Action onExpressionChanged)
        {
            return base.Bind(_source, onExpressionChanged);
        }

        IExpressionObserver<TSource> IExpressionObserver<TSource>.WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression)
        {
            WhenChanged(expression);
            return this;
        }
    }
}
