using ExpressionBinding.Observers.Members.Abstractions;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Members
{
    public abstract class TypedMemberObserver<T> : IMemberBindingObserver
    {
        public IDisposable? Subscribe(object bindingSource, MemberExpression? memberExpression, Action onChanged)
        {
            if (bindingSource is T t)
                return Subscribe(t, memberExpression, onChanged);
            return null;
        }
        protected abstract IDisposable? Subscribe(T bindingSource, MemberExpression? memberExpression, Action onChanged);
    }
}
