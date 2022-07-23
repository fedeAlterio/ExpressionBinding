using ExpressionBinding.BindingStrategies;
using ExpressionBinding.Extensions;
using ExpressionBinding.UnitTests.Setup;
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
            house.WhenChanged(h => h.Name)
                .With<PropertyChangeObserver>()
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
                .WhenChanged(h => h.MainRoom.Table)
                .WhenChanged(h => h.Name)
                .With<PropertyChangeObserver>()
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
                .WhenChanged(h => h.MainRoom.Table)
                .With<PropertyChangeObserver>()
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
                .WhenChanged(h => h.MainRoom.Table)
                .With<PropertyChangeObserver>()
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
            var subscription = house.WhenChanged(h => h.Rooms.FirstOrDefault())
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