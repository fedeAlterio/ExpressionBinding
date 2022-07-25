using ExpressionBinding.Observers.Members.Abstractions;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Expressions.Abstractions
{
    public interface IExpressionsObserver<TSource> 
    {
        IExpressionsObserver<TSource> WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression);
        IExpressionsObserver<TSource> Using<TBindingStrategy>() where TBindingStrategy : IMemberBindingObserver, new();
        IExpressionsObserver<TSource> Using<TBindingStrategy>(TBindingStrategy bindingStrategy)
            where TBindingStrategy : IMemberBindingObserver;
        IDisposable Do(Action onExpressionChanged);
    }
}
