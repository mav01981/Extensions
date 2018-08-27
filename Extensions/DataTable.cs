namespace Extensions
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;

    public static class DataTableExtensions
    {
        public static string ToCSV(this DataTable table)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in table.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            return sb.ToString();
        }
    }
}
