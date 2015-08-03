using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Converters
{
    using System.Collections.ObjectModel;
    using System.Reflection;

    using Saxon.Api;

    using XmlDeserializer.Converter;

    public class DeserializableConverter : IItemAttributeConverter
    {
        private interface IMember
        {
            void Deserialize(Deserializer deserializer, XdmItem xdmItem, object obj);
        }

        private sealed class Field : IMember
        {
            public FieldInfo FieldInfo { get; private set; }
            public ReadOnlyCollection<AbstractXPathAttribute> XPathAttributes { get; private set; }

            public Field(FieldInfo fieldInfo, ReadOnlyCollection<AbstractXPathAttribute> attributes)
            {
                this.FieldInfo = fieldInfo;
                this.XPathAttributes = attributes;
            }

            public void Deserialize(Deserializer deserializer, XdmItem xdmItem, object obj)
            {
                try
                {
                    object fieldValue = this.FieldInfo.GetValue(obj);
                    foreach (var xpathAttribute in this.XPathAttributes)
                    {
                        xpathAttribute.Apply(deserializer, xdmItem, this.FieldInfo.FieldType, ref fieldValue);
                    }
                }
                catch (Exception e)
                {
                    var message = "Failed to deserialize field with name " + this.FieldInfo.Name;
                    throw new XmlDeserializationException(message, e);
                }
            }
        }

        private sealed class Property : IMember
        {
            public PropertyInfo PropertyInfo { get; private set; }

            public ReadOnlyCollection<AbstractXPathAttribute> XPathAttributes { get; private set; }

            public Property(PropertyInfo propertyInfo, ReadOnlyCollection<AbstractXPathAttribute> attributes)
            {
                if (propertyInfo.GetSetMethod(false) == null)
                {
                    var message = "Property with name " + propertyInfo.Name + " does not have set method.";
                    throw new XmlDeserializationException(message);
                }

                this.PropertyInfo = propertyInfo;
                this.XPathAttributes = attributes;
            }

            public void Deserialize(Deserializer deserializer, XdmItem xdmItem, object obj)
            {
                try
                {
                    object propertyValue = this.PropertyInfo.GetValue(obj, null);
                    foreach (var xpathAttribute in this.XPathAttributes)
                    {
                        xpathAttribute.Apply(deserializer, xdmItem, this.PropertyInfo.PropertyType, ref propertyValue);
                    }
                    this.PropertyInfo.SetValue(obj, propertyValue, null);
                }
                catch (Exception e)
                {
                    var message = "Failed to deserialize property with name " + this.PropertyInfo.Name;
                    throw new XmlDeserializationException(message, e);
                }
            }
        }

        private readonly List<IMember> members = new List<IMember>();

        private readonly Lazy<ConstructorInfo> constructorInfo;

        public DeserializableConverter(Type targetType)
        {
            this.TargetType = targetType;

            var fields = from FieldInfo fieldInfo in targetType.GetFields()
                         let xpathAttributes = fieldInfo.GetCustomAttributes(false).OfType<AbstractXPathAttribute>()
                         where xpathAttributes.Any()
                         select new Field(fieldInfo, xpathAttributes.ToList().AsReadOnly());

            var properties = from PropertyInfo propertyInfo in targetType.GetProperties()
                             let xpathAttributes = propertyInfo.GetCustomAttributes(false).OfType<AbstractXPathAttribute>()
                             where xpathAttributes.Any()
                             select new Property(propertyInfo, xpathAttributes.ToList().AsReadOnly());

            this.members.AddRange(fields.Cast<IMember>());
            this.members.AddRange(properties.Cast<IMember>());

            this.constructorInfo = new Lazy<ConstructorInfo>(this.GetConstructor);
        }

        private ConstructorInfo GetConstructor()
        {
            var constructors = this.TargetType.GetConstructors();
            if (constructors.Length == 0)
            {
                throw new XmlDeserializationException("Type does not contain constructors");
            }

            var deserializebleConstructors = constructors
                .Where(constructor => constructor.IsDefined(typeof(DeserializableAttribute), false))
                .ToArray();

            if (deserializebleConstructors.Length > 1)
            {
                throw new XmlDeserializationException("There is more than one constructor annotated with " + typeof(DeserializableAttribute));
            }

            if (deserializebleConstructors.Length == 1)
            {
                return deserializebleConstructors.Single();
            }

            var defaultConstructor = constructors.SingleOrDefault(constructor => constructor.GetParameters().Length == 0);
            if (defaultConstructor != null)
            {
                return defaultConstructor;
            }

            throw new XmlDeserializationException("There is no default constructor or constructor annotated with " + typeof(DeserializableAttribute));
        }

        private object InvokeConstructor(Deserializer deserializer, XdmNode xdmNode)
        {
            var parameters = this.constructorInfo.Value.GetParameters();
            var parameterValues = new List<object>(parameters.Length);
            foreach (var parameter in parameters)
            {
                object parameterValue = parameter.DefaultValue;
                var xpathAttributes = parameter.GetCustomAttributes(false).OfType<AbstractXPathAttribute>();
                foreach (var xpathAttribute in xpathAttributes)
                {
                    xpathAttribute.Apply(deserializer, xdmNode, parameter.GetType(), ref parameterValue);
                }
                parameterValues.Add(parameterValue);
            }
            return this.constructorInfo.Value.Invoke(parameterValues.ToArray());
        }

        public void Convert(Deserializer deserializer, XdmValue xdmValue, Type type, ref object value, string[] format, Func<Type, IItemAttributeConverter> getConverter)
        {
            if (xdmValue.Count == 0)
            {
                return;
            }

            var xdmNode = xdmValue as XdmNode;
            if (xdmNode == null)
            {
                throw new XmlDeserializationException("XPath query returned invalid result " + xdmValue);
            }

            if (value == null)
            {
                value = InvokeConstructor(deserializer, xdmNode);
            }

            foreach (var member in this.members)
            {
                member.Deserialize(deserializer, xdmNode, value);
            }
        }

        public Type TargetType { get; private set; }
    }
}
