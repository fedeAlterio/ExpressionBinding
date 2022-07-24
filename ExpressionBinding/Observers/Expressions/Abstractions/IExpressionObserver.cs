using ExpressionBinding.Observers.Members.Abstractions;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Expressions.Abstractions
{
    public interface IExpressionObserver<TSource>
    {
        IExpressionObserver<TSource> WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression);
        IExpressionObserver<TSource> With<TBindingStrategy>() where TBindingStrategy : IMemberBindingObserver, new();
        IExpressionObserver<TSource> With<TBindingStrategy>(TBindingStrategy bindingStrategy)
            where TBindingStrategy : IMemberBindingObserver;
        IDisposable Do(Action onExpressionChanged);
    }
}
