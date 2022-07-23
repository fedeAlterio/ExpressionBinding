using ExpressionBinding.BindingStrategies;
using ExpressionBinding.Utils;
using System.Linq.Expressions;

namespace ExpressionBinding;


public class BindingExpressionBase<TSource>
{
    public event Action? UnBound;
    private readonly List<Expression> _boundExpressions = new();
    private readonly List<IMemberBindingObserver> _bindingStrategies = new();

    protected void WhenChanged<TValue>(Expression<Func<TSource, TValue>> expression)
    {
        _boundExpressions.Add(expression);
    }


    protected void AddBindingStrategy<TBindingStrategy>() where TBindingStrategy : IMemberBindingObserver, new()
    {
        var strategy = new TBindingStrategy();
        _bindingStrategies.Add(strategy);
    }

    protected virtual IDisposable Bind(TSource source, Action onExpressionChanged)
    {
        var memberExpressions = _boundExpressions.SelectMany(GetMemberExpressions);
        foreach (var memberExpression in memberExpressions)
        {
            Action? unbind = null;

            SubscribeExpressionChanged(source, memberExpression, onExpressionChanged, newUnbind =>
            {
                UnBound -= unbind;
                unbind = newUnbind;
                UnBound += unbind;
            });
        }

        return DisposableAction.OnDisposeDo(UnBind);
    }

    private void UnBind()
    {
        UnBound?.Invoke();
        UnBound = null;
    }

    IEnumerable<MemberExpression> GetMemberExpressions(Expression rootExpression)
    {
        IEnumerable<MemberExpression> MemberExpressions(Expression? expression)
        {
            return expression switch
            {
                LambdaExpression lambda => MemberExpressions(lambda.Body),
                MemberExpression memberExpression => new[] { memberExpression },
                BinaryExpression binaryExpression => MemberExpressions(binaryExpression.Left)
                    .Union(MemberExpressions(binaryExpression.Right)),
                MethodCallExpression methodCallExpression => methodCallExpression.Arguments.SelectMany(
                    MemberExpressions).Union(MemberExpressions(methodCallExpression.Object)),
                IndexExpression indexExpression => indexExpression.Arguments.SelectMany(MemberExpressions),
                ConditionalExpression conditionalExpression => MemberExpressions(conditionalExpression.IfFalse)
                    .Union(MemberExpressions(conditionalExpression.IfTrue))
                    .Union(MemberExpressions(conditionalExpression.Test)),
                _ => Array.Empty<MemberExpression>()
            };
        }

        return MemberExpressions(rootExpression);
    }

    void SubscribeExpressionChanged(TSource source, MemberExpression memberExpression, Action onPropertyChanged, Action<Action?> onUnbindChanged)
    {
        IEnumerable<Expression> UnWrap(Expression? expression) => expression switch
        {
            MemberExpression member => UnWrap(member.Expression).Union(new[] { member }),
            not null => new[] { expression },
            _ => Array.Empty<Expression>()
        };

        if (source is null)
            return;

        var unwrappedExpressions = UnWrap(memberExpression).ToList();
        if (unwrappedExpressions.FirstOrDefault() is not ParameterExpression parameter)
            return;

        if (!typeof(TSource).IsAssignableFrom(parameter.Type))
            return;

        var expressions = unwrappedExpressions.Skip(1).ToList();
        SubscribeExpressionChanged(source, parameter, source, expressions, onPropertyChanged, onUnbindChanged);
    }

    void SubscribeExpressionChanged(TSource source, ParameterExpression parameter, object? father, List<Expression> currentExpressions, Action onExpressionChanged, Action<Action?> notifyPreviousStep)
    {
        if (father is null)
        {
            notifyPreviousStep(null);
            return;
        }

        var currentExpressionAsMemberExpression = currentExpressions.FirstOrDefault() as MemberExpression;
        var nextExpressions = currentExpressions.Skip(1).ToList();

        Action? unsubscribeAllForward = null;
        void NewOnExpressionChanged()
        {
            unsubscribeAllForward?.Invoke();
            SubscribeExpressionChanged(source, parameter, father, currentExpressions, onExpressionChanged, notifyPreviousStep);
            onExpressionChanged.Invoke();
        }

        var currentStepUnsubscribe = SubscribeExpressionChanged(father, currentExpressionAsMemberExpression, NewOnExpressionChanged);
        void PropagateBack(Action? action)
        {
            unsubscribeAllForward = currentStepUnsubscribe + action;
            notifyPreviousStep(unsubscribeAllForward);
        }

        if (currentExpressionAsMemberExpression is null)
        {
            PropagateBack(currentStepUnsubscribe);
            return;
        }

        Expression conversion = Expression.Convert(currentExpressionAsMemberExpression, typeof(object));
        var lambdaExpression = Expression.Lambda<Func<TSource, object?>>(conversion, false, parameter);
        var func = lambdaExpression.Compile();
        var value = func(source);

        SubscribeExpressionChanged(source, parameter, value, nextExpressions, onExpressionChanged, PropagateBack);
    }

    Action? SubscribeExpressionChanged(object father, MemberExpression? currentExpression, Action onPropertyChanged)
    {
        Action? unSubscribeAll = null;
        foreach (var bindingStrategy in _bindingStrategies)
        {
            var unSubscribe = bindingStrategy
                .Subscribe(father, currentExpression, onPropertyChanged);
            if (unSubscribe != null)
                unSubscribeAll += unSubscribe.Dispose;
        }

        return unSubscribeAll;
    }
}