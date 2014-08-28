using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    public abstract class SingleItemAttributeHandler : ItemAttributeHandler
    {
        public override void HandleItem(Deserializer deserializer, Saxon.Api.XdmValue xdmValue, Attribute attribute, Type type, ref object deserializable)
        {
        }
    }
}
