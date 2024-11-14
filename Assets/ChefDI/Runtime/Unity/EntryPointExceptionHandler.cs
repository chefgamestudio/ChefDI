using System;

namespace gs.ChefDI.Unity
{
    sealed class EntryPointExceptionHandler
    {
        readonly Action<Exception> handler;

        public EntryPointExceptionHandler(Action<Exception> handler)
        {
            this.handler = handler;
        }

        public void Publish(Exception ex)
        {
            handler.Invoke(ex);
        }
    }
}
