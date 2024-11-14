using System.Runtime.CompilerServices;

namespace gs.ChefDI.Internal
{
    sealed class ExistingInstanceProvider : IInstanceProvider
    {
        readonly object implementationInstance;

        public ExistingInstanceProvider(object implementationInstance)
        {
            this.implementationInstance = implementationInstance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver) => implementationInstance;
    }
}
