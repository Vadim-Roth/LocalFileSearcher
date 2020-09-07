using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//DLL
using LogicDll;

namespace UiDll
{
    //This class is used by the console and uses the LogicDll
    //The functions here are for IO and they deliver or recieve info to/from the logicClass
    public class UiClass
    {
        LogicClass logicClass;
        public UiClass(){
            logicClass = new LogicClass();
        }
        private bool exit = false;
        private char menuChoice;
        //Here out program begins with a simple loop of options displayed by the Navigator function
        public void Start()
        {
            do {
                menuChoice = Navigator();
                if (menuChoice == 'e')
                    exit = true;
                else if (menuChoice == 'h') {
                    List<string> history = logicClass.GetHistory();
                    DisplayHistory(history);
                }
                else if (menuChoice == 'c')
                    NewSearch("C:\\");
                else if (menuChoice == 'd') {
                    string path = "";
                    do  {
                        if(path != "")
                            Console.WriteLine("Invalid directory! Folder does not exist!");
                        path = GetDirectory();
                    } while (!logicClass.CheckPathExistence(path));
                    NewSearch(path);
                }
            } while (!exit);
            GreetFarewell();
        }
        //This is the user's base menu for his functions
        private char Navigator()
        {
            char[] notBanned = new char[4] { 'c', 'd', 'h', 'e'};
            char choice = 'c';
            do {
                if (!notBanned.Contains(choice)) {
                    Console.Clear();
                    Console.WriteLine("Invalid Key!!!\n\n");
                }
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("| Hello and welcome to File Fetcher! |");
                Console.WriteLine("--------------------------------------\n");
                Console.WriteLine("Please press the key for one of the following options:\n");
                Console.WriteLine("Press (c) to search in all C:\\\nPress (d) to search in a specific directory");
                Console.WriteLine("Press (h) for history\n\nOr press (e) to exit");
                choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                choice = Char.ToLower(choice);
            } while (!notBanned.Contains(choice));
            return choice;
        }
        //This function greets the user farewell before closing
        private void GreetFarewell()
        {
            Console.Clear();
            Console.WriteLine("Farewell user and thanks for using my program");
            Console.WriteLine("Press anything to exit . . .");
            Console.ReadKey();
        }
        //This function displays the history list from the DB using the logicClass
        private void DisplayHistory(List<string> historyList)
        {
            Console.Clear();
            Console.WriteLine("*****************************************************************************");
            if (historyList.Count != 0)
                foreach (string historyItem in historyList) {
                    Console.WriteLine(historyItem + "\n");
                    if (historyItem.First() == 'C')
                        Console.WriteLine("*****************************************************************************");
                }
            else {
                Console.WriteLine("No history yet. Go search for something");
                Console.WriteLine("*****************************************************************************");
            }
            PressKey();
        }
        //This function is used sometimes in the end of a a process
        private void PressKey()
        {
            Console.WriteLine("Press any key to return to the menu . . .");
            Console.ReadKey();
            Console.Clear();
        }
        //This function asks the user for a directory and then sends it back
        private string GetDirectory()
        {
            Console.WriteLine("Please enter a full path for search or press ENTER for search in C:\\");
            string path = Console.ReadLine();
            if (path == "")
                path = "C:\\";
            return path;
        }
        //This function asks the user for a searchStr and then sends it back
        private string GetSearchStr()
        {
            Console.WriteLine("Enter a file's name to search or press ENTER to return to the menu: ");
            return Console.ReadLine();
        }
        //This is the display screen for the fileSearch using the event in the logicClass
        private void NewSearch(string path)
        {
            string searchStr = GetSearchStr();
            if (searchStr != "") {
                Console.Clear();
                Console.WriteLine($"Searching for {searchStr} in {path}. . .");
                logicClass.fileFoundEvent += new FileFoundHandler(DisplayNewFile);
                Console.WriteLine("*****************************************************************************");
                int resultsFound = logicClass.BeginSearch(path, searchStr);
                if (resultsFound == 0)
                    Console.WriteLine($"No files by the name {searchStr} were found in {path}");
                Console.WriteLine("*****************************************************************************");
                if(resultsFound != 0)
                    Console.WriteLine($"\n{resultsFound} results found in total!");
                PressKey();
            }
            else
                Console.Clear();
        }
        //This is the event function to display each file found by the program on the Console
        private void DisplayNewFile(string file)
        {
            Console.WriteLine(file);
        }
    }
}
