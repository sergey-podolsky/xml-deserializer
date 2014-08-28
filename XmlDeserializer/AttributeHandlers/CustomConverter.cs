using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    public abstract class CustomConverter<T> : ItemAttributeHandler<T>
    {
        public abstract T Convert(string value);

        protected override void Convert(Saxon.Api.XdmItem xdmItem, string format, ref T deserializable)
        {
            throw new NotImplementedException();
        }
    }
}
