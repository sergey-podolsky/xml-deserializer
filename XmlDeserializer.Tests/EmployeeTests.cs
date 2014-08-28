namespace XmlDeserializer.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TestDeserializableClasses;
    using XmlDeserializer;

    [TestClass]
    public class EmployeeTests
    {
        private static Employee employee = new Employee();

        [ClassInitialize]
        public static void DeserializeEmployee(TestContext testContext)
        {
            employee.ContactInfo = new Dictionary<string, string> { { "icq", "4770307" } };
            var uri = new Uri(Path.GetFullPath("TestXmlFiles/Employee.xml"));
            new Deserializer().Deserialize(uri, "//employee", ref employee);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("Elizabeth Jones", employee.Name);
        }

        [TestMethod]
        public void Birthday()
        {
            Assert.AreEqual(new DateTime(1989, 3, 16), employee.Birthday);
        }

        [TestMethod]
        public void Birthplace()
        {
            Assert.AreEqual("Unknown", employee.Birthplace);
        }

        [TestMethod]
        public void Gender()
        {
            Assert.AreEqual(Employee.Sex.Female, employee.Gender);
        }

        [TestMethod]
        public void SystemPrivileges()
        {
            Assert.AreEqual(
                Employee.Privilege.Create | Employee.Privilege.Read | Employee.Privilege.Update,
                employee.SystemPrivileges);
        }

        [TestMethod]
        public void IsContractor()
        {
            Assert.IsTrue(employee.IsContractor);
        }

        [TestMethod]
        public void MailingAddress()
        {
            Assert.IsNotNull(employee.MailingAddress);
            Assert.AreEqual(01001, employee.MailingAddress.ZipCode);
            Assert.AreEqual("Ukraine", employee.MailingAddress.Country);
            Assert.AreEqual("Kiev", employee.MailingAddress.City);
            Assert.AreEqual("1, Independence Square", employee.MailingAddress.StreetAddress);
        }

        [TestMethod]
        public void Subordinates()
        {
            Assert.IsNotNull(employee.Subordinates);
            Assert.AreEqual(2, employee.Subordinates.Count);
            Assert.AreEqual("Bob Dickson", employee.Subordinates.First().Name);
            Assert.AreEqual("John Smith", employee.Subordinates.Last().Name);
        }

        [TestMethod]
        public void LineManager()
        {
            Assert.IsNotNull(employee.LineManager);
            Assert.AreEqual("Richard Nelson", employee.LineManager);
        }

        [TestMethod]
        public void ContactInfo()
        {
            Assert.IsNotNull(employee.ContactInfo);
            Assert.AreEqual(4, employee.ContactInfo.Count);
            Assert.AreEqual("icq", "4770307"); // added in before deserialization
            Assert.AreEqual("john.smith@gmail.com", employee.ContactInfo["email"]);
            Assert.AreEqual("+380975459716", employee.ContactInfo["phone"]);
            Assert.AreEqual("john.smith", employee.ContactInfo["skype"]);
        }

        [TestMethod]
        public void Skills()
        {
            Assert.IsNotNull(employee.Skills);
            Assert.AreEqual(2, employee.Skills.Count);
            // languages
            Assert.AreEqual(3, employee.Skills["languages"].Count);
            Assert.AreEqual("csharp: 8 year(s)", employee.Skills["languages"][0]);
            Assert.AreEqual("java: 2 year(s)", employee.Skills["languages"][1]);
            Assert.AreEqual("python: 1 year(s)", employee.Skills["languages"][2]);
            // office
            Assert.AreEqual(2, employee.Skills["office"].Count);
            Assert.AreEqual("word: 7 year(s)", employee.Skills["office"][0]);
            Assert.AreEqual("word: 1 year(s)", employee.Skills["office"][0]);
        }

        [TestMethod]
        public void SpokenLanguages()
        {
            Assert.IsNotNull(employee.SpokenLanguages);
            Assert.AreEqual(3, employee.SpokenLanguages.Count());
            Assert.AreEqual("English", employee.SpokenLanguages[0]);
            Assert.AreEqual("Ukrainian", employee.SpokenLanguages[1]);
            Assert.AreEqual("German", employee.SpokenLanguages[2]);
        }

        [TestMethod]
        public void PersonalDetails()
        {
            Assert.IsNotNull(employee.PersonalDetails);
            Assert.AreEqual(2, employee.PersonalDetails.Count);
            Assert.AreEqual("single", employee.PersonalDetails["Marital status"]);
            Assert.AreEqual("programming, movies, cycling", employee.PersonalDetails["Hobbies"]);
        }
    }
}