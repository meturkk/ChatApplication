using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Web;

/// <summary>
/// Access veritabanı işlemleri için gelişmiş yardımcı sınıf
/// </summary>
public static class Database
{
    private static readonly string connectionString = GetConfiguredConnectionString();

    private static string GetConfiguredConnectionString()
    {
        string connString;
        try
        {
            connString = ConfigurationManager.ConnectionStrings["AccessDbConnection"]?.ConnectionString;
            if (!string.IsNullOrEmpty(connString))
            {
                // Test connection
                TestConnection(connString);
                return connString;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Config connection failed: {ex.Message}");
        }

        // Fallback connection string
        connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\chat_db.accdb;";
        try
        {
            string dbPath = HttpContext.Current.Server.MapPath("~/App_Data/chat_db.accdb");
            connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";
            TestConnection(connString);
            return connString;
        }
        catch
        {
            throw new Exception("No valid database connection could be established");
        }
    }

    private static void TestConnection(string connectionString)
    {
        using (var conn = new OleDbConnection(connectionString))
        {
            conn.Open(); // Bağlantı testi
            using (var cmd = new OleDbCommand("SELECT 1", conn))
            {
                cmd.ExecuteScalar(); // Basit bir sorgu testi
            }
        }
    }

    #region Senkron Metodlar

    /// <summary>
    /// Sorgu çalıştırır ve sonuçları DataTable olarak döndürür
    /// </summary>
    public static DataTable GetData(string query, params OleDbParameter[] parameters)
    {
        try
        {
            using (var conn = new OleDbConnection(connectionString))
            using (var cmd = new OleDbCommand(query, conn))
            {
                AddParameters(cmd, parameters);
                conn.Open();

                using (var da = new OleDbDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
        catch (Exception ex)
        {
            throw new DatabaseException("Veritabanı sorgulama hatası", ex, query, parameters);
        }
    }

    /// <summary>
    /// Tek bir değer döndüren sorgular için
    /// </summary>
    public static object GetScalar(string query, params OleDbParameter[] parameters)
    {
        try
        {
            using (var conn = new OleDbConnection(connectionString))
            using (var cmd = new OleDbCommand(query, conn))
            {
                AddParameters(cmd, parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            throw new DatabaseException("Scalar sorgu hatası", ex, query, parameters);
        }
    }

    /// <summary>
    /// INSERT, UPDATE, DELETE gibi işlemler için
    /// </summary>
    public static int ExecuteNonQuery(string query, params OleDbParameter[] parameters)
    {
        try
        {
            using (var conn = new OleDbConnection(connectionString))
            using (var cmd = new OleDbCommand(query, conn))
            {
                AddParameters(cmd, parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            throw new DatabaseException("ExecuteNonQuery hatası", ex, query, parameters);
        }
    }

    /// <summary>
    /// Transaction içinde çalıştırılacak sorgular için
    /// </summary>
    public static void ExecuteInTransaction(Action<OleDbTransaction> action)
    {
        using (var conn = new OleDbConnection(connectionString))
        {
            conn.Open();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    action(transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    #endregion

    #region Asenkron Metodlar

    /// <summary>
    /// Sorgu çalıştırır ve sonuçları DataTable olarak döndürür (async)
    /// </summary>
    public static async Task<DataTable> GetDataAsync(string query, params OleDbParameter[] parameters)
    {
        try
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new OleDbCommand(query, conn))
                {
                    AddParameters(cmd, parameters);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new DatabaseException("Veritabanı sorgulama hatası", ex, query, parameters);
        }
    }

    /// <summary>
    /// Tek bir değer döndüren sorgular için (async)
    /// </summary>
    public static async Task<object> GetScalarAsync(string query, params OleDbParameter[] parameters)
    {
        try
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new OleDbCommand(query, conn))
                {
                    AddParameters(cmd, parameters);
                    return await cmd.ExecuteScalarAsync();
                }
            }
        }
        catch (Exception ex)
        {
            throw new DatabaseException("Scalar sorgu hatası", ex, query, parameters);
        }
    }

    /// <summary>
    /// INSERT, UPDATE, DELETE gibi işlemler için (async)
    /// </summary>
    public static async Task<int> ExecuteNonQueryAsync(string query, params OleDbParameter[] parameters)
    {
        try
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new OleDbCommand(query, conn))
                {
                    AddParameters(cmd, parameters);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            throw new DatabaseException("ExecuteNonQuery hatası", ex, query, parameters);
        }
    }

    /// <summary>
    /// Transaction içinde çalıştırılacak async sorgular için
    /// </summary>
    public static async Task ExecuteInTransactionAsync(Func<OleDbTransaction, Task> action)
    {
        using (var conn = new OleDbConnection(connectionString))
        {
            await conn.OpenAsync();
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    await action(transaction);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    #endregion

    #region Yardımcı Metodlar

    /// <summary>
    /// Parametreleri komuta ekler ve null değerleri kontrol eder
    /// </summary>
    private static void AddParameters(OleDbCommand cmd, IEnumerable<OleDbParameter> parameters)
    {
        if (parameters == null) return;

        cmd.Parameters.Clear(); // Önceki parametreleri temizle

        foreach (var param in parameters)
        {
            var newParam = new OleDbParameter(
                param.ParameterName,
                param.OleDbType,
                param.Size,
                param.SourceColumn)
            {
                Value = param.Value ?? DBNull.Value,
                Direction = param.Direction
            };
            cmd.Parameters.Add(newParam);
        }
    }

    /// <summary>
    /// Tablonun son identity değerini alır
    /// </summary>
    public static int GetLastIdentity(string tableName)
    {
        string query = $"SELECT @@IDENTITY FROM {tableName}";
        return Convert.ToInt32(GetScalar(query));
    }

    /// <summary>
    /// Belirtilen tablodaki kayıt sayısını verir
    /// </summary>
    public static int GetRecordCount(string tableName, string whereClause = null)
    {
        string query = $"SELECT COUNT(*) FROM {tableName}";
        if (!string.IsNullOrEmpty(whereClause))
            query += $" WHERE {whereClause}";

        return Convert.ToInt32(GetScalar(query));
    }

    #endregion

    /// <summary>
    /// Özel veritabanı istisna sınıfı
    /// </summary>
    public class DatabaseException : Exception
    {
        public string Query { get; }
        public IEnumerable<OleDbParameter> Parameters { get; }

        public DatabaseException(string message, Exception innerException, string query, IEnumerable<OleDbParameter> parameters)
            : base(message, innerException)
        {
            Query = query;
            Parameters = parameters;
        }

        public override string ToString()
        {
            var paramDetails = new List<string>();
            foreach (var param in Parameters)
            {
                paramDetails.Add($"{param.ParameterName}={param.Value} ({param.OleDbType})");
            }
            return $"{Message}\nQuery: {Query}\nParameters: {string.Join(", ", paramDetails)}\n{InnerException}";
        }
    }
}