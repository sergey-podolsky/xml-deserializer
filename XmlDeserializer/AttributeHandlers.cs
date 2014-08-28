using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using XmlDeserializer.Converters;

    class AttributeHandlers
    {
        private readonly Dictionary<Type, XPathAttributeHandler> dictionary = new Dictionary<Type, XPathAttributeHandler>();

        public void Add(Type type, XPathAttributeHandler handler)
        {
            this.dictionary[type] = handler;
        }

        public XPathAttributeHandler Get(Type type)
        {
            if (type.IsDefined(typeof(DeserializableAttribute), false))
            {
                return new DeserializableHandler();
            }

            XPathAttributeHandler handler;
            if (dictionary.TryGetValue(typeof(T), out handler)
            {
                return handler;
            }

            throw new XmlDeserializationException(string.Format("Type {0} is not deserializable", T));
        }
    }
}
