using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class SQLExtension
    {
        private static string BuildProcedure(this string procedureName, params SqlParameter[] parameters)
        {
            StringBuilder query = new StringBuilder();
            query.Append("EXEC dbo.[");
            query.Append(procedureName);
            query.Append("] ");

            query.Append(string.Join(", ", parameters.Select(p =>
                "@" + p.ParameterName + " = " +
                "@" + p.ParameterName +
                (p.Direction == ParameterDirection.Output ? " OUTPUT" : "")
                )));

            return query.ToString();
        }
    }
}
