using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//DLL
using DataDll;

namespace LogicDll
{
    //This class is used by the UiDll and uses the DataDll
    //The functions here are either a bridge to the DB or the search itself
    public delegate void FileFoundHandler(string fileName);
    public class LogicClass
    {
        //Here is the decleration of the event for the UiClass to use
        public event FileFoundHandler fileFoundEvent;
        DataClass dataClass;
        public LogicClass() {
            dataClass = new DataClass();
        }
        //This is the main function that searches for the file in the directory
        //It uses a recursive method to search and then ducuments it in the DB
        public int BeginSearch(string path, string searchStr)
        {
            if (searchStr == "") 
                return 0;
            //This commented line is unnecessary and was used for testing on my pc
            //path = @"C:\Users\koman\Desktop\Backups\nintendo\gba backup 28'8'20";
            List<string> searchResults = GetFiles(path, searchStr);
            dataClass.NewSearch(searchResults, searchStr, path);
            return searchResults.Count;
        }
        //This is a recursive function that first searches for files in folder, then checks
        //whether there are more folders inside, and if so - calls itslef until no inside folders
        //are found. Every file found is added up to fileList
        private List<string> GetFiles(string path, string searchStr)
        {
            string[] insideFolders = Directory.GetDirectories(path);//, "*.txt", SearchOption.AllDirectories);
            string[] insideFiles = Directory.GetFiles(path);
            List<string> fileList = new List<string>();
            foreach (string file in insideFiles) {
                string lowerStr = searchStr.ToLower();
                string fileName;
                fileName = file.Substring(file.LastIndexOf('\\') + 1).ToLower();
                if (fileName.Contains(lowerStr)) {
                    fileFoundEvent?.Invoke(file);
                    fileList.Add(file);
                }
            }
            if (insideFolders.Length > 0) {
                foreach (string folder in insideFolders) {
                    try {
                        List<string> insideList = GetFiles(folder, searchStr);
                        foreach (string file in insideList)
                            fileList.Add(file);
                    }
                    catch (UnauthorizedAccessException) { };
                }
            }
            return fileList;
        }
        //This function is a bridge between the DataClass and the UiClass
        public List<string> GetHistory()
        {
            return dataClass.GetHistory(Directory.GetCurrentDirectory());
        }
        //The IO class is a logic function and only used in this class to return info to the UiClass
        public bool CheckPathExistence(string path)
        {
            if (Directory.Exists(path))
                return true;
            return false;
        }
    }
}
