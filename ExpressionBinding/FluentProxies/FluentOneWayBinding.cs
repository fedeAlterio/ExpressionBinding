using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressionBinding.Observers.Members.Abstractions;
using ExpressionBinding.FluentProxies.Abstractions;
using ExpressionBinding.BindingStrategies;

namespace ExpressionBinding.FluentProxies
{
    public class FluentOneWayBinding<TSource, TBindingSource> : IFluentOneWayBinding<TBindingSource>
    {
        private readonly TSource _source;
        private readonly Expression<Func<TSource, TBindingSource>> _sourceExpression;
        private List<IMemberBindingObserver>? _observers = new();

        public FluentOneWayBinding(TSource source, Expression<Func<TSource, TBindingSource>> sourceExpression)
        {
            _source = source;
            _sourceExpression = sourceExpression;
        }

        public IFluentOneWayBinding<TBindingSource> Using<TObserver>(TObserver observer) where TObserver : IMemberBindingObserver
        {
            _observers!.Add(observer);
            return this;
        }

        public IDisposable To(Action<TBindingSource?> onSourceExpressionChanged, TBindingSource? fallbackValue = default)
        {
            var oneWayBinding = new OneWayBinding<TSource, TBindingSource>(_sourceExpression, onSourceExpressionChanged, fallbackValue);
            foreach (var observer in _observers!)
                oneWayBinding.AddObservationStrategy(observer);
            _observers = null;
            return oneWayBinding.Bind(_source);
        }
    }
}
