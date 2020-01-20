using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Cleaner
{
    class Delo
    {
        public String Refactor_string(String name)
        {
            string[] splitan = name.Split('\\');
            return splitan[4];
        }


    }
}
