using System.Linq.Expressions;
using ExpressionBinding.Observers.Expressions;
using ExpressionBinding.Observers.Members.Abstractions;

namespace ExpressionBinding.BindingStrategies
{
    public class OneWayBinding<TSource, TBindingSource>
    {
        private readonly Func<TSource, TBindingSource> _sourceFunc;
        private readonly Action<TBindingSource?> _onSourceExpressionChanged;
        private readonly TBindingSource? _fallbackValue;
        private readonly ExpressionObserver<TSource> _expressionObserver;


        public OneWayBinding(Expression<Func<TSource, TBindingSource>> sourceExpression,
            Action<TBindingSource?> onSourceExpressionChanged, TBindingSource? fallbackValue = default)
        {
            _sourceFunc = sourceExpression.Compile();
            _onSourceExpressionChanged = onSourceExpressionChanged;
            _fallbackValue = fallbackValue;
            _expressionObserver = new();
            _expressionObserver.Observe(sourceExpression);
        }

        public void AddObservationStrategy<TBindingStrategy>(TBindingStrategy bindingStrategy) where TBindingStrategy : IMemberBindingObserver
        {
            _expressionObserver.AddObservationStrategy(bindingStrategy);
        }

        public IDisposable Bind(TSource source) => 
            _expressionObserver.Bind(source,
                () => _onSourceExpressionChanged.Invoke(GetValueOrFallback(source)));

        private TBindingSource? GetValueOrFallback(TSource source)
        {
            try
            {
                return _sourceFunc.Invoke(source);
            }
            catch
            {
                return _fallbackValue;
            }
        }
    }
}
