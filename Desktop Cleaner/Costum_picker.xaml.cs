using System;
using System.Collections.Generic;
using System.Windows;

namespace Desktop_Cleaner
{
    public partial class Costum_picker : Window
    {
        readonly Podatki povezava;
        readonly Delo delo;
        public Costum_picker(Podatki abc, Delo abcd)
        {

            InitializeComponent();
            povezava = abc;
            delo = abcd;
            ListView_Init();

        }

        List<string> _vrni = new List<string>();
        List<string> podatki_za_primerjat = new List<string>();
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        readonly string path2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);


        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (lis.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Nicesar nisi izbral", "information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //_vrni.Clear();
                var selectedItems = lis.SelectedItems;
                string izpis = "";
                foreach (ListView_Data selectedItem in selectedItems)
                {

                    string name_of_file = selectedItem.Name;
                    if (podatki_za_primerjat.Contains(name_of_file))
                    {
                        izpis += ("NI DODALO: " + name_of_file + "!\n");
                        Console.WriteLine("ni dodano");
                        if (lis.SelectedItems.Count == 1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        izpis += ("OK: " + name_of_file + "!\n");
                        podatki_za_primerjat.Add(name_of_file);
                        if (delo.User_public_check(name_of_file))
                        {
                            name_of_file = path2 + "\\" + selectedItem.Name;
                        }
                        else
                        {
                            name_of_file = path + "\\" + selectedItem.Name;
                        }

                        _vrni.Add(name_of_file);
                    }
                }
                MessageBox.Show(izpis, "Informacije", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Item Add Error: " + ex, "Item Add Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public List<string> Vrni()
        {
            return _vrni;

        }

        public void resetList()
        {
            _vrni.Clear();
        }


        private void ListView_Init()
        {
            try
            {
                foreach (string stvar in povezava.Vrni_vse())
                {
                    podatki_za_primerjat.Add(delo.Refactor_string(stvar));

                }

                List<ListView_Data> abc = delo.Datoteke_namizje();
                lis.ItemsSource = abc;

            }
            catch (Exception ex)
            {

                MessageBox.Show("ListtView_innit Error: " + ex, "ListtView_innit Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }
    }
}

