using System.Linq.Expressions;

namespace ExpressionBinding.Extensions
{
    public static class BindingExpressionExtensions
    {
        public static IExpressionChangedBuilder<TSource> WhenChanged<TSource, TValue>(this TSource @this,
            Expression<Func<TSource, TValue>> expression)
        {
            IExpressionChangedBuilder<TSource> ret = new EarlySourceBindingExpression<TSource>(@this);
            return ret.WhenChanged(expression);
        }
    }
}
