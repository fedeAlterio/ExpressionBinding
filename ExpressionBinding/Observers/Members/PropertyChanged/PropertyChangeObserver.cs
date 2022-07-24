using ExpressionBinding.Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ExpressionBinding.Observers.Members.PropertyChanged
{
    public class PropertyChangeObserver : TypedMemberObserver<INotifyPropertyChanged>
    {
        private readonly PropertyChangedHandlersDictionary _propertyChangedHandlers;

        public PropertyChangeObserver(PropertyChangedHandlersDictionary propertyChangedHandlers)
        {
            _propertyChangedHandlers = propertyChangedHandlers;
        }
        public PropertyChangeObserver() : this(new()) { }

        protected override IDisposable? Subscribe(INotifyPropertyChanged bindingSource, MemberExpression? memberExpression, Action onChanged)
        {
            if (memberExpression is null)
                return null;

            var propertyName = memberExpression.Member.Name;
            Subscribe(bindingSource, propertyName, onChanged);
            return DisposableAction.OnDisposeDo(() => UnSubscribe(bindingSource, propertyName, onChanged));
        }

        private void Subscribe(INotifyPropertyChanged source, string propertyName, Action handler)
        {
            if (!_propertyChangedHandlers.TryGetValue(source, out var taggedAction))
            {
                taggedAction = new();
                _propertyChangedHandlers.TryAdd(source, taggedAction);
                source.PropertyChanged += OnPropertyChanged;
            }
            taggedAction.Add(propertyName, handler);
        }

        private void OnPropertyChanged(object? source, PropertyChangedEventArgs args)
        {
            if (args.PropertyName is null || source is null)
                return;

            if (!_propertyChangedHandlers.TryGetValue((source as INotifyPropertyChanged)!, out var handler))
                return;

            handler.Invoke(args.PropertyName);
        }

        private void UnSubscribe(INotifyPropertyChanged source, string propertyName, Action handler)
        {
            Debug.Assert(_propertyChangedHandlers.ContainsKey(source));
            var taggedAction = _propertyChangedHandlers[source];
            taggedAction.Remove(propertyName, handler);
            if (taggedAction.HandlersCount == 0)
            {
                _propertyChangedHandlers.TryRemove(source, out _);
                source.PropertyChanged -= OnPropertyChanged;
            }
        }
    }
}
