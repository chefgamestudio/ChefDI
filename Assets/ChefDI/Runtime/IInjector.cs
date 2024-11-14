using System.Collections.Generic;

namespace gs.ChefDI
{
    public interface IInjector
    {
        void Inject(object instance, IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters);
        object CreateInstance(IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters);
    }
}
