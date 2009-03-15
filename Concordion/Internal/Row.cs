using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class Row
    {
        #region Properties

        public Element RowElement
        {
            get;
            private set;
        }

        public bool IsHeaderRow
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Constructors

        public Row(Element element)
        {
            RowElement = element;
        }

        #endregion

        #region Methods

        public IList<Element> GetCells()
        {
            IList<Element> cells = new List<Element>();
            foreach (Element childElement in RowElement.GetChildElements())
            {
                if (childElement.IsNamed("td") || childElement.IsNamed("th")) 
                {
                    cells.Add(childElement);
                }
            }
            return cells;
        }

        public int GetIndexOfCell(Element element)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
