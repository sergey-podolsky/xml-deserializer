using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converters
{
    using XmlDeserializer.Converter;

    class ListConverter : IConverter
    {
        public void Convert(Saxon.Api.XdmValue xdmValue, Type type, ref object value, string format, Func<Type, IConverter> getConverter)
        {
            throw new NotImplementedException();
        }
    }
}
