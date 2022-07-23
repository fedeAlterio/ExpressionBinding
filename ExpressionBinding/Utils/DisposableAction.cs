namespace ExpressionBinding.Utils
{
    public struct DisposableAction : IDisposable
    {
        private readonly Action _onDispose;
        private DisposableAction(Action onDispose) => _onDispose = onDispose;
        public static DisposableAction OnDisposeDo(Action onDispose) => new(onDispose);
        public void Dispose() => _onDispose();
    }
}
