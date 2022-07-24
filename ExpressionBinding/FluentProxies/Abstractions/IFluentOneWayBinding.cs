using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionBinding.Observers.Members.Abstractions;

namespace ExpressionBinding.FluentProxies.Abstractions
{
    public interface IFluentOneWayBinding<TBindingSource>
    {
        IFluentOneWayBinding<TBindingSource> With<TObserver>(TObserver bindingStrategy) where TObserver : IMemberBindingObserver;
        IDisposable To(Action<TBindingSource?> onSourceExpressionChanged, TBindingSource? fallbackValue = default);
    }
}
