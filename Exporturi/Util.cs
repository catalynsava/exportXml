using System;
using System.Data.OleDb;
using System.IO;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public static class Util
    {
        public static void navigateFisiere(){
            string[] fisiere={"2.7.50.1.", "2.7.39.1.", "2.7.22.1.", "2.5.32.1.", "2.5.1.1.", "2.13.44.1.", "2.13.36.1.", "2.13.19.1.", "2.13.13.1.", "1.9.50.1.", "1.9.5.1.", "1.9.12.1.", "1.8.32.3.", "1.8.25.2.", "1.8.21.1.", "1.8.12.1.", "1.7.31.1.", "1.7.28.1.", "1.7.17.1.", "1.6.3.1.", "1.6.22.3.", "1.6.16.2.", "1.6.11.1.", "1.5.42.1.", "1.5.33.1.", "1.5.31.1.", "1.5.20.3.", "1.5.13.1.", "1.5.12.1.", "1.4.36.1.", "1.4.34.1.", "1.4.2.1.", "1.4.12.1.", "1.3.47.3.", "1.3.4.2.", "1.3.21.1.", "1.25.38.1.", "1.24.16.1.", "1.22.45.1.", "1.20.13.1.", "1.2.44.3.", "1.2.35.2.", "1.17.26.1.", "1.17.16.1.", "1.16.34.1.", "1.15.50.1.", "1.14.32.1.", "1.12.50.1.", "1.12.20.1.", "1.12.13.1.", "1.11.37.1.", "1.10.8.2.", "1.10.46.1.", "1.10.4.2.", "1.1.48.1.", "1.1.37.1."};
             foreach (var item in fisiere)
            {
                Console.WriteLine(item);
            }
        }
        public static void comun(string folderCapitol){
            bool folderExists = Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + folderCapitol);
            if(folderExists==false){
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + folderCapitol);
                Console.WriteLine("am creat: " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + folderCapitol);
            }
            folderExists = Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong");
            if(folderExists==false ){
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong");
                Console.WriteLine("am creat: " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong");
            }

            AjutExport.stergeInFolder(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" +folderCapitol+ "\\");
            Console.WriteLine("am sters " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" +folderCapitol+ "\\");

            AjutExport.stergeInFolder(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong\\");
            Console.WriteLine("am sters " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong\\");
        }
        public static void cap1(){
            

            string[] fisiere={"2.7.50.1.", "2.7.39.1.", "2.7.22.1.", "2.5.32.1.", "2.5.1.1.", "2.13.44.1.", "2.13.36.1.", "2.13.19.1.", "2.13.13.1.", "1.9.50.1.", "1.9.5.1.", "1.9.12.1.", "1.8.32.3.", "1.8.25.2.", "1.8.21.1.", "1.8.12.1.", "1.7.31.1.", "1.7.28.1.", "1.7.17.1.", "1.6.3.1.", "1.6.22.3.", "1.6.16.2.", "1.6.11.1.", "1.5.42.1.", "1.5.33.1.", "1.5.31.1.", "1.5.20.3.", "1.5.13.1.", "1.5.12.1.", "1.4.36.1.", "1.4.34.1.", "1.4.2.1.", "1.4.12.1.", "1.3.47.3.", "1.3.4.2.", "1.3.21.1.", "1.25.38.1.", "1.24.16.1.", "1.22.45.1.", "1.20.13.1.", "1.2.44.3.", "1.2.35.2.", "1.17.26.1.", "1.17.16.1.", "1.16.34.1.", "1.15.50.1.", "1.14.32.1.", "1.12.50.1.", "1.12.20.1.", "1.12.13.1.", "1.11.37.1.", "1.10.8.2.", "1.10.46.1.", "1.10.4.2.", "1.1.48.1.", "1.1.37.1."};
             foreach (var item in fisiere)
            {
                Console.WriteLine(item);
            

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + item + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + item + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){
                    CAP1.make_CAP1xml(item + "20.");
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1\\" + item + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1\\" + item + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1_Wrong\\" + item + "xml"
                        );
                    }
                }
            }
        }
    }
}