using System;

namespace theorbo.Config
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ProtectedStringAttribute : Attribute
    {
        public const string CleartextPrefix = "cleartext:";
    }
}