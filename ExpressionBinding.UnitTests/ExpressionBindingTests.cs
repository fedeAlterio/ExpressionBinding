using ExpressionBinding.Observers.Members.CollectionChanged;
using ExpressionBinding.UnitTests.Setup;
using ExpressionBinding.UnitTests.Setup.Extensions;
using FluentAssertions;

namespace ExpressionBinding.UnitTests
{
    public class ExpressionBindingTests
    {
        [Fact]
        public void ShouldNotifyCorrectlyIfPropertyChange()
        {
            var house = new House();
            bool nameChanged = false;
            house.WhenPropertyChanged(h => h.Name)
                .Do(() => nameChanged = true);

            house.Name = "Name";
            nameChanged.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotifyCorrectlyIfNestedPropertyChange()
        {
            var house = new House();
            int counter = 0;
            house
                .WhenPropertyChanged(h => h.MainRoom.Table)
                .WhenChanged(h => h.Name)
                .Do(() => counter++);

            house.MainRoom = new();
            house.MainRoom.Table = new();
            house.Name = "NewName";
            counter.Should().Be(3);
        }

        [Fact]
        public void ShouldNotNotifyIfPropertyChangesOnOldReference()
        {
            var house = new House
            {
                MainRoom = new()
                {
                    Table = new()
                }
            };

            var counter = 0;
            house
                .WhenPropertyChanged(h => h.MainRoom.Table)
                .Do(() => counter++);

            var oldRoom = house.MainRoom;
            house.MainRoom = new();
            oldRoom.Table = new();
            counter.Should().Be(1);
        }

        [Fact]
        public void ShouldNotNotifyIfDisposed()
        {
            var house = new House
            {
                MainRoom = new Room
                {
                    Table = new()
                }
            };

            var counter = 0;
            var subscription = house
                .WhenPropertyChanged(h => h.MainRoom.Table)
                .Do(() => counter++);

            subscription.Dispose();
            house.MainRoom = new();
            counter.Should().Be(0);
        }

        [Fact]
        public void ShouldNotifyCollectionChanges()
        {
            var house = new House
            {
                Rooms = { new() }
            };

            var counter = 0;
            var subscription = house
                .WhenPropertyChanged(h => h.Rooms.FirstOrDefault())
                .With<CollectionChangedObserver>()
                .Do(() => counter++);

            house.Rooms.Clear();
            house.Rooms.Add(new());
            subscription.Dispose();
            house.Rooms.Clear();
            counter.Should().Be(2);
        }
    }
}