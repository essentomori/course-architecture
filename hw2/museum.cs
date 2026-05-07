/// <summary>
/// Класс, представляющий музей (справочная таблица)
/// </summary>
public class Museum
{
    /// <summary>
    /// Идентификатор музея
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название музея
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    public Museum(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public Museum() : this(0, "") { }

    /// <summary>
    /// Переопределение метода для удобного вывода
    /// </summary>
    public override string ToString() => $"[{Id}] {Name}";
}
