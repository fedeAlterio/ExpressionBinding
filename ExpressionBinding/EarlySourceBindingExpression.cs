using ExpressionBinding.BindingStrategies;
using System.Linq.Expressions;

namespace ExpressionBinding
{
    public interface IExpressionChangedBuilder<TSource> : IBindingStrategiesSetup<TSource>
    {
        IExpressionChangedBuilder<TSource> WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression);
    }

    public interface IBindingStrategiesSetup<TSource> : IExpressionChangedSetup
    {
        IBindingStrategiesSetup<TSource> With<TBindingStrategy>() where TBindingStrategy : IMemberBindingObserver, new();
    }

    public interface IExpressionChangedSetup
    {
        IDisposable Do(Action onExpressionChanged);
    }

    public class EarlySourceBindingExpression<TSource> : BindingExpressionBase<TSource>, IExpressionChangedBuilder<TSource>
    {
        readonly TSource _source;

        public EarlySourceBindingExpression(TSource source)
        {
            _source = source;
        }

        IBindingStrategiesSetup<TSource> IBindingStrategiesSetup<TSource>.With<TBindingStrategy>()
        {
            AddBindingStrategy<TBindingStrategy>();
            return this;
        }

        public IDisposable Do(Action onExpressionChanged)
        {
            return base.Bind(_source, onExpressionChanged);
        }

        IExpressionChangedBuilder<TSource> IExpressionChangedBuilder<TSource>.WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression)
        {
            WhenChanged(expression);
            return this;
        }
    }
}
