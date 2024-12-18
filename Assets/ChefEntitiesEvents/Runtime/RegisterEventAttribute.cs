using System;

namespace Chef.EntitiesEvents
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RegisterEventAttribute : Attribute
    {
        public RegisterEventAttribute(Type type) { }
    }
}