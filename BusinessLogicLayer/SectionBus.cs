using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransferObject;
using DataAccessLayer;
using System.Collections;
using System.Data;

namespace BusinessLogicLayer
{
    public class SectionBus
    {
        private static SectionDao sectionDao = new SectionDao();

        public static ArrayList GetSectionList()
        {
            return sectionDao.GetSectionList();
        }

        public static ArrayList GetSectionListByName(string p)
        {
            return sectionDao.GetSectionListByName(p);
        }

        public static void Insert(Section ds)
        {
            sectionDao.Insert(ds);
        }

        public static void Update(Section section)
        {
            sectionDao.Update(section);
        }

        public static void Delete(Section section)
        {
            sectionDao.Delete(section);
        }

        public static int GetNextID()
        {
            return sectionDao.GetNextID();
        }
    }
}
