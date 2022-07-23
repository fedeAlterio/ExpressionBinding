using ExpressionBinding.UnitTests.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExpressionBinding.UnitTests.Setup
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<TValue>(ref TValue valueReference, TValue value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<TValue>.Default.Equals(valueReference, value))
                return false;

            valueReference = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}