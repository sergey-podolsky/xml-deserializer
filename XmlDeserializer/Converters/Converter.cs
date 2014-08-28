using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using XmlDeserializer.Converters;

    public abstract class Converter<T> : ItemAttributeHandler<T>
    {
        protected abstract T Convert(string value, string format);

        protected override void Convert(Saxon.Api.XdmItem xdmItem, ItemAttribute itemAttribute, ref T deserializable)
        {
            
        }
    }
}
