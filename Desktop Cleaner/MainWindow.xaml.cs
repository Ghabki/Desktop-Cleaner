using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
// todo pretvori vse v angleščino ali v slovenpščino
namespace Desktop_Cleaner
{
    public partial class MainWindow
    {
        #region Window

        public MainWindow() {
            InitializeComponent();
            Init();
        }
        #endregion

        readonly string _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        readonly string _publicPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);

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
            window.ShowDialog();
            List<string> daj_v_bazo = window.Vrni();
            foreach (string item in daj_v_bazo)
            {
                Console.WriteLine(item);
                Add_file(item);
            }
        }

        #endregion

        Podatki povezava ;
        Delo delo ;

        #region Init
        
        private void Init() {
            delo = new Delo(_desktopPath, _publicPath);
            povezava = new Podatki();


            Listbox_innit();
            CheckBox_set();
        }

        #endregion

        #region Functions
        public void Add_file(string file_name) {
            string prilepi;

            try {
                povezava.Dodaj(file_name);

                prilepi = delo.Refactor_string(file_name);
                List_Files.Items.Add(prilepi); 
            }
            catch(Exception ex) {
                MessageBox.Show("Database error in adding file name to database " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Remove_file() {
            string item = string.Empty;
            try
            {
                item = List_Files.SelectedItem.ToString();

                List_Files.Items.RemoveAt(List_Files.SelectedIndex);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("You need to select a line to delete it.", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
                Console.WriteLine(ex);
            }

            string full_item;
            if (delo.User_public_check(item))
            {
                full_item = _publicPath + '\\' + item;
            }
            else
            {
                full_item = _desktopPath + '\\' + item; // 
            }
            Console.WriteLine(full_item); // tole se izbrise iz database

            try {
                povezava.Izbrisi(full_item);
            }catch (Exception ex) {
                MessageBox.Show("Database error: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //System.Windows.MessageBox.Show(item, "FileDialog Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Listbox_innit()
        {
            List<string> datoteke;
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
                MessageBox.Show("Database error in init write to listbox: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int _settingStevilka;
        private void CheckBox_set() {
            try {
                _settingStevilka = povezava.Select_Settings();
                if (_settingStevilka == 1) {
                    Check_Box_Button.IsChecked = true;

                } else {
                    Check_Box_Button.IsChecked = false;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Check box set error: " + ex, "CheckBox load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CheckBox_save() {
            if (Check_Box_Button.IsChecked.Value) {
                try {
                    povezava.Update_settings(1);
                }
                catch (Exception ex) {
                    MessageBox.Show("Check box save error: " + ex, "CheckBox save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } else {
                try {
                    povezava.Update_settings(0);

                }
                catch (Exception ex) {
                    MessageBox.Show("Check box save error: " + ex, "CheckBox save Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            string prejšnja_mapa;
            try
            {
                prejšnja_mapa = povezava.Vrni_zadnjo_mapo();



                if (Check_Box_Button.IsChecked == true)
                {
                    int i = 0;
                    const string nova_mapa = "Nova_mapa";
                    string current = Path.Combine(_desktopPath, nova_mapa);

                    while (Directory.Exists(current)) {
                        i++;
                        current = String.Format("{0} {1}", nova_mapa, i);
                    }
                    Directory.CreateDirectory(System.IO.Path.Combine(_desktopPath, current));
                    povezava.Dodaj_zadnjo_mapo(current);// todo izpiši na okence da je spravilo v to mapo da uporabnik ve
                    //todo metoda za premik podatkov
                }
                else
                {
                    if (prejšnja_mapa=="")
                    {
                        //todo isto kot zgoraj metoda za premik podatkov
                    }

                    //naredi da napise v mapo ki je bila nazadnje uporabljena oziroma nazadnje narejena ali neki
                    Console.WriteLine("naredi da spravi v v prejšnjo mapo mapo");
                }
            }catch(Exception){
                Console.WriteLine("napaka pri kreiranju mape na ");
            }





            int a = 0;

            String[] cars;
            DirectoryInfo d = new DirectoryInfo(_desktopPath);
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
