using BackendMonitor.Properties;
using Npgsql;

namespace BackendMonitor.share;

/// <summary>
/// PgConnect
/// </summary>
public class PgConnect {
    private static NpgsqlConnection connection;

    public static PgConnect CreateInstance() {
        return new PgConnect();
    }

    private static string GetConnectionString() {
        return $"Host={Settings.Default.DB_Host};" +
               $"Port={Settings.Default.DB_Port};" +
               $"Username={Settings.Default.DB_User};" +
               $"Password={Settings.Default.DB_Pass};" +
               $"Database={Settings.Default.DB_Name}";
    }

    public static NpgsqlDataReader Read(string sql) {
        Open();
        var command = new NpgsqlCommand(sql, connection);

        return command.ExecuteReader();
    }

    public static int Update(string sql) {
        Open();
        using var command = new NpgsqlCommand(sql, connection);
        var rowsAffected = command.ExecuteNonQuery();
        Close();
        return rowsAffected;
    }

    private static void Open() {
        connection = new NpgsqlConnection(GetConnectionString());
        connection.Open();
    }

    public static void Close() {
        connection.Close();
    }
}