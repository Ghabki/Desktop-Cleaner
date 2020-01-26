using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Cleaner
{
    public class Delo
    {
        public String Refactor_string(String name)
        {
            string[] splitan = name.Split('\\');
            return splitan[4];
        }


        public List<ListView_Data> Datoteke_namizje()
        {
            List<ListView_Data> stvari = new List<ListView_Data>();


            DirectoryInfo namizje_a = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            DirectoryInfo namizje_b = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));

            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));

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
                        stvari.Add(new ListView_Data() { Name = file_a });

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
                        stvari.Add(new ListView_Data() { Name = file_b });

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
                        stvari.Add(new ListView_Data() { Name = file_b });

                    }


                }
                return stvari;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
    //ta class narejen je tukaj samo da je olajsano ce zelim se kej dodati v listview
    public class ListView_Data
    {
        public string Name { get; set; }
    }
}
