using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Desktop_Cleaner
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Costum_picker : Window
    {
        Podatki povezava;
        Delo delo;
        public Costum_picker(Podatki abc, Delo abcd)
        {

            InitializeComponent();
            povezava = abc;
            delo = abcd;
            ListView_Init();

        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (lis.SelectedItems.Count == 0)
            {
                return;
            }


            var selectedItems = lis.SelectedItems;
            foreach (ListView_Data selectedItem in selectedItems)
            {
                /*
                 Za vsak file preveri ce ni ze v database (naredi get pa set zato da drig class lahko to vzame, mogoce pa to ni niti potrebno ki lahko ze tle to naridm)
                 nekatere datoteke so od windowsa pomembne in se jih nespme premikat ipo desktop.ini, za to dodaj exception
                 v database dodaj last used folder
                 
                 
                 */



                string name_of_file = selectedItem.Name.ToString();
                System.Windows.MessageBox.Show(name_of_file);

            }










        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void ListView_Init()
        {
            lis.ItemsSource = delo.Datoteke_namizje();
        }
    }
}



//class narejen samo ce zelim se kaksne podatke dati v listview je lazje

