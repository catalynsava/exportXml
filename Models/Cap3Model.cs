using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;

namespace exportXml.Models
{
    public class Rand
    {
        public string NrCrt { get; set; }
        public string suma { get; set; }
        public decimal inloc { get; set; }
        public decimal altloc { get; set; }
    }
    public class Cap3Model{
        public List<Rand> Capitol3=new List<Rand>();
        public Cap3Model(string idRol)
        {
            try
                    {
                        using (OleDbCommand command=new OleDbCommand())
                        {
                            command.CommandText=@"SELECT nomcap3.NrCrt, nomcap3.suma, CAP3.inloc, CAP3.altloc, CAP3.tot FROM nomcap3 LEFT JOIN (SELECT * FROM CAP3 WHERE CAP3.IDROL= "" + idRol + @"") AS TABLE2 ON nomcap3.NrCrt = TABLE2.NrCrt ORDER BY nomcap3.NrCrt;";
                            command.Connection=BazaDeDate.conexiune;
                            
                            OleDbDataReader dr =command.ExecuteReader();
                            while(dr.Read()){
                                Capitol3.Add(
                                    new Rand{
                                        NrCrt=dr!["nrcrt"].ToString(),
                                        suma=dr["suma"].ToString(),
                                        inloc=Convert.ToDecimal( dr["inloc"]),
                                        altloc=Convert.ToDecimal(dr["altloc"])
                                    }
                                );
                            }
                            dr.Close();
                            command.Dispose();
                        }
                    }
                    catch (InvalidOperationException Ex)
                    {
                        Console.WriteLine(Ex.Message);
                    }
        }
    }
}