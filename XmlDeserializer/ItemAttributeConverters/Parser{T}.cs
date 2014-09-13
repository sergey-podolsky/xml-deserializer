using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using Saxon.Api;

    public abstract class Parser<T>
    {
        protected abstract void Parse(string input, ref T output, string[] format);

        Type TergetType
        {
            get
            {
                return typeof(T);
            }
        }
    }
}