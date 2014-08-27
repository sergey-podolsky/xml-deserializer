using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer
{
    public class SynonymsAttribute : Attribute
    {
        public ICollection<string> Synonyms { get; private set; }

        public SynonymsAttribute(params string[] synonyms)
        {
            this.Synonyms = synonyms;
        }
    }
}
