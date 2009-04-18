using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Examples
{
    [ConcordionTest]
    public class SpikeTest
    {
        public string getGreetingFor(string name) 
        {
            return "Hello " + name + "!";
        }
        
        public void doSomething() 
        {
        }
        
        public ICollection<Person> getPeople() 
        {
            return new List<Person> { new Person("John", "Travolta") };
        }
        
        public class Person 
        {
            public Person(string firstName, string lastName) 
            {
                this.FirstName = firstName;
                this.LastName = lastName;
            }
            
            public string FirstName;
            public string LastName;
        } 
    }
}
