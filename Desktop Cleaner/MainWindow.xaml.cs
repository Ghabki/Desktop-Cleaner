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

        public void Add_Button_Button_Click(object sender, RoutedEventArgs e) {
            String File_name = String.Empty;
            try{
                OpenFileDialog dlg = new OpenFileDialog {
                    InitialDirectory = "Desktop"
                };

                if (dlg.ShowDialog() == true) {
                    File_name = dlg.FileName;
                    MessageBox.Show(File_name);
                    Add_file(File_name);
                    

                }
            }
            catch (Exception ex){
                MessageBox.Show("FileDialog Error: " + ex, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }














        }



        #endregion











        #region Init
        // more biti vzunaj da druga metoda to najde
        SQLiteConnection sqlite_conn = null;
        public void Init() { 
            sqlite_conn = new SQLiteConnection("Data Source=../../Data/Data.db; Version = 3");
            try {
                sqlite_conn.Open();
                System.Console.WriteLine("ok dela boi");
            }
            catch (Exception ex) {
                System.Console.WriteLine(ex);
                sqlite_conn.Close();
                MessageBox.Show("Database error: "+ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
            }
            //todo remove ta close

            
            }
            //TODO naredi da z db pobere imena filow in da v list box

        

        #endregion

        #region Functions
        public void Add_file(String file_name) {
            using (SQLiteCommand cmd = new SQLiteCommand(sqlite_conn)) {


                cmd.CommandText = @"INSERT INTO File_names (name) VALUES (@datoteka);";
                cmd.Parameters.AddWithValue("@datoteka", file_name);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

                //List_Files.Items.Add("asg");
                List_Files.Items.Add("fdsfsdf");

            }

        }
        private void Remove_file() {

        }



        #endregion

    

    }
}
