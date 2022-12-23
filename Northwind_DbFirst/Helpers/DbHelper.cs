using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using Pluralize.NET;

namespace Northwind_DbFirst.Helpers;

public static class DbHelper
{
    public static List<string> GetTableNames(string databaseName)
    {
        List<string> tableNames = new List<string>();
        string connectionString = $"Server=DESKTOP-MLES57C;Database={databaseName};Integrated Security=true";
        using (SqlConnection connection = new(connectionString))
        {
            SqlCommand cmd = new("SELECT TABLE_NAME\r\nFROM INFORMATION_SCHEMA.TABLES\r\nWHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='NorthwindDB' and TABLE_NAME != 'sysdiagrams'\r\norder by TABLE_NAME", connection);

            connection.Open();

            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                tableNames.Add(sdr["TABLE_NAME"].ToString());
            }

            connection.Close();

            return tableNames;
        }

    }

    public static List<string> GetTableColumns(string databaseName, string tableName)
    {
        List<string> columnNames = new List<string>();
        string connectionString = $"Server=DESKTOP-MLES57C;Database={databaseName};Integrated Security=true";
        using (SqlConnection connection = new(connectionString))
        {
            SqlCommand cmd = new(@$"SELECT CONCAT('public ',IIF(DATA_TYPE = 'nvarchar', 'string' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),IIF(DATA_TYPE = 'uniqueidentifier', 'string' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),IIF(DATA_TYPE = 'ntext', 'string' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),IIF(DATA_TYPE = 'nchar', 'string' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),IIF(DATA_TYPE = 'int', 'int' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),IIF(DATA_TYPE = 'smallint', 'short' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),IIF(DATA_TYPE = 'tinyint', 'byte' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),IIF(DATA_TYPE = 'money', 'decimal' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'float', 'float' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'decimal', 'decimal' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'bit', 'bool' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'datetime', 'DateTime' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'date', 'DateTime' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'datetime2', 'DateTime' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'image', 'byte[]' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 IIF(DATA_TYPE = 'real', 'double' + (IIF(IS_NULLABLE = 'YES', '?', '')), ''),
                 ' ',
                 COLUMN_NAME,
                 ' {{ get; set; }}'
                 + (IIF((DATA_TYPE = 'nvarchar' or DATA_TYPE = 'nchar') and (IS_NULLABLE = 'NO'), ' = null!;', ''))
             ) as COLUMN_NAME
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = N'{tableName}'
                  and TABLE_SCHEMA = 'dbo'", connection);
            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                columnNames.Add(sdr["COLUMN_NAME"].ToString());
            }

            connection.Close();

            return columnNames;
        }
    }

    public static Dictionary<string,List<string>> GetTableNamesWithPropertyColumns(string databaseName)
    {
        Dictionary<string,List<string>> tableWithColumns = new();
        string connectionString = $"Server=DESKTOP-MLES57C;Database={databaseName};Integrated Security=true";
        using (SqlConnection connection = new(connectionString))
        {
            SqlCommand cmd = new("SELECT TABLE_NAME\r\nFROM INFORMATION_SCHEMA.TABLES\r\nWHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='NorthwindDB' and TABLE_NAME != 'sysdiagrams'\r\norder by TABLE_NAME", connection);

            connection.Open();

            SqlDataReader sdr = cmd.ExecuteReader();

            Pluralizer pl = new();
            while (sdr.Read())
            {
                string? tableName = sdr["TABLE_NAME"].ToString();
                string tableNameSingular = pl.Singularize(tableName);
                tableWithColumns.Add(tableNameSingular, GetTableColumns(databaseName, tableName));
            }

            connection.Close();

            return tableWithColumns;
        }

    }
}
