using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converter
{
    using Saxon.Api;

    interface IConverter
    {
        void Convert(XdmValue xdmValue, Type type, ref object value, string format, Func<Type, IConverter> getConverter);
    }
}