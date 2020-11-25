using System;
using System.IO;

namespace exportXml.Validari
{
    public static class Ajutatoare
    {
        public static bool IsDateTime(string txtDate)
        {
            DateTime tempDate;
            return DateTime.TryParse(txtDate, out tempDate);
        }
        public static bool IsNumeric(string value)
        {
            double retNum;
            return Double.TryParse(value, out retNum);
        }
        public static void scrielinie(string iffier, string linie){
            bool folderExist = Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + "BUGS");
            if(folderExist==false){
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + "BUGS");
                Console.WriteLine("am creat: " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + "BUGS");
            }

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\XML\\BUGS\\" + iffier, true))
            {
                file.WriteLine(linie);
            }
        }
    }
}