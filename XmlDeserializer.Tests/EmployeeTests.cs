using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlDeserializer.Tests
{
    using XmlDeserializer;
    using TestDeserializableClasses;
    using System.IO;

    [TestClass]
    public class EmployeeTests
    {
        private static Employee employee = new Employee { Title = "Software Engineer" };

        [ClassInitialize]
        public static void DeserializeEmployee(TestContext testContext)
        {
            object deserializable = employee;
            var url = new Uri(Path.GetFullPath("TestXmlFiles/Employee.xml"));
            new Deserializer().Deserialize(url, "//employee", ref deserializable);
        }

        [TestMethod]
        public void Name()
        {
        }
    }
}
