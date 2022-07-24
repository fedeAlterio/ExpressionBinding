using ExpressionBinding.Observers.Expressions;
using ExpressionBinding.Observers.Expressions.Abstractions;
using ExpressionBinding.Observers.Members.PropertyChanged;
using System.Linq.Expressions;

namespace ExpressionBinding.UnitTests.Setup.Extensions
{
    public static class BindingPropertyChangedBindingStrategyExtensions
    {
        public static IExpressionObserver<TSource> WhenPropertyChanged<TSource, TValue>(this TSource @this,
            Expression<Func<TSource, TValue>> expression)
        {
            IExpressionObserver<TSource> ret = new EarlySourceExpressionObserver<TSource>(@this);
            return ret
                .With<PropertyChangeObserver>()
                .WhenChanged(expression);
        }
    }
}
