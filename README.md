# ExpressionBinding
Let's start with the result. Those tests pass:

```C#
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
```

# How does it work?

Let's start from the beginning. WhenChanged is declared like this

``` c#
public static IExpressionChangedBuilder<TSource> WhenChanged<TSource, TValue>(this TSource @this, Expression<Func<TSource, TValue>> expression)
{
    IExpressionChangedBuilder<TSource> ret = new EarlySourceBindingExpression<TSource>(@this);
    return ret.WhenChanged(expression);
}
```
## So now the point is what does EarlySourceBindingExpression do?

You can see that this class expect a parameter for be built `(@this)`. This will be the object from which start to "search" for properties to bind to.

The second parameter of the extension method is an expression.

You can think at it as the lambda: `h => h.MainRoom.Table` , considered not as a function, but as a collection (a tree) of all the "parts" on which the lambda is composed. So think at it as {h, h.MainRoom, h.MainRoom.Table}. Every part of the expression, so every element in {h, h.MainRoom, h.MainRoom.Table}, is an object containing all the metadata needed to fully classify itself. So if we put a breakpoint, exploring the expression we can see that:

* `h` is an instance of `ParameterExpression`
* `h.MainRoom` is an instance of `MemberExpression` (Member = Property)
* `h.MainRoom.Table` is an instance of `MemberExpression` too.
What we need to understand is that a `MemberExpression` class has a field called Member. The latter contains all the information about the member it is representing (the one to the rightmost), and between these there is also the Name. In our case so we can then obtain the strings `"MainRoom"` and `"Table"` from h.MainRoom and h.MainRoom.Table respectively.

## And now that we have the property names?

Now that we have the strings "MainRoom" and "Table" we need just to subscribe to the NotifyPropertyChanged event of h and h.MainRoom respectively, and listen for changes of properties caleed "MainRoom" or "Table". If that's the case we just invoke a callback.

## What if `h.MainRoom` is newed up?

If h.MainRoom changes, that means that we have to unsubsrice to all old nested properties (the old h.MainRoom), and resubscribe to all the new ones. This is done automatically.

## What if I need to unsubscribe completely from all?

When you subscribe you have returned back an IDisposable, that if called automatically unsubscribe from everything for you, releasing all the events.

```c#
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
        .With<propertychangeobserver>()
        .Do(() => counter++);

    subscription.Dispose();
    house.MainRoom = new();
    counter.Should().Be(0);
}
```
