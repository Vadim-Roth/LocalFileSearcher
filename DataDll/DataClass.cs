using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
//DLL
namespace DataDll
{
    //This class is used by the LogicDll for data functions
    public class DataClass
    {
        SearchHistoryDataEntities shDB = new SearchHistoryDataEntities();
        //This funtion return the search history from the DB
        public List<string> GetHistory(string currentDirectory)
        {
            string allFilesFound;
            List<string> searchHistory = new List<string>();
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="
                 + currentDirectory + @"\SearchHistoryDB.mdf;Integrated Security=True");
            SqlDataAdapter adp = new SqlDataAdapter("select * from ResultsFound", cn);
            try {
                adp.Fill(dt);
                foreach (SearchHistory sr in shDB.SearchHistories) {
                    allFilesFound = "";
                    searchHistory.Add($"{sr.searchStr}\n{sr.date}\nIn folder {sr.folderName}\n" +
                        $"{sr.resultsFound} results found.");
                    if (sr.resultsFound > 0) {
                        foreach (DataRow i in dt.Rows)
                            if (int.Parse(i[1].ToString()) == sr.searchId)
                                allFilesFound += i[2] + "\n";
                    }
                    searchHistory.Add(allFilesFound);
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); Console.ReadKey(); };
            return searchHistory;
        }
        //This function adds the info for the search into the tables
        public void NewSearch(List<string> searchResults, string searchStr, string folderName)
        {
            SearchHistory sr = new SearchHistory() { date = DateTime.Now.ToString(),
                searchStr = searchStr, folderName = folderName, resultsFound = searchResults.Count };
            shDB.SearchHistories.Add(sr);
            //The exceptions here were fixed and remained for safety only
            try {
                shDB.SaveChanges();
                foreach(string file in searchResults)
                    shDB.ResultsFounds.Add(new ResultsFound(){searchId = sr.searchId, fileName = file});
                shDB.SaveChanges();
            }
            catch (DbEntityValidationException e) {
                Console.WriteLine(e.Message);
            }
            catch (Exception e) { Console.WriteLine(e.InnerException.InnerException.Message); }
        }
    }
}
