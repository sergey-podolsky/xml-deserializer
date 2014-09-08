using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    using System.Collections;
    using System.Security.Cryptography.X509Certificates;

    using Saxon.Api;

    using XmlDeserializer.Converter;
    using XmlDeserializer.Converters;
    using XmlDeserializer.Parsers;

    public class ItemAttribute : XPathAttribute
    {
        private readonly string xpath;

        public static IDictionary<Type, IItemAttributeConverter> Converters { get; private set; }

        private Type converterType;

        private bool isRequired;

        private string[] format;

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
        
        public ItemAttribute(string xpath, bool isRequired = false, params string[] format)
        {
            this.xpath = xpath;
            this.isRequired = isRequired;
            this.format = format;
        }

        static ItemAttribute()
        {
            Converters = new Dictionary<Type, IItemAttributeConverter>();
            var convertibleConverter = new ConvertibleConverter();
            Converters[typeof(int)] = convertibleConverter;
            Converters[typeof(List<>)] = new ListConverter();
        }


        public override void Apply(Deserializer deserializer, XdmItem xdmItem, Type type, ref object value)
        {
            var xdmValue = deserializer.XPathCompiler.Evaluate(xpath, xdmItem);
            if (xdmValue.Count == 0 && this.isRequired)
            {
                throw new XmlDeserializationException("XPath query returned 0 results for required value.");
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