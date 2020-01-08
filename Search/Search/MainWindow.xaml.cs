using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;


namespace Search
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SearchClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearResultsClick(sender, e);

                string file = this.NameFileText.Text;
                if (file == " ") throw new Exception("Не введено ім'я файлу.");
                file = file.TrimStart();

                string str = this.ReviewText.Text;
                if (str == " ") throw new Exception("Некоректний шлях.");
                str = str.TrimStart();
                DirectoryInfo dir = new DirectoryInfo(@str);

                bool nullSearch = false;
                FindInDir(dir, file, (bool)this.SearchInSubfolders.IsChecked, ref nullSearch);
                if (nullSearch == false)
                {
                    this.Result.Items.Add("Файл не знайдено");
                }
            }
            catch (Exception ex)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show(ex.Message);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Close();
                }

            }
        }

        private void FindInDir(DirectoryInfo dir, string pattern, bool recursive, ref bool nullSearch)
        {
            foreach (var file in dir.GetFiles(pattern))
            {
                this.Result.Items.Add(file.FullName);
                nullSearch = true;
            }

            if (recursive)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    this.FindInDir(subdir, pattern, recursive, ref nullSearch);
                }
            }
        }

        private void ReviewClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ReviewText.Text = " " + fbd.SelectedPath;
            }
            
        }

        private void ClearResultsClick(object sender, RoutedEventArgs e)
        {
            this.Result.Items.Clear();
        }

        private void ClearEverythingClick(object sender, RoutedEventArgs e)
        {
            this.NameFileText.Text = " ";
            this.ReviewText.Text = " ";
            this.Result.Items.Clear();
        }

        private void ResultMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(this.Result.SelectedItem.ToString());
        }
    }
}
