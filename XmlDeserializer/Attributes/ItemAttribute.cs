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
        
        private static Dictionary<Type, IConverter> converters = new Dictionary<Type, IConverter>();

        private readonly XPathExecutable xpathExecutable;

        private IParser parser;

        private Type parserType;

        public bool IsRequired { get; set; }

        public Type Parser
        {
            get
            {
                return parserType;
            }
            set
            {
                this.parserType = value;
                if (value == null)
                {
                    return;
                }

                if (!typeof(IParser).IsAssignableFrom(value))
                {
                    throw new XmlDeserializationException("Parser should implement " + typeof(Parser<>));
                }

                try
                {
                    this.parser = (IParser)Activator.CreateInstance(value);
                }
                catch (Exception e)
                {
                    throw new XmlDeserializationException(value + " shoud have default constructor without parameters", e);
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
            var convertibleConverter = new ConvertibleConverter();
            converters[typeof(int)] = convertibleConverter;
            converters[typeof(List<>)] = new ListConverter();
        }


        public override void Apply(Deserializer deserializer, Saxon.Api.XdmItem xdmItem, Type type, ref object value)
        {
            // Evaluate XPath
            var xdmValue = deserializer.XPathCompiler.Evaluate(xpath, xdmItem);
            if (xdmValue.Count == 0 && this.IsRequired)
            {
                throw new XmlDeserializationException("Value is required but XPath query returned nothing");
            }

            if (xdmValue.Count > 1 && !typeof(IEnumerable).IsAssignableFrom(type))
            {
                throw new XmlDeserializationException("XPath returned more than one value");
            }
        }

        private XdmValue EvaluateXPath(XdmItem xdmItem)
        {
            XPathSelector xPathSelector = xpathExecutable.Load();
            xPathSelector.ContextItem = xdmItem;
            return xPathSelector.Evaluate();
        }

        private IConverter GetConverterForType(Type type)
        {
            if (type == parser.TergetType)
            {
                return new ParsableConverter(this.parser);
            }

            if (type.IsDefined(typeof(DeserializableAttribute), false))
            {
                return new DeserializableConverter();
            }

            IConverter converter;
            if (converters.TryGetValue(type, out converter))
            {
                return converter;
            }

            throw new XmlDeserializationException("Type " + type + " is not supported");
        }
    }
}