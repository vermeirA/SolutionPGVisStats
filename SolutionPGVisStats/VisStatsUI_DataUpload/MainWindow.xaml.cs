using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisStatsBL;
using VisStatsBL.Interfaces;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_DataUpload
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Initialiseren van file upload venster (standaard functie in windows)

        OpenFileDialog dialog = new OpenFileDialog();

        string conn = "Data Source=LTAV\\SQLEXPRESS;Initial Catalog=VisStats;Integrated Security=True;Trust Server Certificate=True";
        IFileProcessor fileProcessor;
        IVisStatsRepository visStatsRepository;
        VisStatsManager visStatsManager;


        public MainWindow()
        {
            InitializeComponent();

            // Specificaties van het file upload venster (default type, filterwaarden, start directory, meerdere files toegelaten..)
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.InitialDirectory = @"C:\Users\Gebruiker\Desktop\Graduaat\SEM3\Programmeren gevorderd 1\Project 1\Vis";
            dialog.Multiselect = true;

            // connectie met databank en fileprocessor
            fileProcessor = new FileProcessor();
            visStatsRepository = new VisStatsRepository(conn);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);

        }

        private void Button_Click_Vissoorten(object sender, RoutedEventArgs e)
        {
            // result = wat wordt er in het venster aangeduid? true = ok, false = cancel
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var fileNames = dialog.FileNames;
                VissoortenFileListBox.ItemsSource = fileNames;
                dialog.FileName = null;
            }
            else VissoortenFileListBox.ItemsSource = null;
        }

        private void Button_Click_UploadVissoorten(object sender, RoutedEventArgs e)
        {
            // elke file uploaden via UploadVissoorten (uit VisStatsManager)
            foreach (string fileName in VissoortenFileListBox.ItemsSource)
            {
                visStatsManager.UploadVissoorten(fileName);
            }
            //Upload confirmatie op scherm
            MessageBox.Show("Upload gereed", "VisStats");
        }

        private void Button_Click_Havens(object sender, RoutedEventArgs e)
        {
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var fileNames = dialog.FileNames;
                HavensFileListBox.ItemsSource = fileNames;
                dialog.FileName= null;
            } 
            else HavensFileListBox.ItemsSource = null;
        }

        private void Button_Click_UploadHavens(object sender, RoutedEventArgs e)
        {
            foreach (string fileName in HavensFileListBox.ItemsSource)
            {
                visStatsManager.UploadHavens(fileName);
            }

            MessageBox.Show("Upload gereed", "HavenStats");
        }
    }
}