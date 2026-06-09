using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace MuseumApp
{
    /// <summary>
    /// Контекст базы данных для работы с музеями и экспонатами
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Museum> Museums { get; set; }
        public DbSet<Exhibit> Exhibits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=museum.db");

        /// <summary>
        /// Создание базы данных и заполнение начальными данными
        /// </summary>
        public void InitializeDatabase()
        {
            bool created = Database.EnsureCreated();

            if (created && !Museums.Any())
            {
                SeedData();
            }
        }

        private void SeedData()
        {
            if (File.Exists("museum.csv"))
            {
                var museums = File.ReadAllLines("museum.csv")
                    .Skip(1)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(';'))
                    .Select(parts => new Museum
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1].Trim('"')
                    });

                Museums.AddRange(museums);
                SaveChanges();
            }

            if (File.Exists("expo.csv"))
            {
                var exhibits = File.ReadAllLines("expo.csv")
                    .Skip(1)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(';'))
                    .Select(parts => new Exhibit
                    {
                        Id = int.Parse(parts[0]),
                        MuseumId = int.Parse(parts[1]),
                        Name = parts[2].Trim('"'),
                        ValueK = double.Parse(parts[3])
                    });

                Exhibits.AddRange(exhibits);
                SaveChanges();
            }
        }
    }
}