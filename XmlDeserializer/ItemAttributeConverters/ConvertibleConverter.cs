﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converters
{
    using XmlDeserializer.Converter;

    public class ConvertibleConverter : IItemAttributeConverter
    {
        public void Convert(Saxon.Api.XdmValue xdmValue, ref object value, string format)
        {
            throw new NotImplementedException();
        }

        public void Convert(Saxon.Api.XdmValue xdmValue, Type type, ref object value, string format, Func<Type, IItemAttributeConverter> getConverter)
        {
            throw new NotImplementedException();
        }

        public Type TargetType
        {
            get { throw new NotImplementedException(); }
        }
    }
}