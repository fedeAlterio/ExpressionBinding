using System.Linq.Expressions;

namespace ExpressionBinding.BindingStrategies
{
    public abstract class TypedBindingBindingObserver<T> : IMemberBindingObserver
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
