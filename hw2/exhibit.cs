/// <summary>
/// Класс, представляющий экспонат музея
/// </summary>
public class Exhibit
{
    /// <summary>
    /// Идентификатор экспоната
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор музея (внешний ключ)
    /// </summary>
    public int MuseumId { get; set; }

    /// <summary>
    /// Название экспоната
    /// </summary>
    public string Name { get; set; }

    private int _valueK;

    /// <summary>
    /// Оценочная стоимость в тыс. руб. (не может быть отрицательной)
    /// </summary>
    public int ValueK
    {
        get => _valueK;
        set
        {
            if (value < 0)
                throw new ArgumentException("Оценочная стоимость не может быть отрицательной");
            _valueK = value;
        }
    }

    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    public Exhibit(int id, int museumId, string name, int valueK)
    {
        Id = id;
        MuseumId = museumId;
        Name = name;
        ValueK = valueK;
    }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public Exhibit() : this(0, 0, "", 0) { }

    /// <summary>
    /// Переопределение метода для удобного вывода
    /// </summary>
    public override string ToString()
        => $"[{Id}] {Name}, музей #{MuseumId}, стоимость: {ValueK} тыс. руб.";
}
