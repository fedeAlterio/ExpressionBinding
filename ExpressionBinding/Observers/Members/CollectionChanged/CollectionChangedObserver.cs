using ExpressionBinding.Utils;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Members.CollectionChanged
{
    public class CollectionChangedObserver : TypedMemberObserver<INotifyCollectionChanged>
    {
        protected override IDisposable Subscribe(INotifyCollectionChanged collection, MemberExpression? memberExpression, Action onChanged)
        {
            void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            {
                onChanged.Invoke();
            }

            collection.CollectionChanged += OnCollectionChanged;
            return DisposableAction.OnDisposeDo(() => collection.CollectionChanged -= OnCollectionChanged);
        }
    }
}
