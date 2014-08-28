using Saxon.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    internal class DictionaryAttributeHandler : IAttributeHandler
    {
        public void Handle(Deserializer deserializer, XdmNode xdmNode, Attribute attribute, Type type, ref object deserializable)
        {
            throw new NotImplementedException();
        }
    }
}
