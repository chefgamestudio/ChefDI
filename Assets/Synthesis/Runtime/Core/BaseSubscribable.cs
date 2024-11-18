using System;
using System.Threading;
using gs.ChefDI.Unity;
using MessagePipe;

namespace Synthesis.Core
{
    public abstract class BaseSubscribable : IInitializable, IDisposable
    {
        private IDisposable _subscription;
        private CancellationTokenSource _cts;

        protected DisposableBagBuilder _bagBuilder;

        protected CancellationToken Token
        {
            get
            {
                _cts ??= new CancellationTokenSource();
                return _cts.Token;
            }
        }

        public void Initialize()
        { 
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _bagBuilder = DisposableBag.CreateBuilder();
            Init();
            Subscriptions();
            _subscription = _bagBuilder.Build();
        }
        
        protected virtual void Init() {}
        
        protected virtual void Subscriptions(){}
        
        public void Dispose()
        {
            _cts?.Cancel();
            _subscription?.Dispose();
            _cts?.Dispose();
        }
    }
}