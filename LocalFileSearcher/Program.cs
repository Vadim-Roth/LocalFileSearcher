using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//DLL
using UiDll;

namespace LocalFileSearcher
{
    class Program
    {
        //This program references the UiDll and starts the program
        static void Main(string[] args)
        {
            UiClass ui = new UiClass();
            ui.Start();
        }
    }
}
