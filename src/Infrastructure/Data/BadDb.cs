using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

public static class BadDb
{
    private static readonly string _connectionString =
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        ?? "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

    public static string ConnectionString => _connectionString;
    public static int ExecuteNonQueryUnsafe(string sql)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }
    public static DataTable ExecuteReaderUnsafe(string sql)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        using var adapter = new SqlDataAdapter(cmd);

        var dataTable = new DataTable();
        conn.Open();
        adapter.Fill(dataTable);

        return dataTable;
    }
}