namespace MuseumApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var context = new AppDbContext())
            {
                context.InitializeDatabase();
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}