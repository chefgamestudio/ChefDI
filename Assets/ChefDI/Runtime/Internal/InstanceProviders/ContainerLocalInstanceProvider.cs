using System;

namespace gs.ChefDI.Internal
{
    sealed class ContainerLocalInstanceProvider : IInstanceProvider
    {
        readonly Type wrappedType;
        readonly Registration valueRegistration;

        public ContainerLocalInstanceProvider(Type wrappedType, Registration valueRegistration)
        {
            this.wrappedType = wrappedType;
            this.valueRegistration = valueRegistration;
        }

        public object SpawnInstance(IObjectResolver resolver)
        {
            object value;

            if (resolver is ScopedContainer scope &&
                valueRegistration.Provider is CollectionInstanceProvider collectionProvider)
            {
                using (ListPool<RegistrationElement>.Get(out var entirelyRegistrations))
                {
                    collectionProvider.CollectFromParentScopes(scope, entirelyRegistrations, localScopeOnly: true);
                    value = collectionProvider.SpawnInstance(scope, entirelyRegistrations);
                }
            }
            else
            {
                value = resolver.Resolve(valueRegistration);
            }
            var parameterValues = CappedArrayPool<object>.Shared8Limit.Rent(1);
            try
            {
                parameterValues[0] = value;
                return Activator.CreateInstance(wrappedType, parameterValues);
            }
            finally
            {
                CappedArrayPool<object>.Shared8Limit.Return(parameterValues);
            }
        }
   }
}