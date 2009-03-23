using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    class MissingRowsTest
    {
        private List<Person> people = new List<Person>();

        public void addPerson(string firstName, string lastName, int birthYear)
        {
            people.Add(new Person(firstName, lastName, birthYear));
        }

        public string getOutputFragment(string inputFragment)
        {
            return new TestRig()
                .WithFixture(this)
                .ProcessFragment(inputFragment).SuccessOrFailureInWords();
                //.GetXOMDocument()
                //.Query("//table").get(0)
                //.ToXML()
                //.Replace("\u00A0", "&#160;");
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
