using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Members.Abstractions
{
    public interface IMemberBindingObserver
    {
        IDisposable? Subscribe(object bindingSource, MemberExpression? memberExpression, Action onChanged);
    }
}
