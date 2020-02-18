using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace Desktop_Cleaner
{
    public class Podatki
    {
        readonly SQLiteConnection _sqliteConn;
        public Podatki()
        {
            try
            {
                _sqliteConn = new SQLiteConnection("Data Source=../../Data/Data.db; Version = 3");
                _sqliteConn.Open();
                Console.WriteLine("ok dela boi");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _sqliteConn.Close();//vrjetno nerabi tega
                MessageBox.Show("Database error: " + ex, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        public int Select_Settings()
        {

            using (SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM Settings WHERE ROWID = 1;", _sqliteConn))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int setting_stevilka = rdr.GetInt32(0);
                        return setting_stevilka;
                    }
                    return (0);
                }
            }
        }

        public List<string> Vrni_vse()
        {
            List<string> datoteke = new List<string>();

            using (SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM File_names;", _sqliteConn))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        datoteke.Add(rdr.GetString(0));                          
                    }
                }
            }
            return datoteke;
        }

        public void Izbrisi(string cela_pot)
        {

            using (SQLiteCommand cmd = new SQLiteCommand(_sqliteConn))
            {
                cmd.CommandText = @"DELETE FROM File_names WHERE name = (@ime);";
                cmd.Parameters.AddWithValue("@ime", cela_pot);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public void Dodaj(string ime)
        {

            using (SQLiteCommand cmd = new SQLiteCommand(_sqliteConn))
            {
                cmd.CommandText = @"INSERT INTO File_names (name) VALUES (@datoteka);";
                cmd.Parameters.AddWithValue("@datoteka", ime);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update_settings(int a)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(@"UPDATE Settings SET New_folder = (@true_false) WHERE ROWID = 1;", _sqliteConn)) { 
                cmd.Parameters.AddWithValue("@true_false", a);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        public string Vrni_zadnjo_mapo()
        {
            using (SQLiteCommand cmd = new SQLiteCommand(@"SELECT * FROM Folder WHERE ROWID = 1;", _sqliteConn))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        string mapa = rdr.GetString(0);
                        return mapa;
                    }
                    return ("");
                }
            }
        }

        public void Dodaj_zadnjo_mapo(string ime)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(_sqliteConn))
            {
                cmd.CommandText = @"UPDATE Folder SET last_folder = (@ime) WHERE ROWID = 1;";
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

        }

        //todo lahko naredis da se datoteke pravilno formatirane vrnejo uz tle

    }
}
