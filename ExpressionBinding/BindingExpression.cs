namespace ExpressionBinding;

public interface IBindingExpressionBinder<in TSource>
{
    IDisposable Bind(TSource source, Action onPropertyChanged);
}

public class BindingExpression<TSource> : BindingExpressionBase<TSource>, IBindingExpressionBinder<TSource>
{
    public new IDisposable Bind(TSource source, Action onPropertyChanged)
    {
        return base.Bind(source, onPropertyChanged);
    }
}