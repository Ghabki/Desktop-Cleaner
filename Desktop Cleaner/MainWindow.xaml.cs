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

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        #endregion

        readonly string _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        readonly string _publicPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);

        #region Button_clicks
        private void Clean_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckBox_save();
            Clean_desek();

        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            //zbrise označeni file name

            Remove_file();
        }

        private void Add_Button_Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new Costum_picker(povezava, delo);
            window.ShowDialog();
            List<string> daj_v_bazo = new List<string>(window.Vrni()); // mores drugace prekopirat list ker cene kazalci zbrijeso iz obeh
            window.resetList();
            foreach (string item in daj_v_bazo)
            {
                Console.WriteLine(item);
                Add_file(item);
            }
        }

        #endregion

        Podatki povezava;
        Delo delo;

        #region Init

        private void Init()
        {
            delo = new Delo(_desktopPath, _publicPath);
            povezava = new Podatki();

            Listbox_innit();
            CheckBox_set();
        }

        #endregion

        #region Functions
        public void Add_file(string file_name)
        {
            string prilepi;

            try
            {
                povezava.Dodaj(file_name);

                prilepi = delo.Refactor_string(file_name);
                List_Files.Items.Add(prilepi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error in adding file name to database " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Remove_file()
        {
            string item = string.Empty;
            try
            {
                item = List_Files.SelectedItem.ToString();

                List_Files.Items.RemoveAt(List_Files.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("You need to select a line to delete it.", "Reminder", MessageBoxButton.OK, MessageBoxImage.Information);
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

            try
            {
                povezava.Izbrisi(full_item);
            }
            catch (Exception ex)
            {
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
        private void CheckBox_set()
        {
            try
            {
                _settingStevilka = povezava.Select_Settings();
                if (_settingStevilka == 1)
                {
                    Check_Box_Button.IsChecked = true;

                }
                else
                {
                    Check_Box_Button.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check box set error: " + ex, "CheckBox load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CheckBox_save()
        {
            if (Check_Box_Button.IsChecked.Value)
            {
                try
                {
                    povezava.Update_settings(1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Check box save error: " + ex, "CheckBox save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                try
                {
                    povezava.Update_settings(0);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Check box save error: " + ex, "CheckBox save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Clean_desek()
        {
            try
            {

                if (Check_Box_Button.IsChecked == true)
                {
                    string ime_mape = delo.Check_map();
                    ime_mape = Path.Combine(_desktopPath, ime_mape); // Directory.CreateDirectory(<------------
                    Directory.CreateDirectory(ime_mape);
                    povezava.Dodaj_zadnjo_mapo(ime_mape);
                    Console.WriteLine("!!!!!" + ime_mape);

                    premik(ime_mape);
                    //todo metoda za premik podatkov in    izpiši na okence da je spravilo v to mapo da uporabnik ve
                }
                else
                {
                    string prejšnja_mapa = povezava.Vrni_zadnjo_mapo();

                    if (prejšnja_mapa == "" || !Directory.Exists(prejšnja_mapa))
                    {
                        string ime_mape = delo.Check_map();
                        ime_mape = Path.Combine(_desktopPath, ime_mape);
                        Directory.CreateDirectory(ime_mape);

                        povezava.Dodaj_zadnjo_mapo(ime_mape);
                        premik(ime_mape);
                    }
                    else
                    {

                        string map_path = Path.Combine(_desktopPath, prejšnja_mapa);
                        Directory.CreateDirectory(map_path);
                        premik(map_path);
                    }

                    //naredi da napise v mapo ki je bila nazadnje uporabljena oziroma nazadnje narejena ali neki
                    Console.WriteLine("naredi da spravi v v prejšnjo mapo mapo");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("napaka pri kreiranju mape na " + ex);
            }
        }

        private void premik(string kam)
        {
            List<string> namizje = new List<string>();
            List<string> ne_premikat = null;

            try
            {

                string che;
                foreach (var nam in delo.Datoteke_namizje())// to do tukaj naredui nekaj da prepreci null
                {
                    che = nam.Name;
                    Console.WriteLine(che);
                    if (delo.User_public_check(che))
                    {
                        namizje.Add(Path.Combine(_publicPath, che.Remove(che.Length - 13)));// ce je slucajo namizje prazno bo vrglo nazaj null(pac dodaj exception in je)
                    }
                    else
                    {
                        namizje.Add(_desktopPath + "\\" + nam.Name);// ce je slucajo namizje prazno bo vrglo nazaj null(pac dodaj exception in je)
                    }

                }
                ne_premikat = povezava.Vrni_vse();
                ne_premikat.Add(kam);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Neki ne dela v premiku" + ex, "Move error", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            foreach (string last in namizje)
            {
                FileAttributes attr = File.GetAttributes(last);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {

                    if (ne_premikat == null)
                    {
                        continue;// todo naredi da ce slucajno ne premakne nic da tudi napise da ni nic premakniilo ali neki

                    }
                    if (!ne_premikat.Contains(last))
                    {
                        try
                        {
                            Console.WriteLine(last + "\\" + " " + kam + "\\" + delo.Refactor_string(last));


                            Directory.Move(last, kam + "\\" + delo.Refactor_string(last));
                            //Directory.Move(last, kam);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    //MessageBox.Show("Its a directory");
                }
                else
                {
                    if (ne_premikat == null)
                    {
                        continue;// todo naredi da ce slucajno ne premakne nic da tudi napise da ni nic premakniilo ali neki

                    }
                    if (!ne_premikat.Contains(last))
                    {
                        try
                        {


                            File.Move(last, kam + "\\" + delo.Refactor_string(last));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }


                }

            }
        }
        #endregion
    }
}
