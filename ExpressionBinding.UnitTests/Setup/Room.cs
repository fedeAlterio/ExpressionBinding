namespace ExpressionBinding.UnitTests.Setup
{
    public class Room : BindableBase
    {
        private Table? _table;
        public Table? Table
        {
            get => _table;
            set => SetProperty(ref _table, value);
        }
    }
}
