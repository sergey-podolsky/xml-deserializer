using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converter
{
    using XmlDeserializer.Parsers;

    public class ParsableConverter : IItemAttributeConverter
    {
        private readonly IParser parser;

        public ParsableConverter(IParser parser)
        {
            this.parser = parser;
        }

        public void Convert(Saxon.Api.XdmValue xdmValue, Type type, ref object value, string format, Func<Type, IItemAttributeConverter> getConverter)
        {
            value = parser.Parse(xdmValue.Cast<string>().Single());
        }

        public Type TargetType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
