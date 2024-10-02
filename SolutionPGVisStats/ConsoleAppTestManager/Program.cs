using VisStatsBL;
using VisStatsBL.Interfaces;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace ConsoleAppTestManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string conn = "Data Source=LTAV\\SQLEXPRESS;Initial Catalog=VisStats;Integrated Security=True;Trust Server Certificate=True";

            string filePath = @"C:\Users\Gebruiker\Desktop\Graduaat\SEM3\Programmeren gevorderd 1\Project 1\Vis\vissoorten1.txt";

            IFileProcessor fp = new FileProcessor();
            IVisStatsRepository repo = new VisStatsRepository(conn);

            VisStatsManager vm = new VisStatsManager(fp, repo);
            vm.UploadVissoorten(filePath);
        }
    }
}
