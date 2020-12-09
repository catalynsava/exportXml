using System;
using System.Data.OleDb;
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
        public static int executaScriptSql(string sqlFilePath){
            string line;
            int raspuns=0;
            

            StreamReader file = new System.IO.StreamReader(sqlFilePath);  
            while((line = file.ReadLine()) != null)  
            {
                if(line.Substring(0,2)!="--"){
                    try
                    {
                        using (OleDbCommand command=new OleDbCommand())
                        {
                            Console.WriteLine(@line);
                            command.CommandText=@line;
                            command.Connection=BazaDeDate.conexiune;
                            
                            raspuns=command.ExecuteNonQuery();
                            Console.WriteLine(raspuns.ToString() + " rows affected;");
                            Console.WriteLine("--");
                            command.Dispose();
                        }
                    }
                    catch (InvalidOperationException Ex)
                    {
                        Console.WriteLine(Ex.Message);
                    }
                    
                   
                }else{
                     Console.WriteLine(line);
                }
                 
            }  
            file.Close();
            return raspuns;
        }
    }
}