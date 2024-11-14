using System;

namespace gs.ChefDI.Internal
{
    readonly struct BuilderCallbackDisposable : IDisposable
    {
        readonly Action<IObjectResolver> callback;
        readonly IObjectResolver container;

        public BuilderCallbackDisposable(Action<IObjectResolver> callback, IObjectResolver container)
        {
            this.callback = callback;
            this.container = container;
        }

        public void Dispose()
        {
            callback.Invoke(container);
        }
    }
}