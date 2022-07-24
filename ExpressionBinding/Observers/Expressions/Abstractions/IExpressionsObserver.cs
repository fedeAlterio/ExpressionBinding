using ExpressionBinding.Observers.Members.Abstractions;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Expressions.Abstractions
{
    public interface IExpressionsObserver<TSource> 
    {
        IExpressionsObserver<TSource> WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression);
        IExpressionsObserver<TSource> With<TBindingStrategy>() where TBindingStrategy : IMemberBindingObserver, new();
        IExpressionsObserver<TSource> With<TBindingStrategy>(TBindingStrategy bindingStrategy)
            where TBindingStrategy : IMemberBindingObserver;
        IDisposable Do(Action onExpressionChanged);
    }
}
