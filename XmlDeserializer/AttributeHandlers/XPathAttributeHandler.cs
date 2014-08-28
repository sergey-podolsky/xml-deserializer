using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converters
{
    public abstract class XPathAttributeHandler
    {
        void Handle(
            IDeserializer deserializer,
            XdmNode xdmNode,
            XPathAttribute xpathAttribute,
            Type type,
            ref object deserializable);
    }
}
