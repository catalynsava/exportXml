using System;
using System.Data;
using System.Data.OleDb;
namespace exportXml
{
    public static class BazaDeDate
    {
        public static string nume { get; set; }
        public static string connectionstring { get; set; }
        public static OleDbConnection conexiune { get; set; }
        public static bool conexiuneDeschisa { get; set; }
        public static bool conectare(string numebazadedate="2020.mdb"){
            connectionstring="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + 
                    AppDomain.CurrentDomain.BaseDirectory + "\\" + numebazadedate;
            conexiune=new OleDbConnection(connectionstring);
            conexiune.Open();
            conexiuneDeschisa=true;
            return true;
        
        }
        public static bool deconectare(){
            if(conexiuneDeschisa==true){
                conexiune.Close();
                conexiune.Dispose();
                conexiuneDeschisa=false ;
            }
            return true;
        }
    }
}
