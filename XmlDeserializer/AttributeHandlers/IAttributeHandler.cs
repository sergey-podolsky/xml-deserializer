using Saxon.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    public interface IAttributeHandler
    {
        void Handle(
            IDeserializer deserializer,
            XdmNode xdmNode,
            Attribute attribute,
            Type type,
            ref object deserializable);
    }
}
