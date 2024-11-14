using System;

namespace gs.ChefDI.Unity
{
    sealed class AsyncLoopItem : IPlayerLoopItem
    {
        readonly Action action;

        public AsyncLoopItem(Action action)
        {
            this.action = action;
        }
        
        public bool MoveNext()
        {
            action();
            return false;
        }
    }
}
