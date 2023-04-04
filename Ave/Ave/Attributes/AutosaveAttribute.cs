using System;

namespace Ave.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AutosaveAttribute : Attribute
    {
        public bool PerformAutosave { get; }

        public AutosaveAttribute(bool performAutosave = true)
        {
            PerformAutosave = performAutosave;
        }
    }
}