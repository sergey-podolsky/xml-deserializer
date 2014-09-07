using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using Saxon.Api;

    public static class DeserializerExtensionsForItemAttribute
    {
        public static void Deserialize<T>(this Deserializer deserializer, Uri uri, string xpath, ref T deserializable)
        {
            XdmNode xdmNode = deserializer.DocumentBuilder.Build(uri);
            Deserialize(deserializer, xdmNode, xpath, ref deserializable);
        }

        public static void Deserialize<T>(this Deserializer deserializer, XdmNode xdmItem, string xpath, ref T deserializable)
        {
            var itemAttribute = new ItemAttribute(xpath, true);
            object box = deserializable;
            itemAttribute.Apply(deserializer, xdmItem, typeof(T), ref box);
            deserializable = (T)box;
        }
    }
}
