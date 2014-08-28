using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    class ListHandler : ItemAttributeHandler
    {
        public override void HandleItem(Deserializer deserializer, Saxon.Api.XdmValue xdmValue, Attribute attribute, Type type, ref object deserializable)
        {
            if (deserializable == null)
            {
                deserializable = Activator.CreateInstance(type);
            }

            Type itemType = type.GetGenericArguments().Single();
            var handler = deserializer.SupportedTypes.Get(itemType, attribute.GetType());
            foreach (var value in xdmValue)
            {
                object itemValue = 0;
                handler.Handle(deserializer, value, attribute, itemType, ref itemValue);
                type.GetMethod("Add").Invoke(deserializable, new[] { itemValue });
            }
        }
    }
}