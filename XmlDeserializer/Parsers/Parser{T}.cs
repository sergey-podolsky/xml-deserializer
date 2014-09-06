using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using Saxon.Api;

    using XmlDeserializer.Parsers;

    public abstract class Parser<T> : IParser
    {
        public abstract T Parse(string value);

        Type IParser.TergetType
        {
            get
            {
                return typeof(T);
            }
        }

        object IParser.Parse(string value)
        {
            return this.Parse(value);
        }
    }
}