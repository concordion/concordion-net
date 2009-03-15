using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordion.Api;

namespace Concordion.Internal
{
    public class TableSupport
    {
        #region Properties

        private CommandCall TableCommandCall
        {
            get;
            set;
        }

        private Table Table
        {
            get;
            set;
        }

        private IDictionary<int, CommandCall> CommandCallByColumn
        {
            get;
            set;
        }

        public long ColumnCount
        {
            get
            {
                return GetLastHeaderRow().GetCells().Count;
            }
        }
        
        #endregion
        
        #region Constructors

        public TableSupport(CommandCall tableCommandCall)
        {
            if (!tableCommandCall.Element.IsNamed("table"))
            {
                throw new ArgumentException("This strategy can only work on table elements", "tableCommandCall");
            }

            TableCommandCall = tableCommandCall;
            Table = new Table(tableCommandCall.Element);

            CommandCallByColumn = new Dictionary<int, CommandCall>();
            PopulateCommandCallByColumnMap();
        }

        #endregion

        #region Methods

        private void PopulateCommandCallByColumnMap()
        {
            Row headerRow = GetLastHeaderRow();
            CommandCallList children = TableCommandCall.Children;
            foreach (CommandCall childCall in children)
            {
                int columnIndex = headerRow.GetIndexOfCell(childCall.Element);
                if (columnIndex == -1)
                {
                    throw new Exception("Commands must be placed on <th> elements when using 'execute' or 'verifyRows' commands on a <table>.");
                }
                CommandCallByColumn.Add(columnIndex, childCall);
            }
        }

        public void CopyCommandCallsTo(Row detailRow)
        {
            int columnIndex = 0;
            foreach (Element cell in detailRow.GetCells()) 
            {
                CommandCall cellCall;
                if (CommandCallByColumn.TryGetValue(columnIndex, out cellCall)) 
                {
                    cellCall.Element = cell;
                }
                columnIndex++;
            }
        }

        public IList<Row> GetDetailRows()
        {
            return Table.GetDetailRows();
        }

        public Row AddDetailRow()
        {
            return Table.AddDetailRow();
        }

        public Row GetLastHeaderRow()
        {
            return Table.GetLastHeaderRow();
        }

        #endregion

    }
}
