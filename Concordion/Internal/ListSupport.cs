using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class ListSupport
    {
        #region Properties

        private CommandCall ListCommandCall
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public ListSupport(CommandCall listCommandCall)
        {
            if (!(listCommandCall.Element.IsNamed("ol") || listCommandCall.Element.IsNamed("ul")))
            {
                throw new ArgumentException("This strategy can only work on list elements", "listCommandCall");
            }

            ListCommandCall = listCommandCall;
        }

        #endregion

        #region Methods

        public IList<Element> GetListItemElements()
        {
            List<Element> listItemElements = new List<Element>();
            foreach (Element itemElement in ListCommandCall.Element.GetDescendantElements("li"))
            {
                listItemElements.Add(itemElement);
            }
            return listItemElements;
        }

        public List<Element> GetListElements()
        {
            List<Element> listElements = new List<Element>();
            foreach (Element listElement in ListCommandCall.Element.GetDescendantElements("ul"))
            {
                listElements.Add(listElement);
            }
            foreach (Element listElement in ListCommandCall.Element.GetDescendantElements("ol"))
            {
                listElements.Add(listElement);
            }
            return listElements;
        }

        #endregion
    }
}
