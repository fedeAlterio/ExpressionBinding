using ExpressionBinding.Utils;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ExpressionBinding.BindingStrategies
{
    public class PropertyChangeObserver : TypedBindingBindingObserver<INotifyPropertyChanged>
    {
        protected override IDisposable? Subscribe(INotifyPropertyChanged bindingSource, MemberExpression? memberExpression, Action onChanged)
        {
            if (memberExpression is null)
                return null;

            void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == memberExpression.Member.Name)
                    onChanged();
            }

            bindingSource.PropertyChanged += OnPropertyChanged;
            void UnSubscribe() => bindingSource.PropertyChanged -= OnPropertyChanged;
            return DisposableAction.OnDisposeDo(UnSubscribe);
        }
    }
}
