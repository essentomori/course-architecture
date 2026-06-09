using System.Collections.Generic;

namespace MuseumApp
{
    /// <summary>
    /// Музей (справочная таблица, сторона "один")
    /// </summary>
    public class Museum
    {
        /// <summary>Идентификатор музея (первичный ключ)</summary>
        public int Id { get; set; }

        /// <summary>Название музея</summary>
        public string Name { get; set; } = "";

        /// <summary>Навигационное свойство: экспонаты этого музея</summary>
        public ICollection<Exhibit> Exhibits { get; set; } = new List<Exhibit>();
    }

    /// <summary>
    /// Экспонат (основная таблица, сторона "много")
    /// </summary>
    public class Exhibit
    {
        /// <summary>Идентификатор экспоната (первичный ключ)</summary>
        public int Id { get; set; }

        /// <summary>Идентификатор музея (внешний ключ)</summary>
        public int MuseumId { get; set; }

        /// <summary>Навигационное свойство: музей, которому принадлежит экспонат</summary>
        public Museum? Museum { get; set; }

        /// <summary>Название экспоната</summary>
        public string Name { get; set; } = "";

        /// <summary>Оценочная стоимость (тыс. руб.)</summary>
        public double ValueK { get; set; }
    }
}