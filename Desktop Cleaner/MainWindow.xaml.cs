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
            CheckBox_save();
            clean_desek();

        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e) {
            //zbrise označeni file name

            Remove_file();
        }

        private void Add_Button_Button_Click(object sender, RoutedEventArgs e) {
            String File_name = String.Empty;

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

                try {
                FolderBrowserDialog lol = new FolderBrowserDialog {
                    //RootFolder = Environment.SpecialFolder.Desktop,
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
            CheckBox_set();
        }

        #endregion

        #region Functions
        private void Add_file(String file_name) {
            string prilepi = string.Empty;

            try {
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
            catch(Exception ex) {
                System.Windows.MessageBox.Show("Database error in adding file name to database " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Remove_file() {
            String item = string.Empty;
            String full_item = string.Empty;
            String desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try {
                item = List_Files.SelectedItem.ToString();

                List_Files.Items.RemoveAt(List_Files.SelectedIndex);
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show("You need to select a line to delete it.", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
                Console.WriteLine(ex);
            }

            full_item = desktop_path +'\\'+ item;
            Console.WriteLine(full_item);

            try {
                using (SQLiteCommand cmd = new SQLiteCommand(sqlite_conn)) {

                    cmd.CommandText = @"DELETE FROM File_names WHERE name = (@ime);";
                    cmd.Parameters.AddWithValue("@ime",full_item );
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }catch (Exception ex) {
                System.Windows.MessageBox.Show("Database error: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //System.Windows.MessageBox.Show(item, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        SQLiteConnection sqlite_conn = null;         // more biti vzunaj da druga metoda to najde
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
            String Izpisi_v_listbox = String.Empty;
            try {
                using (SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM File_names;", sqlite_conn)) {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader()) {
                        while (rdr.Read()) {
                            Izpisi_v_listbox = Refactor_string(rdr.GetString(0));
                            List_Files.Items.Add(Izpisi_v_listbox);
                            Console.WriteLine($"{rdr.GetString(0)}");
                        }
                    }
                }
            }catch(Exception ex){
                System.Windows.MessageBox.Show("Database error in init write to listbox: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private String Refactor_string(String name) {
            string[] splitan = name.Split('\\');
            return splitan[4];
        }

        int setting_stevilka;
        private void CheckBox_set() {
            try {
                using (SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM Settings WHERE ROWID = 1;", sqlite_conn)) {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader()) {
                        while (rdr.Read()) {
                            setting_stevilka = rdr.GetInt32(0);
                        }
                    }
                }
                if (setting_stevilka == 1) {
                    Check_Box_Button.IsChecked = true;

                } else {
                    Check_Box_Button.IsChecked = false;
                }
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show("Check box set error: " + ex, "CheckBox load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CheckBox_save() {

            //TODO dubi vrednost check boxa in naredi sa v db shrani 0 ali 1 



            try {
                using (SQLiteCommand cmd = new SQLiteCommand(@"UPDATE Settings SET New_folder = 1 WHERE ROWID = 1;", sqlite_conn)) {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader()) {
                        while (rdr.Read()) {
                            setting_stevilka = rdr.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show("Check box save error: " + ex, "CheckBox save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void clean_desek() {
            prog_bar.Visibility = Visibility.Visible;




        }

        #endregion

    }
}
