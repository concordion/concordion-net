using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Integration;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    [ConcordionTest]
    public class SurplusRowsTest : MissingRowsTest
    {
        public void addPerson(string firstName, string lastName)
        {
            base.addPerson(firstName, lastName, 1973);
        }
    }
}
