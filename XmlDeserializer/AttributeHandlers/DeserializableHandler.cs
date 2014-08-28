using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    class DeserializableHandler : IAttributeHandler
    {
        public void Handle(IDeserializer deserializer, Saxon.Api.XdmNode xdmNode, Attribute attribute, Type type, ref object deserializable)
        {
            throw new NotImplementedException();
        }
    }
}
