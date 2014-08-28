using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using System.Runtime.Serialization;

    public class XmlDeserializationException : Exception
    {
        public XmlDeserializationException()
        {
        }

        public XmlDeserializationException(string message) : base(message)
        {
        }

        public XmlDeserializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public XmlDeserializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
