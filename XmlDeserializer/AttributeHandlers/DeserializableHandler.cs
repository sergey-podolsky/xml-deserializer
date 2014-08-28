using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    using System.Reflection;
    using System.Runtime.InteropServices;

    using Saxon.Api;

    class DeserializableHandler : ItemAttributeHandler
    {
        public override void HandleItem(Deserializer deserializer, XdmValue xdmValue, Attribute attribute, Type type, ref object deserializable)
        {
            var xdmNode = xdmValue as XdmNode;
            if (xdmNode == null)
            {
                throw new XmlDeserializationException("XPath query returned invalid result " + xdmValue);
            }

            if (deserializable == null)
            {
                var constructor = GetConstructor(type);
                deserializable = InvokeConstructor(deserializer, xdmNode, constructor);
            }

            DeserializeFields(deserializer, xdmNode, deserializable);
            DeserializeProperties(deserializer, xdmNode, deserializable);
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            var constructors = type.GetConstructors();
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

        private static object InvokeConstructor(Deserializer deserializer, XdmNode xdmNode, ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();
            var parameterValues = new List<object>(parameters.Length);
            foreach (var parameter in parameters)
            {
                var attributes = parameter.GetCustomAttributes(false).OfType<XPathAttribute>().ToArray();
                if (attributes.Length == 0)
                {
                    parameterValues.Add(null);
                }

                if (attributes.Length > 1)
                {
                    var error = "Constructor parameter is annotated with nore than one attribute deriving from " + typeof(XPathAttribute);
                    throw new XmlDeserializationException(error);
                }

                var peremeterValue = parameter.DefaultValue;
                var attribute = attributes.Single();
                var handler = deserializer.SupportedTypes.Get(parameter.ParameterType, attribute.GetType());
                handler.Handle(deserializer, xdmNode, attribute, parameter.ParameterType, ref peremeterValue);
                parameterValues.Add(peremeterValue);
            }

            return constructor.Invoke(parameterValues.ToArray());
        }


        private static void DeserializeFields(Deserializer deserializer, XdmNode xdmNode, object deserializable)
        {
            var fields = deserializable.GetType()
                .GetFields()
                .Where(field => field.IsDefined(typeof(XPathAttribute), false))
                .ToArray();

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(false).OfType<XPathAttribute>().ToArray();
                if (attributes.Length > 1)
                {
                    var error = string.Format(
                        "Field '{0}' is annotated with more than one attribute deriving from {1}",
                        field.Name,
                        typeof(XPathAttribute));
                    throw new XmlDeserializationException(error);
                }

                var fieldAttribute = attributes.Single();
                object value = field.GetValue(deserializable);
                var handler = deserializer.SupportedTypes.Get(field.FieldType, fieldAttribute.GetType());
                handler.Handle(deserializer, xdmNode, fieldAttribute, field.FieldType, ref value);
                field.SetValue(deserializable, value);
            }
        }

        private static void DeserializeProperties(Deserializer deserializer, XdmNode xdmNode, object deserializable)
        {
            var properties = deserializable
                .GetType()
                .GetProperties()
                .Where(property => property.IsDefined(typeof(XPathAttribute), false))
                .ToArray();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    throw new XmlDeserializationException("Property " + property.Name + " can not write");
                }

                var attributes = property.GetCustomAttributes(false).OfType<XPathAttribute>().ToArray();
                if (attributes.Length > 1)
                {
                    var error = string.Format(
                        "Property '{0}' is annotated with more than one attribute deriving from {1}",
                        property.Name,
                        typeof(XPathAttribute));
                    throw new XmlDeserializationException(error);
                }

                var propertyAttribute = attributes.Single();
                object value = property.GetValue(deserializable, null);
                var handler = deserializer.SupportedTypes.Get(property.PropertyType, propertyAttribute.GetType());
                handler.Handle(deserializer, xdmNode, propertyAttribute, property.PropertyType, ref value);
                property.SetValue(deserializable, value, null);
            }
        }
    }
}