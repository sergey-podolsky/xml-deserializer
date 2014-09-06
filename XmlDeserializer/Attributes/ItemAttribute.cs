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

        private Type converter;

        public bool IsRequired { get; set; }

        public Type Converter
        {
            get
            {
                return converter;
            }
            set
            {
                if (typeof(IItemAttributeConverter).IsAssignableFrom(value))
                {
                    this.converter = value;
                }
                else
                {
                    throw new XmlDeserializationException("Custom converter should implement " + typeof(IItemAttributeConverter));
                }
            }
        }

        public string Format { get; set; }
        
        public ItemAttribute(string xpath)
        {
            this.xpath = xpath;
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
            if (xdmValue.Count == 0 && this.IsRequired)
            {
                throw new XmlDeserializationException("XPath query returned 0 results for required value.");
            }
        }

        private IItemAttributeConverter GetConverterForType(Type type)
        {
            if (type == this.converter)
            {
                return this.CreateCustomConverterInstance();
            }

            if (type.IsDefined(typeof(DeserializableAttribute), false))
            {
                return new DeserializableConverter();
            }

            IItemAttributeConverter converterInstance;
            if (Converters.TryGetValue(type, out converterInstance))
            {
                return converterInstance;
            }

            throw new XmlDeserializationException("Type " + type + " is not supported.");
        }

        private IItemAttributeConverter CreateCustomConverterInstance()
        {
            try
            {
                return (IItemAttributeConverter)Activator.CreateInstance(this.converter);
            }
            catch (Exception e)
            {
                var message = "Type " + this.converter + " shoud have default constructor without parameters.";
                throw new XmlDeserializationException(message, e);
            }
        }
    }
}