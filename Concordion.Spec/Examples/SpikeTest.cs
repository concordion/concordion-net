using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Examples
{
    public class SpikeTest
    {
        public string GetGreetingFor(string name) 
        {
            return "Hello " + name + "!";
        }
        
        public void DoSomething() 
        {
        }
        
        public ICollection<Person> GetPeople() 
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
