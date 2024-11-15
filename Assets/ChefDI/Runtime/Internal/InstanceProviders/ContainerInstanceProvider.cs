using System.Runtime.CompilerServices;

namespace gs.ChefDI.Internal
{
    sealed class ContainerInstanceProvider : IInstanceProvider
    {
        public static readonly ContainerInstanceProvider Default = new ContainerInstanceProvider();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object SpawnInstance(IObjectResolver resolver) => resolver;
    }
}
