using System;
using System.ComponentModel;
using System.Resources;
using System.Threading;

namespace Metatrade.Core.Misc
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class DescriptionFromResourceAttribute : DescriptionAttribute
    {
        private readonly string _value;
        public DescriptionFromResourceAttribute(Type resourceSource, string name)
        {
            var resourceManager = new ResourceManager(resourceSource);
            _value = resourceManager.GetString(name, Thread.CurrentThread.CurrentCulture);
        }

        public override string Description => _value;
    }
}
