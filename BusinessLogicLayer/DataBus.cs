using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class DataBus
    {
        private static DataDao dataDao = new DataDao();

        public static void Restore(string filePath)
        {
            dataDao.Restore(filePath);
        }

        public static void Backup(string fileLocation)
        {
            dataDao.Backup(fileLocation);
        }
    }
}
