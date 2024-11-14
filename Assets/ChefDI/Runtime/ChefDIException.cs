using System;

namespace gs.ChefDI
{
    public sealed class ChefDIException : Exception
    {
        public readonly Type InvalidType;

        public ChefDIException(Type invalidType, string message) : base(message)
        {
            InvalidType = invalidType;
        }
    }
}
