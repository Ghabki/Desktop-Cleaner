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
using System.IO;

namespace Desktop_Cleaner {
    public partial class MainWindow : Window {
        #region Window

        public MainWindow() {
            InitializeComponent();
            Init();
        }
        #endregion

        readonly string desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        #region Button_clicks
        private void Clean_Button_Click(object sender, RoutedEventArgs e) {
            CheckBox_save();
            Clean_desek();

        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e) {
            //zbrise označeni file name

            Remove_file();
        }

        private void Add_Button_Button_Click(object sender, RoutedEventArgs e) {
            var window = new Costum_picker(povezava, delo); 
            window.Show();



            /*
            String File_name = String.Empty;

            try{
                
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    File_name = dlg.FileName;
                    System.Windows.MessageBox.Show(File_name);
                    Add_file(File_name);
                }
            }
            catch (Exception ex){
                System.Windows.MessageBox.Show("FileDialog Error: " + ex, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }




        #endregion

        Podatki povezava = null;
        Delo delo = null;
        #region Init
        
        private void Init() {
            povezava = new Podatki();
            delo = new Delo();

            Listbox_innit();
            CheckBox_set();
        }

        #endregion

        #region Functions
        private void Add_file(String file_name) {
            string prilepi = string.Empty;

            try {
                povezava.Dodaj(file_name);

                prilepi = delo.Refactor_string(file_name);
                List_Files.Items.Add(prilepi); 
            }
            catch(Exception ex) {
                System.Windows.MessageBox.Show("Database error in adding file name to database " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Remove_file() {
            String item = string.Empty;
            String full_item = string.Empty;

            try {
                item = List_Files.SelectedItem.ToString();

                List_Files.Items.RemoveAt(List_Files.SelectedIndex);
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show("You need to select a line to delete it.", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
                Console.WriteLine(ex);
            }

            full_item = desktop_path +'\\'+ item;
            Console.WriteLine(full_item); // tole se izbrise iz database

            try {
                povezava.Izbrisi(full_item);
            }catch (Exception ex) {
                System.Windows.MessageBox.Show("Database error: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //System.Windows.MessageBox.Show(item, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Listbox_innit()
        {
            List<string> datoteke = new List<string>();
            try
            {
                datoteke = povezava.Vrni_vse();

                foreach (string a in datoteke)
                {
                    string Izpisi_v_listbox = delo.Refactor_string(a);
                    List_Files.Items.Add(Izpisi_v_listbox);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Database error in init write to listbox: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int setting_stevilka;
        private void CheckBox_set() {
            try {
                setting_stevilka = povezava.Select_Settings();
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
            //TODO uredi database execution ker nerabim tega read pa to 
            if (Check_Box_Button.IsChecked.Value) {
                try {
                    povezava.Update_settings(1);
                }
                catch (Exception ex) {
                    System.Windows.MessageBox.Show("Check box save error: " + ex, "CheckBox save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } else {
                try {
                    povezava.Update_settings(0);

                }
                catch (Exception ex) {
                    System.Windows.MessageBox.Show("Check box save error: " + ex, "CheckBox save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Clean_desek()
        {
            prog_bar.Visibility = Visibility.Visible;
            //CheckBox_save();
            // mogoce 3 arraye buu
            /*
             prvo naredi da naradi novi file ali ne 
             probaj naredit z threadom
             mogoce dodaj okno za kako da je ime filu ? ali pa ne zato da je hitreje in izi


            lahko naredis da je spremenljivka za desktop path global ker se jo pac dostikrat uporabi
             */
            try
            {
                if (Check_Box_Button.IsChecked == true)
                {
                    int i = 0;
                    string nova_mapa = "Nova mapa";
                    string current = System.IO.Path.Combine(desktop_path, nova_mapa);

                    while (Directory.Exists(current)) {
                        i++;
                        current = String.Format("{0} {1}", nova_mapa, i);
                    }
                    Directory.CreateDirectory(System.IO.Path.Combine(desktop_path, current));
                }
                else
                {
                    //naredi da napise v mapo ki je bila nazadnje uporabljena oziroma nazadnje narejena ali neki
                    Console.WriteLine("naredi da spravi v v prejšnjo mapo mapo");
                }
            }catch(Exception){
                Console.WriteLine("napaka pri kreiranju mape na ");
            }





            int a = 0;

            String[] cars;
            DirectoryInfo d = new DirectoryInfo(desktop_path);
            DirectoryInfo b = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));

            foreach (var file in b.GetFiles())
            {
                Console.WriteLine(file);
                a++;

                //Podatki.zacetek();
            }
            /*
            foreach (var file in d.GetFiles()){
                Console.WriteLine(file);
                a++;
                

            }
            foreach (var file in d.GetDirectories())
            {
                Console.WriteLine(file);
                a++;
                


            }*/
            Console.WriteLine(a);
        }
        #endregion

    }
}
