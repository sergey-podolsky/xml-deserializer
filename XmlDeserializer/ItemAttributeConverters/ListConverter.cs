using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converters
{
    using XmlDeserializer.Converter;

    class ListConverter : IItemAttributeConverter
    {
        public void Convert(Saxon.Api.XdmValue xdmValue, Type type, ref object value, string format, Func<Type, IItemAttributeConverter> getConverter)
        {
            throw new NotImplementedException();
        }

        public Type TargetType
        {
            get
            {
                return typeof(List<>);
            }
        }
    }
}
