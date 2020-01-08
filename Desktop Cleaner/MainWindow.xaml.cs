using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SQLite;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Desktop_Cleaner {
    public partial class MainWindow : Window {
        #region Window

        public MainWindow() {
            InitializeComponent();
            Init();
        }
        #endregion

        #region Button_clicks
        private void Clean_Button_Click(object sender, RoutedEventArgs e) {
            //gre po podatkih in premakne kar mora in kaj ne
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e) {
            //zbrise označeni file name

            Remove_file();
        }

        private void Add_Button_Button_Click(object sender, RoutedEventArgs e) {
            String File_name = String.Empty;

            //todo add gumb za file in folder dialog rak
            try{
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog {
                    InitialDirectory = "Desktop"
            };

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    File_name = dlg.FileName;
                    System.Windows.MessageBox.Show(File_name);
                    Add_file(File_name);
                }
            }
            catch (Exception ex){
                System.Windows.MessageBox.Show("FileDialog Error: " + ex, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_button_folder(object sender, RoutedEventArgs e) {
            String File_name = String.Empty;

                //todo add gumb za file in folder dialog rak
                try {
                FolderBrowserDialog lol = new FolderBrowserDialog {
                    SelectedPath = "Desktop"
                };

                if (lol.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    System.Windows.MessageBox.Show("You selected: " + lol.SelectedPath);
                    File_name = lol.SelectedPath;
                    Add_file(File_name);
                }
            }
            catch (Exception ex){
                System.Windows.MessageBox.Show("FolderDialog Error: " + ex, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion











        #region Init

        private void Init() {
            Database_innit();
            Listbox_innit();
            List_Files.Items.Add("1");
            List_Files.Items.Add("2");
            List_Files.Items.Add("3");
            List_Files.Items.Add("4");

        }
        //TODO naredi da z db pobere imena filow in da v list box



        #endregion

        #region Functions
        private void Add_file(String file_name) {
            string prilepi = string.Empty;

            using (SQLiteCommand cmd = new SQLiteCommand(sqlite_conn)) {

                cmd.CommandText = @"INSERT INTO File_names (name) VALUES (@datoteka);";
                cmd.Parameters.AddWithValue("@datoteka", file_name);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                //List_Files.Items.Add("asg");
                prilepi = Refactor_string(file_name);
                List_Files.Items.Add(prilepi);

            }

        }

        private void Remove_file() {
            String item = string.Empty;
            try {
                string item = List_Files.SelectedItem.ToString();

                List_Files.Items.RemoveAt(List_Files.SelectedIndex);
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show("You need to select a line to delete it.", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
                Console.WriteLine(ex);
            }


            //System.Windows.MessageBox.Show(item, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);



        }


        // more biti vzunaj da druga metoda to najde
        SQLiteConnection sqlite_conn = null;
        private void Database_innit() {
            sqlite_conn = new SQLiteConnection("Data Source=../../Data/Data.db; Version = 3");
            try {
                sqlite_conn.Open();
                System.Console.WriteLine("ok dela boi");
            }
            catch (Exception ex) {
                System.Console.WriteLine(ex);
                sqlite_conn.Close();
                System.Windows.MessageBox.Show("Database error: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
            }
        }


        private void Listbox_innit() {

        }
        private String Refactor_string(String name) {
            string[] splitan = name.Split('\\');
            return splitan[4];
        }






        #endregion


    }
}
