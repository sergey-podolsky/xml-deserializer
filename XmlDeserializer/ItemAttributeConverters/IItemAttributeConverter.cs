using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converter
{
    using Saxon.Api;

    public interface IItemAttributeConverter
    {
        Type TargetType { get; }

        void Convert(
            XdmValue xdmValue,
            Type type,
            ref object value,
            string[] format,
            Func<Type, IItemAttributeConverter> getConverter);
    }
}