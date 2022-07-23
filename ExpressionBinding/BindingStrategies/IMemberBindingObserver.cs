using System.Linq.Expressions;

namespace ExpressionBinding.BindingStrategies
{
    public interface IMemberBindingObserver
    {
        IDisposable? Subscribe(object bindingSource, MemberExpression? memberExpression, Action onChanged);
    }
}
