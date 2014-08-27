using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlDeserializer.Tests.TestDeserializableClasses
{
    [Deserializable] // Marks user class as deserializable
    class Employee
    {
        // mandatory value
        [Item(xpath: "name", IsRequired = true)]
        public string Name { get; set; }

        // optional value
        [Item(xpath: "title")]
        public string Title { get; set; }

        // with custom format
        [Item(xpath: "birthday", Format = "dd-MM-yyyy")]
        public DateTime Birthday { get; set; }

        // nested class 
        [Item(xpath: "mailing_address")]
        public Address MailingAddress { get; set; }

        // enum
        [Item(xpath: "gender")]
        public Sex Gender { get; set; }

        // enum flags
        [Item(xpath: "system_privileges/privilege")]
        public SqlPrivilege SystemPrivileges { get; set; }

        // collection (can be empty)
        [Item(xpath: "subordinates/subordinate")]
        public ICollection<Employee> Subordinates { get; set; }

        [XmlUri(xpath: "line_manager/@link")]
        [Item(xpath: "//Employee")]
        public Employee LineManager { get; set; }

        // dictionary with non-null values
        [Dictionary(entryXPath: "contact_info/*", keyXPath: "name()", valueXPath: ".", IsValueRequired = true)]
        public Dictionary<string, string> ContactInfo { get; set; }

        // dictionary where values are non-empty lists
        [Dictionary(entryXPath: "skills/*", keyXPath: "name()", valueXPath: "@*/concat(name(), '=', .)", IsValueRequired = true)]
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

            [Constructor] // tells which constructor to use for instantiation
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
        public enum SqlPrivilege
        {
            [Synonyms("Create")]
            Insert,
            [Synonyms("Read", "Retrieve")]
            Select,
            [Synonyms("Update", "Modify")]
            Update,
            [Synonyms("Delete", "Destroy")]
            Delete
        }
    }
}
