using System;
using System.Data.OleDb;

namespace exportXml.Exporturi
{
    public class Sirute
    {
        public string Siruta { get; set; }
        public string SirutaSuperioara { get; set; }
        public string SirutaJudet { get; set; }

        
        public Sirute (string strSat, string strJudet)
        {
            string strSQL = @"SELECT nivel0.*, judete.siruta as judsiruta FROM (nivel1 INNER JOIN nivel0 ON nivel1.sirsup = nivel0.sirsup) INNER JOIN judete ON nivel0.jud = judete.nr WHERE nivel0.denumire=""" + strSat + @""" AND nivel0.jud In (SELECT nr FROM judete WHERE denumire=""" + strJudet + @""")";
            //Console.WriteLine(strSQL);

            OleDbConnection cnnTEMP = new OleDbConnection();
            cnnTEMP.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data source= " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "init.mdb";
            cnnTEMP.Open();
            try
            {
                OleDbCommand cmdTEMP = new OleDbCommand(strSQL, cnnTEMP);
                OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
                if(drTEMP.Read()){
                    this.SirutaJudet = drTEMP["judsiruta"].ToString();
                    this.SirutaSuperioara = drTEMP["sirsup"].ToString();
                    this.Siruta = drTEMP["siruta"].ToString();
                }else{
                    this.SirutaJudet = "";
                    this.SirutaSuperioara = "";
                    this.Siruta = "";
                }
                
                

                cnnTEMP.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to data source " + ex.ToString());
                cnnTEMP.Close();
            }
        }
    }
}