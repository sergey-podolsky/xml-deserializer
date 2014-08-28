using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    [AttributeUsage(
        validOn: AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor,
        AllowMultiple = false,
        Inherited = false)]
    public class DeserializableAttribute : Attribute
    {
    }
}