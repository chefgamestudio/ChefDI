using System.Threading;
#if CHEFDI_UNITASK_INTEGRATION
using Awaitable = Cysharp.Threading.Tasks.UniTask;
#elif UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using Awaitable = System.Threading.Tasks.Task;
#endif

namespace gs.ChefDI.Unity
{
    public interface IAsyncStartable
    {
        Awaitable StartAsync(CancellationToken cancellation = default);
    }
}
