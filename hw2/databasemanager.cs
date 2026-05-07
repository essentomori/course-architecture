using System.IO;
using Microsoft.Data.Sqlite;

/// <summary>
/// Управление базой данных SQLite
/// </summary>
public class DatabaseManager
{
    private readonly string _connectionString;

    /// <summary>
    /// Конструктор с параметром пути к файлу БД
    /// </summary>
    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
    }

    /// <summary>
    /// Создаёт таблицы museum и exhibit
    /// </summary>
    public void CreateTables()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS museum (
                museum_id INTEGER PRIMARY KEY AUTOINCREMENT,
                museum_name TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS exhibit (
                exhibit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                museum_id INTEGER NOT NULL,
                exhibit_name TEXT NOT NULL,
                value_k INTEGER NOT NULL,
                FOREIGN KEY (museum_id) REFERENCES museum(museum_id)
            );";
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Импортирует данные из CSV-файлов
    /// </summary>
    public void ImportFromCsv(string museumCsvPath, string exhibitCsvPath)
    {
        CreateTables();
        if (!string.IsNullOrEmpty(museumCsvPath))
            ImportMuseumsFromCsv(museumCsvPath);
        if (!string.IsNullOrEmpty(exhibitCsvPath))
            ImportExhibitsFromCsv(exhibitCsvPath);
    }

    /// <summary>
    /// Возвращает список всех музеев
    /// </summary>
    public List<Museum> GetAllMuseums()
    {
        var result = new List<Museum>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT museum_id, museum_name FROM museum ORDER BY museum_id";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Museum(reader.GetInt32(0), reader.GetString(1)));
        }
        return result;
    }

    /// <summary>
    /// Возвращает список всех экспонатов
    /// </summary>
    public List<Exhibit> GetAllExhibits()
    {
        var result = new List<Exhibit>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT exhibit_id, museum_id, exhibit_name, value_k FROM exhibit ORDER BY exhibit_id";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Exhibit(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3)
            ));
        }
        return result;
    }

    /// <summary>
    /// Возвращает экспонат по идентификатору или null
    /// </summary>
    public Exhibit GetExhibitById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT exhibit_id, museum_id, exhibit_name, value_k FROM exhibit WHERE exhibit_id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Exhibit(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3)
            );
        }
        return null;
    }

    /// <summary>
    /// Добавляет новый экспонат
    /// </summary>
    public void AddExhibit(Exhibit exhibit)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO exhibit (museum_id, exhibit_name, value_k) VALUES (@museumId, @name, @valueK)";
        cmd.Parameters.AddWithValue("@museumId", exhibit.MuseumId);
        cmd.Parameters.AddWithValue("@name", exhibit.Name);
        cmd.Parameters.AddWithValue("@valueK", exhibit.ValueK);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Обновляет данные экспоната
    /// </summary>
    public void UpdateExhibit(Exhibit exhibit)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "UPDATE exhibit SET museum_id = @museumId, exhibit_name = @name, value_k = @valueK WHERE exhibit_id = @id";
        cmd.Parameters.AddWithValue("@museumId", exhibit.MuseumId);
        cmd.Parameters.AddWithValue("@name", exhibit.Name);
        cmd.Parameters.AddWithValue("@valueK", exhibit.ValueK);
        cmd.Parameters.AddWithValue("@id", exhibit.Id);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Удаляет экспонат по идентификатору
    /// </summary>
    public void DeleteExhibit(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM exhibit WHERE exhibit_id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Выполняет SQL-запрос и возвращает результат
    /// </summary>
    public (string[] columns, List<string[]> rows) ExecuteQuery(string sql)
    {
        string[] columns;
        List<string[]> rows = new List<string[]>();

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        using var reader = cmd.ExecuteReader();

        columns = new string[reader.FieldCount];
        for (int i = 0; i < reader.FieldCount; i++)
            columns[i] = reader.GetName(i);

        while (reader.Read())
        {
            string[] row = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                row[i] = reader.GetValue(i)?.ToString() ?? "";
            rows.Add(row);
        }
        return (columns, rows);
    }

    private void ImportMuseumsFromCsv(string path)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(';');
            if (parts.Length < 2) continue;
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO museum (museum_id, museum_name) VALUES (@id, @name)";
            cmd.Parameters.AddWithValue("@id", int.Parse(parts[0]));
            cmd.Parameters.AddWithValue("@name", parts[1]);
            cmd.ExecuteNonQuery();
        }
    }

    private void ImportExhibitsFromCsv(string path)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string[] lines = File.ReadAllLines(path);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(';');
            if (parts.Length < 4) continue;
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO exhibit (exhibit_id, museum_id, exhibit_name, value_k) VALUES (@id, @museumId, @name, @valueK)";
            cmd.Parameters.AddWithValue("@id", int.Parse(parts[0]));
            cmd.Parameters.AddWithValue("@museumId", int.Parse(parts[1]));
            cmd.Parameters.AddWithValue("@name", parts[2]);
            cmd.Parameters.AddWithValue("@valueK", int.Parse(parts[3]));
            cmd.ExecuteNonQuery();
        }
    }
}