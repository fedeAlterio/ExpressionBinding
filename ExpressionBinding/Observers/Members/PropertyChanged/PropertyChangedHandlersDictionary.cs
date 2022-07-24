using ExpressionBinding.Utils;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace ExpressionBinding.Observers.Members.PropertyChanged
{
    public class PropertyChangedHandlersDictionary : ConcurrentDictionary<INotifyPropertyChanged, TaggedAction<string>>
    {
    }
}
