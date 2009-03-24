using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordion.Spec.Concordion.Command.VerifyRows
{
    class SurplusRowsTest : MissingRowsTest
    {
        public void addPerson(string firstName, string lastName)
        {
            base.addPerson(firstName, lastName, 1973);
        }
    }
}
