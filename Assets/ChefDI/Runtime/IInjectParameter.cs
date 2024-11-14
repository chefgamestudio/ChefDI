using System;

namespace gs.ChefDI
{
    public interface IInjectParameter
    {
        bool Match(Type parameterType, string parameterName);
        object GetValue(IObjectResolver resolver);
    }
}