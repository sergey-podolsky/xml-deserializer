using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    using Saxon.Api;

    public abstract class ItemAttributeHandler<T> : IAttributeHandler
    {
        protected abstract void Convert(XdmItem xdmItem, string format, ref T deserializable);


        public void Handle(IDeserializer deserializer, XdmNode xdmNode, Attribute attribute, Type type, ref object deserializable)
        {
            throw new NotImplementedException();
        }
    }
}
