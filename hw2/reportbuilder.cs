using System;
using System.Text;

/// <summary>
/// Построитель отчётов (Fluent Interface)
/// </summary>
public class ReportBuilder
{
    private readonly DatabaseManager _db;
    private string _sql = "";
    private string _title = "";
    private string[] _headers = Array.Empty<string>();
    private int[] _widths = Array.Empty<int>();
    private bool _numbered = false;

    /// <summary>
    /// Конструктор
    /// </summary>
    public ReportBuilder(DatabaseManager db)
    {
        _db = db;
    }

    /// <summary>
    /// Задаёт SQL-запрос
    /// </summary>
    public ReportBuilder Query(string sql)
    {
        _sql = sql;
        return this;
    }

    /// <summary>
    /// Задаёт заголовок
    /// </summary>
    public ReportBuilder Title(string title)
    {
        _title = title;
        return this;
    }

    /// <summary>
    /// Задаёт названия колонок
    /// </summary>
    public ReportBuilder Header(params string[] columns)
    {
        _headers = columns;
        return this;
    }

    /// <summary>
    /// Задаёт ширину колонок
    /// </summary>
    public ReportBuilder ColumnWidths(params int[] widths)
    {
        _widths = widths;
        return this;
    }

    /// <summary>
    /// Включает нумерацию строк
    /// </summary>
    public ReportBuilder Numbered()
    {
        _numbered = true;
        return this;
    }

    /// <summary>
    /// Формирует отчёт и возвращает строку
    /// </summary>
    public string Build()
    {
        var (columns, rows) = _db.ExecuteQuery(_sql);
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(_title))
        {
            sb.AppendLine($"=== {_title} ===");
        }

        string[] displayHeaders = _headers.Length > 0 ? _headers : columns;
        int colCount = displayHeaders.Length;

        int[] widths;
        if (_widths.Length >= colCount)
        {
            widths = new int[colCount];
            Array.Copy(_widths, widths, colCount);
        }
        else
        {
            widths = new int[colCount];
            for (int i = 0; i < colCount; i++)
                widths[i] = 20;
        }

        if (_numbered)
            sb.Append("№".PadRight(5));

        for (int i = 0; i < colCount; i++)
            sb.Append(displayHeaders[i].PadRight(widths[i]));
        sb.AppendLine();

        int totalWidth = (_numbered ? 5 : 0);
        for (int i = 0; i < colCount; i++)
            totalWidth += widths[i];
        sb.AppendLine(new string('─', totalWidth));

        for (int r = 0; r < rows.Count; r++)
        {
            if (_numbered)
                sb.Append((r + 1).ToString().PadRight(5));

            for (int c = 0; c < rows[r].Length && c < colCount; c++)
            {
                sb.Append(rows[r][c].PadRight(widths[c]));
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Выводит отчёт в консоль
    /// </summary>
    public void Print()
    {
        Console.WriteLine(Build());
    }
}