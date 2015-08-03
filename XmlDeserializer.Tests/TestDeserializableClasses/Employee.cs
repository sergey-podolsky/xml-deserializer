using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Tests.TestDeserializableClasses
{
    using System.Text.RegularExpressions;
    using Saxon.Api;
    

    [Deserializable] // Marks user class as deserializable
    internal class Employee
    {
        // mandatory value
        [ItemXPath("name", IsOptional = true)]
        public string Name { get; set; }

        // optional value with custom format
        [ItemXPath("birtday", Format = new[] {"dd-MM-yyyy"})]
        public DateTime? Birthday { get; set; }

        // default optional value idiom
        [ItemXPath("(birthplace, 'Unknown')[1]")]
        public string Birthplace { get; set; }

        // enum
        [ItemXPath("gender")]
        public Sex Gender { get; set; }

        // enum flags
        [ItemXPath("system_privileges/privilege")]
        public Privilege SystemPrivileges { get; set; }

        // bool value with custom converter
        [ItemXPath("contractor", Converter = typeof(YesNoBoolConverter))] 
        public bool IsContractor { get; set; }

        // nested class
        [ItemXPath("mailing_address")]
        public Address MailingAddress { get; set; }

        // collection (can be empty)
        [ItemXPath("subordinates/subordinate")]
        public ICollection<Employee> Subordinates { get; set; }

        // value from another XML with URI that can be retrieved from current XML by given XPath
        [ItemXPath("//employee", XmlUriXPath = "line_manager/@link")]
        public Employee LineManager { get; set; }

        // dictionary with non-null values
        [DictionaryXPaths(entryXPath: "contact_info/*", keyXPath: "name()", valueXPath: ".", IsValueRequired = true)]
        public Dictionary<string, string> ContactInfo { get; set; }

        // dictionary where values are non-empty lists
        [DictionaryXPaths(entryXPath: "skills/*", keyXPath: "name()", valueXPath: "@*/concat(name(), ': ', ., ' year(s)')", IsValueRequired = true)]
        public Dictionary<string, List<string>> Skills { get; set; }

        // collection of comma separated values
        [InlineCollection(xpath: "spoken_languages", separator: @"\s*,\s*")] // separator is a regular expression
        public string[] SpokenLanguages { get; set; }

        // dictionary of semicolon separated entries
        [InlineDictionaryXPath(xpath: "personal_details", entrySeparator: @"\s*;\s*", keyValueSeparator: @"\s*=\s*")]
        public IDictionary<string, string> PersonalDetails { get; set; }

        [Deserializable]
        public sealed class Address // immutable class
        {
            public ulong ZipCode { get; private set; }
            public string Country { get; private set; }
            public string City { get; private set; }
            public string StreetAddress { get; private set; }

            [Deserializable] // tells which constructor to use for instantiation
            public Address(
                [ItemXPath("@zip")] ulong zipCode,
                [ItemXPath("@country", IsOptional = true)] string country,
                [ItemXPath("@city")] string city,
                [ItemXPath("@street_address")] string streetAddress)
            {
                this.ZipCode = zipCode;
                this.Country = country;
                this.City = city;
                this.StreetAddress = streetAddress;
            }
        }

        public enum Sex
        {
            Male, // case insensitive
            Female
        }

        [Flags] // can be deserialized in the same way as collections
        public enum Privilege
        {
            Create,
            Read,
            Update,
            Delete
        }

        private class YesNoBoolConverter : FormattedConverter<bool>
        {
            protected override void Convert(string input, ref bool output, string[] format)
            {
                switch (input.ToLower())
                {
                    case "yes":
                        output = true;
                        break;
                    case "no":
                        output = false;
                        break;
                    default:
                        throw new ArgumentException("Expected 'yes' or 'no' value, but was " + output);
                }
            }
        }
    }
}