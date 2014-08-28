using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using XmlDeserializer.AttributeHandlers;

    public class SupportedTypes
    {
        private readonly Dictionary<Tuple<Type, Type>, IAttributeHandler> supportedTypes;

        public SupportedTypes()
        {
            this.supportedTypes = new Dictionary<Tuple<Type, Type>, IAttributeHandler>();
            // TODO: Add built-in supported types here
        }

        public void Add(Type deserializableType, Type attributeType, IAttributeHandler handler)
        {
            this.supportedTypes[Tuple.Create(deserializableType, attributeType)] = handler;
        }

        public IAttributeHandler Get(Type deserializableType, Type attributeType)
        {
            IAttributeHandler handler;
            if (this.supportedTypes.TryGetValue(Tuple.Create(deserializableType, attributeType), out handler))
            {
                return handler;
            }

            if (deserializableType.IsDefined(typeof(DeserializableAttribute), false) && attributeType.Equals(typeof(ItemAttribute)))
            {
                return new DeserializableHandler();
            }

            throw new XmlDeserializationException("Type " + deserializableType + " is not supported by attribute " + attributeType);
        }
    }
}