using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public class DataDao : DataProvider
    {

        public DataDao()
        {

        }


        public void Restore(string filePath)
        {
            string sqlQuery = string.Format(@"USE master; ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; 
                                                RESTORE DATABASE {1} FROM DISK = '{2}' WITH REPLACE;", "RestaurantDB", "RestaurantDB", filePath);
            ExecuteNonQuery(sqlQuery);

        }

        public void Backup(string fileLocation)
        {
            fileLocation += @"\RestaurantDB-" + DateTime.Now.ToString("dd-MM-yyyy.HH-mm-ss") + ".bak";
            string sqlQuery = string.Format(@"BACKUP DATABASE {0} TO DISK = '{1}'", "RestaurantDB", fileLocation);
            ExecuteNonQuery(sqlQuery);
        }
    }
}
