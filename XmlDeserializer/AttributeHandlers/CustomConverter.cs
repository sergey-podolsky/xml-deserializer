﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.AttributeHandlers
{
    public abstract class CustomConverter<T> : ItemAttributeHandler
    {
        public abstract T Convert(string value);

        public override void HandleItem(Deserializer deserializer, Saxon.Api.XdmValue xdmValue, Attribute attribute, Type type, ref object deserializable)
        {
            throw new NotImplementedException();
        }
    }
}
