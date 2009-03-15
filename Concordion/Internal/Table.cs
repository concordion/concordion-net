using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class Table
    {
        #region Properties
        
        private Element TableElement
        {
            get;
            set;
        }

        private long ColumnCount
        {
            get
            {
                return GetLastHeaderRow().GetCells().Count;
            }
        }

        #endregion

        #region Constructors
        
        public Table(Element element)
        {
            if (!element.IsNamed("table"))
            {
                throw new ArgumentException("This strategy can only work on table elements", "element");
            }

            TableElement = element;
        } 

        #endregion

        #region Methods

        public IList<Row> GetRows()
        {
            IList<Row> rows = new List<Row>();

            foreach (Element rowElement in TableElement.GetDescendantElements("tr")) 
            {
                rows.Add(new Row(rowElement));
            }

            return rows;
        }

        public IList<Row> GetHeaderRows()
        {
            IList<Row> headerRows = new List<Row>();
            foreach (Row row in GetRows()) 
            {
                if (row.IsHeaderRow) 
                {
                    headerRows.Add(row);
                }
            }
            return headerRows;
        }

        public IList<Row> GetDetailRows()
        {
            IList<Row> detailRows = new List<Row>();
            foreach (Row row in GetRows())
            {
                if (!row.IsHeaderRow)
                {
                    detailRows.Add(row);
                }
            }
            return detailRows;
        }

        public Row GetLastHeaderRow()
        {
            IList<Row> headerRows = GetHeaderRows();

            if (headerRows.Count == 0)
            {
                throw new Exception("Table has no header row (i.e. no row containing only <th> elements)");
            }
            else
            {
                return headerRows[headerRows.Count - 1];
            }
        }

        public Row AddDetailRow()
        {
            Element rowElement = new Element("tr");

            Element tbody = TableElement.GetFirstChildElement("tbody");
            if (tbody != null)
            {
                tbody.AppendChild(rowElement);
            }
            else
            {
                TableElement.AppendChild(rowElement);
            }

            for (int i = 0; i < ColumnCount; i++)
            {
                rowElement.AppendChild(new Element("td"));
            }

            return new Row(rowElement);
        }

        #endregion
    }
}
