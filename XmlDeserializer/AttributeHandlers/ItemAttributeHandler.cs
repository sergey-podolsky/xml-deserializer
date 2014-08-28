using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    using Saxon.Api;

    public abstract class ItemAttributeHandler : IAttributeHandler
    {
        public abstract void HandleItem(
            Deserializer deserializer,
            XdmValue xdmValue,
            Attribute attribute,
            Type type,
            ref object deserializable);

        public void Handle(Deserializer deserializer, XdmNode xdmNode, Attribute attribute, Type type, ref object deserializable)
        {
            var itemAttribute = (ItemAttribute)attribute;
            XdmValue xdmValue = deserializer.XPathCompiler.Evaluate(itemAttribute.XPath, xdmNode);
            if (xdmValue.Count == 0 && itemAttribute.IsRequired)
            {
                throw new XmlDeserializationException("Value is required but not provided in XML");
            }

            this.HandleItem(deserializer, xdmValue, attribute, type, ref deserializable);
        }
    }
}