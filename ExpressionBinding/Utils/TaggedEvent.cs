using System.Collections.Concurrent;

namespace ExpressionBinding.Utils
{
    public class TaggedAction<TTag> where TTag : notnull
    {
        private readonly ConcurrentDictionary<TTag, Action> _handlersByTag = new();

        public void Add(TTag tag, Action handler)
        {
            if (_handlersByTag.TryGetValue(tag, out var currentHandler))
                _handlersByTag.TryAdd(tag, currentHandler + handler);
            else
                _handlersByTag[tag] = handler;
        }

        public void Remove(TTag tag, Action handler)
        {
            if (!_handlersByTag.TryGetValue(tag, out var currentHandler))
                return;

            var newHandler = currentHandler - handler;
            if (newHandler is null)
                _handlersByTag.TryRemove(tag, out _);
            else
                _handlersByTag.TryAdd(tag, newHandler);
        }

        public void Invoke(TTag tag)
        {
            if (_handlersByTag.TryGetValue(tag, out var handler))
                handler.Invoke();
        }

        public int HandlersCount => _handlersByTag.Count;
    }
}
