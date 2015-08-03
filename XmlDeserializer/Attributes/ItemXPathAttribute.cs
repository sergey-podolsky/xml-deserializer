using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Security.Cryptography.X509Certificates;

    using Saxon.Api;

    using XmlDeserializer.Converter;
    using XmlDeserializer.Converters;

    public class ItemXPathAttribute : AbstractXPathAttribute
    {
        private readonly string xpath;
        private Type converterType;
        public static IDictionary<Type, IItemAttributeConverter> Converters { get; private set; }
        public bool IsOptional { get; set; }
        public string[] Format;

        public Type Converter
        {
            get
            {
                return converterType;
            }
            set
            {
                if (typeof(IItemAttributeConverter).IsAssignableFrom(value))
                {
                    this.converterType = value;
                }
                else
                {
                    throw new XmlDeserializationException("Custom converter should implement " + typeof(IItemAttributeConverter));
                }
            }
        }
        
        public ItemXPathAttribute(string xpath)
        {
            this.xpath = xpath;
        }

        static ItemXPathAttribute()
        {
            Converters = new ConcurrentDictionary<Type, IItemAttributeConverter>();
        }


        public override void Apply(Deserializer deserializer, XdmItem xdmItem, Type type, ref object value)
        {
            var xdmValue = deserializer.XPathCompiler.Evaluate(xpath, xdmItem);
            if (xdmValue.Count == 0 && !this.IsOptional)
            {
                throw new XmlDeserializationException("XPath query returned 0 results for non-optional value.");
            }
        }

        private IItemAttributeConverter GetConverterForType(Type type)
        {
            if (type == this.converterType)
            {
                return this.CreateCustomConverterInstance();
            }

            IItemAttributeConverter converter;
            if (Converters.TryGetValue(type, out converter))
            {
                return converter;
            }

            if (type.IsDefined(typeof(DeserializableAttribute), false))
            {
                return Converters[type] = new DeserializableConverter(type);
            }

            throw new XmlDeserializationException("Type " + type + " is not supported.");
        }

        private IItemAttributeConverter CreateCustomConverterInstance()
        {
            try
            {
                return (IItemAttributeConverter)Activator.CreateInstance(this.converterType);
            }
            catch (Exception e)
            {
                var message = "Type " + this.converterType + " shoud have default constructor without parameters.";
                throw new XmlDeserializationException(message, e);
            }
        }
    }
}