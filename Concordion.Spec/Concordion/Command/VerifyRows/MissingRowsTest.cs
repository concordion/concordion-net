using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    [ConcordionTest]
    public class MissingRowsTest
    {
        private List<Person> people = new List<Person>();

        public void addPerson(string firstName, string lastName, int birthYear)
        {
            people.Add(new Person(firstName, lastName, birthYear));
        }

        public string getOutputFragment(string inputFragment)
        {
            var document = new TestRig()
                                .WithFixture(this)
                                .ProcessFragment(inputFragment)
                                .GetXDocument();

            var tables = document.Descendants("table");

            foreach (var table in tables)
            {
                // stops loop after first entry, simulating the java code.
                return table.ToString().Replace("\u00A0", "&#160;");
            }

            return String.Empty;
        }

        public ICollection<Person> getPeople()
        {
            return people;
        }

        public class Person
        {

            public Person(string firstName, string lastName, int birthYear)
            {
                this.firstName = firstName;
                this.lastName = lastName;
                this.birthYear = birthYear;
            }

            public string firstName;
            public string lastName;
            public int birthYear;
        }
    }
}
