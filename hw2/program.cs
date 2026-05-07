using System;
using System.IO;
using System.Text;

namespace MuseumCatalog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            string dbPath = "museums.db";
            string museumCsv = Path.Combine(AppContext.BaseDirectory, "museums.csv");
            string exhibitCsv = Path.Combine(AppContext.BaseDirectory, "exhibits.csv");

            var db = new DatabaseManager(dbPath);
            db.CreateTables();

            if (File.Exists(museumCsv) || File.Exists(exhibitCsv))
            {
                db.ImportFromCsv(museumCsv, exhibitCsv);
                Console.WriteLine("[OK] Данные загружены из CSV-файлов");
            }

            string choice = "";
            while (choice != "0")
            {
                Console.WriteLine("\n=== УПРАВЛЕНИЕ ЭКСПОНАТАМИ ===");
                Console.WriteLine("1 - Показать все музеи");
                Console.WriteLine("2 - Показать все экспонаты");
                Console.WriteLine("3 - Добавить экспонат");
                Console.WriteLine("4 - Редактировать экспонат");
                Console.WriteLine("5 - Удалить экспонат");
                Console.WriteLine("6 - Отчёты");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");
                choice = Console.ReadLine()?.Trim() ?? "";

                if (choice == "1")
                {
                    ShowAllMuseums(db);
                }
                else if (choice == "2")
                {
                    ShowAllExhibits(db);
                }
                else if (choice == "3")
                {
                    AddExhibit(db);
                }
                else if (choice == "4")
                {
                    EditExhibit(db);
                }
                else if (choice == "5")
                {
                    DeleteExhibit(db);
                }
                else if (choice == "6")
                {
                    ReportsMenu(db);
                }
                else if (choice == "0")
                {
                }
                else
                {
                    Console.WriteLine("Неверный пункт меню.");
                }
            }
        }

        static void ShowAllMuseums(DatabaseManager db)
        {
            Console.WriteLine("\n---- Все музеи ----");
            var museums = db.GetAllMuseums();
            foreach (var m in museums)
                Console.WriteLine($"  {m}");
            Console.WriteLine($"Итого: {museums.Count} музеев");
        }

        static void ShowAllExhibits(DatabaseManager db)
        {
            Console.WriteLine("\n---- Все экспонаты ----");
            var exhibits = db.GetAllExhibits();
            foreach (var ex in exhibits)
                Console.WriteLine($"  {ex}");
            Console.WriteLine($"Итого: {exhibits.Count} экспонатов");
        }

        static void AddExhibit(DatabaseManager db)
        {
            Console.WriteLine("\n---- Добавление экспоната ----");
            Console.WriteLine("Доступные музеи:");
            var museums = db.GetAllMuseums();
            foreach (var m in museums)
                Console.WriteLine($"  {m}");

            Console.Write("ID музея: ");
            if (!int.TryParse(Console.ReadLine(), out int museumId))
            {
                Console.WriteLine("Ошибка: введите целое число");
                return;
            }

            Console.Write("Название экспоната: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Оценочная стоимость (тыс. руб.): ");
            if (!int.TryParse(Console.ReadLine(), out int valueK))
            {
                Console.WriteLine("Ошибка: введите целое число");
                return;
            }
            if (valueK < 0)
            {
                Console.WriteLine("Ошибка: стоимость не может быть отрицательной");
                return;
            }

            var exhibit = new Exhibit(0, museumId, name, valueK);
            db.AddExhibit(exhibit);
            Console.WriteLine("Экспонат успешно добавлен.");
        }

        static void EditExhibit(DatabaseManager db)
        {
            Console.WriteLine("\n---- Редактирование экспоната ----");
            Console.Write("Введите ID экспоната: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка ввода ID");
                return;
            }

            var exhibit = db.GetExhibitById(id);
            if (exhibit == null)
            {
                Console.WriteLine($"Экспонат с ID={id} не найден.");
                return;
            }

            Console.WriteLine($"Текущие данные: {exhibit}");
            Console.WriteLine("(Нажмите Enter, чтобы оставить без изменений)");

            Console.Write($"Название [{exhibit.Name}]: ");
            string input = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(input)) exhibit.Name = input;

            Console.Write($"ID музея [{exhibit.MuseumId}]: ");
            input = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input, out int newId))
                    exhibit.MuseumId = newId;
                else
                    Console.WriteLine("Ошибка: введено не число, оставлено прежнее значение");
            }

            Console.Write($"Стоимость (тыс. руб.) [{exhibit.ValueK}]: ");
            input = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input, out int newValue))
                {
                    if (newValue >= 0)
                        exhibit.ValueK = newValue;
                    else
                        Console.WriteLine("Ошибка: стоимость не может быть отрицательной, оставлено прежнее значение");
                }
                else
                    Console.WriteLine("Ошибка: введено не число, оставлено прежнее значение");
            }

            db.UpdateExhibit(exhibit);
            Console.WriteLine("Данные обновлены.");
        }

        static void DeleteExhibit(DatabaseManager db)
        {
            Console.WriteLine("\n---- Удаление экспоната ----");
            Console.Write("Введите ID экспоната: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка ввода ID");
                return;
            }

            var exhibit = db.GetExhibitById(id);
            if (exhibit == null)
            {
                Console.WriteLine($"Экспонат с ID={id} не найден.");
                return;
            }

            Console.Write($"Удалить \"{exhibit.Name}\"? (да/нет): ");
            string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";
            if (confirm == "да" || confirm == "y")
            {
                db.DeleteExhibit(id);
                Console.WriteLine("Экспонат удалён.");
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }
        }

        static void ReportsMenu(DatabaseManager db)
        {
            string reportChoice = "";
            while (reportChoice != "0")
            {
                Console.WriteLine("\n--- Отчёты ---");
                Console.WriteLine("1 - Экспонаты по музеям");
                Console.WriteLine("2 - Количество экспонатов по музеям");
                Console.WriteLine("3 - Средняя стоимость по музеям");
                Console.WriteLine("0 - Назад");
                Console.Write("Ваш выбор: ");
                reportChoice = Console.ReadLine()?.Trim() ?? "";

                if (reportChoice == "1")
                {
                    new ReportBuilder(db)
                        .Query(@"SELECT e.exhibit_name, m.museum_name, e.value_k
                                 FROM exhibit e
                                 JOIN museum m ON e.museum_id = m.museum_id
                                 ORDER BY e.exhibit_name")
                        .Title("Экспонаты по музеям")
                        .Header("Название экспоната", "Музей", "Стоимость (тыс. руб.)")
                        .ColumnWidths(35, 25, 20)
                        .Numbered()
                        .Print();
                }
                else if (reportChoice == "2")
                {
                    new ReportBuilder(db)
                        .Query(@"SELECT m.museum_name, COUNT(*) AS count
                                 FROM exhibit e
                                 JOIN museum m ON e.museum_id = m.museum_id
                                 GROUP BY m.museum_name
                                 ORDER BY m.museum_name")
                        .Title("Количество экспонатов по музеям")
                        .Header("Музей", "Количество")
                        .ColumnWidths(40, 15)
                        .Numbered()
                        .Print();
                }
                else if (reportChoice == "3")
                {
                    new ReportBuilder(db)
                        .Query(@"SELECT m.museum_name, ROUND(AVG(e.value_k), 1) AS avg_value
                                 FROM exhibit e
                                 JOIN museum m ON e.museum_id = m.museum_id
                                 GROUP BY m.museum_name
                                 ORDER BY avg_value DESC")
                        .Title("Средняя стоимость экспонатов по музеям")
                        .Header("Музей", "Средняя стоимость (тыс. руб.)")
                        .ColumnWidths(40, 25)
                        .Numbered()
                        .Print();
                }
                else if (reportChoice == "0")
                {
                }
                else
                {
                    Console.WriteLine("Неверный пункт меню.");
                }
            }
        }
    }
}