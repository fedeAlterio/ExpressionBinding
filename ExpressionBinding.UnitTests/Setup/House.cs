using System.Collections.ObjectModel;

namespace ExpressionBinding.UnitTests.Setup
{
    public class House : BindableBase
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }


        private Room? _mainRoom;
        public Room? MainRoom
        {
            get => _mainRoom;
            set => SetProperty(ref _mainRoom, value);
        }


        public readonly ObservableCollection<Room> Rooms = new();
    }
}
