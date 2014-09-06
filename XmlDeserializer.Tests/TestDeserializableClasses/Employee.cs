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
        [Item(xpath: "name", IsRequired = true)]
        public string Name { get; set; }

        // optional value with custom format
        [Item(xpath: "birtday", Format = "dd-MM-yyyy")]
        public DateTime? Birthday { get; set; }

        // default optional value
        [Item(xpath: "(birthplace, 'Unknown')[1]")]
        public string Birthplace { get; set; }

        // enum
        [Item(xpath: "gender")]
        public Sex Gender { get; set; }

        // enum flags
        [Item(xpath: "system_privileges/privilege")]
        public Privilege SystemPrivileges { get; set; }

        // bool value with custom converter
        [Item(xpath: "contractor", Parser = typeof(YesNoBoolParser))]
        public bool IsContractor { get; set; }

        // nested class
        [Item(xpath: "mailing_address")]
        public Address MailingAddress { get; set; }

        // collection (can be empty)
        [Item(xpath: "subordinates/subordinate")]
        public ICollection<Employee> Subordinates { get; set; }

        // value from another XML with URI that can be retrieved from current XML by given XPath
        [Item(xpath: "//employee", XmlUriXPath = "line_manager/@link")]
        public Employee LineManager { get; set; }

        // dictionary with non-null values
        [Dictionary(entryXPath: "contact_info/*", keyXPath: "name()", valueXPath: ".", IsValueRequired = true)]
        public Dictionary<string, string> ContactInfo { get; set; }

        // dictionary where values are non-empty lists
        [Dictionary(entryXPath: "skills/*", keyXPath: "name()", valueXPath: "@*/concat(name(), ': ', ., ' year(s)')", IsValueRequired = true)]
        public Dictionary<string, List<string>> Skills { get; set; }

        // collection of comma separated values
        [InlineCollection(xpath: "spoken_languages", separator: @"\s*,\s*")] // separator is a regular expression
        public string[] SpokenLanguages { get; set; }

        // dictionary of semicolon separated entries
        [InlineDictionary(xpath: "personal_details", entrySeparator: @"\s*;\s*", keyValueSeparator: @"\s*=\s*")]
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
                [Item(xpath: "@zip", IsRequired = true)] ulong zipCode,
                [Item(xpath: "@country", IsRequired = true)] string country,
                [Item(xpath: "@city")] string city,
                [Item(xpath: "@street_address")] string streetAddress)
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

        private class YesNoBoolParser : Parser<bool>
        {
            public override bool Parse(string value)
            {
                switch (value.ToLower())
                {
                    case "yes":
                        return true;
                    case "no":
                        return false;
                    default:
                        throw new Exception("Expected 'yes'/'no' value, but was " + value);
                }
            }
        }
    }
}