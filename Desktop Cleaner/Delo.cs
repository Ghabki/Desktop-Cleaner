using System;
using System.Collections.Generic;
using System.IO;

namespace Desktop_Cleaner
{
    public class Delo
    {
        readonly string _path1;
        readonly string _path2;

        public Delo(string p1, string p2)
        {
            _path1 = p1;
            _path2 = p2;

        }


        public string Refactor_string(string name)
        {
            string[] splitan = name.Split('\\');
            return splitan[4];
        }


        public List<ListView_Data> Datoteke_namizje()
        {
            List<ListView_Data> stvari = new List<ListView_Data>();


            DirectoryInfo namizje_a = new DirectoryInfo(_path1);
            DirectoryInfo namizje_b = new DirectoryInfo(_path2);


            //Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            //Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));

            try
            {
                foreach (var file in namizje_a.GetFiles())
                {
                    string file_a = file.ToString();
                    if (file_a== "desktop.ini")
                    {
                    }
                    else
                    {
                        stvari.Add(new ListView_Data() { Name = file_a });
                    }

                }

                foreach (var file in namizje_a.GetDirectories())
                {
                    string file_a = file.ToString();
                    if (file_a == "desktop.ini")
                    {

                    }
                    else
                    {
                        stvari.Add(new ListView_Data() {Name = file_a});
                    }
                }

                foreach (var file in namizje_b.GetFiles())
                {

                    string file_b = file.ToString();
                    if (file_b == "desktop.ini")
                    {
                    }
                    else
                    {
                        stvari.Add(new ListView_Data() {Name = file_b + " (((PUBLIC)))"});
                    }
                }

                foreach (var file in namizje_b.GetDirectories())
                {
                    string file_b = file.ToString();
                    if (file_b == "desktop.ini")
                    {
                    }
                    else
                    {
                        stvari.Add(new ListView_Data() { Name = file_b+" (((PUBLIC)))" });
                    }
                }
                return stvari;
            }
            catch (Exception)
            {

                throw;
            }
            
        }


        public bool User_public_check (string vnos)    //todo ce bo kaksna napaka z tem da gre cez string al neki ker je pac 12 mest je tole problem in se more spremeniti
        {
            string rezul = vnos.Substring(vnos.Length - 12);

            return rezul == "(((PUBLIC)))";   // basicly ce je public vrne true ce pa ni vrne false
        }

    }
    //ta class narejen je tukaj samo da je olajsano ce zelim se kej dodati v listview
    public class ListView_Data
    {
        public string Name { get; set; }
    }
}
