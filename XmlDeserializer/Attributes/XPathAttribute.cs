using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using Saxon.Api;

    [AttributeUsage(
        validOn: AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter,
        AllowMultiple = false,
        Inherited = false)]
    public abstract class XPathAttribute : Attribute
    {
        public string XmlUriXPath { get; set; }

        public abstract void Apply(Deserializer deserializer, XdmItem xdmItem, Type type, ref object value);
    }
}