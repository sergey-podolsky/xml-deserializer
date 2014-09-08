using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Parsers
{
    public interface IParser
    {
        Type TergetType { get; }

        object Parse(string value, string[] format);
    }
}