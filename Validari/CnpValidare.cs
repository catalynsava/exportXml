
using System;
using System.Data.OleDb;

namespace exportXml.Validari
{
    public static class CnpValidare
    {
        public static void adrrolcnpuri(){
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnp FROM adrRol WHERE tip=1 or tip=2;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP( dr["cnp"].ToString());
                
                 if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("ADRROLcnpurieronate.log",rsp.detalii);
                }
            }
        }
        public static void cap1cnpuri(){
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnp FROM cap1;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP( dr["cnp"].ToString());

                if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("CAP1cnpurieronate.log",rsp.detalii);
                }
            }
        }
        public static void cap2bcnpuri(){
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnptit FROM cap2b;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP( dr["cnptit"].ToString());
                
                if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("CAP2Bcnpurieronate.log",rsp.detalii);
                }
            }
        }
        public static void cap13cnpuri(){
            
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnpDef FROM cap13;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP(dr["cnpdef"].ToString());
                
                if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("CAP13cnpurideferonate.log",rsp.detalii);
                }
            }
        }
        public static void succesoricnpuri(){
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnpsucces FROM succesori;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP( dr["cnpsucces"].ToString());

                if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("SUCCESORIcnpurieronate.log",rsp.detalii);
                }
            }
        }
        public static void cumparatoricnpuri(){
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnp_Cump FROM cap14;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP( dr["cnp_Cump"].ToString());

                if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("CUMPARATORIcnpurieronate.log",rsp.detalii);
                }
            }
        }
        public static void cap15acnpuri(){
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnp FROM cap15;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP( dr["cnp"].ToString());

                if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("CAP15Acnpurieronate.log",rsp.detalii);
                }
            }
        }
        public static void cap15bcnpuri(){
            RaspunsValidare rsp;
            OleDbCommand command=new OleDbCommand("SELECT cnp FROM cap15b;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                rsp=Validari.CnpValidare.verificaCNP( dr["cnp"].ToString());

                if(rsp.corect==false){
                    Console.WriteLine(rsp.detalii);
                    Ajutatoare.scrielinie("CAP15Bcnpurieronate.log",rsp.detalii);
                }
            }
        }
        public static void uncnpcateroluri(){
            OleDbCommand command=new OleDbCommand("SELECT adrrol.cnp, Count(adrrol.cnp) AS nori FROM adrrol GROUP BY adrrol.cnp HAVING Count(adrrol.cnp)>1;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                Ajutatoare.scrielinie("uncnpcateroluri.log", "\"" + dr["cnp"].ToString() + "\" nr.roluri: " + dr[1].ToString());
                Console.WriteLine("\"" + dr["cnp"].ToString() + "\" nr.roluri: " + dr[1].ToString());
            }
        }
        public static void cnpuricatimembri(){
            OleDbCommand command=new OleDbCommand("SELECT CNP, COUNT(CNP) as nori FROM CAP1 GROUP BY CNP HAVING COUNT(CNP)>1;",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            while(dr.Read()){
                Ajutatoare.scrielinie("uncnpcatimembri.log", "\"" + dr["cnp"].ToString() + "\" nr.roluri: " + dr[1].ToString());
                Console.WriteLine("\"" + dr["cnp"].ToString() + "\" nr.membri: " + dr[1].ToString());
            }
        }
        public static int uncnpcatimembri(string CnpString){
            OleDbCommand command=new OleDbCommand("SELECT COUNT(CNP) as CatiMemb FROM CAP1 WHERE CNP=\"" + CnpString + "\";",BazaDeDate.conexiune);
            OleDbDataReader dr=command.ExecuteReader();
            if(dr.Read()){
                return Convert.ToInt16(dr["CatiMemb"]);
            }else{
                return 0;
            }
        }
        public static RaspunsValidare verificaCNP(string strCNP)
        {
            RaspunsValidare raspuns=new RaspunsValidare();
            //mai mult de 13 caractere
            if (strCNP.Length > 13)
            {
                raspuns.corect=false;
                raspuns.detalii="CNP:\"" +strCNP + "\" eronat. Mai mult de 13 caractere.";
                return raspuns;
            }
            //mai putin de 13 caractere
            if (strCNP.Length < 13)
            {
                raspuns.corect=false;
                raspuns.detalii="CNP:\"" +strCNP + "\" eronat. Mai puțin de 13 caractere.";
                return raspuns;
            }
            //daca sunt numerice
            for (int x = 0; x <= 12; x++)
            {

                if (Ajutatoare.IsNumeric(strCNP.Substring(x, 1)) == false)
                {
                    raspuns.corect=false;
                    raspuns.detalii="CNP:\"" +strCNP + "\" eronat. Prezinta caractere non-numerice.";
                    return raspuns;
                }

            }
            //începe cu caracterele: 1,2,3,4,5,6,7,8
            if (strCNP.Substring(0, 1) != "1" &&
               strCNP.Substring(0, 1) != "2" &&
               strCNP.Substring(0, 1) != "3" &&
               strCNP.Substring(0, 1) != "4" &&
               strCNP.Substring(0, 1) != "5" &&
               strCNP.Substring(0, 1) != "6" &&
               strCNP.Substring(0, 1) != "7" &&
               strCNP.Substring(0, 1) != "8")
            {
                raspuns.corect=false;
                raspuns.detalii="CNP:\"" +strCNP + "\" eronat. Nu începe cu caracterele: 1,2,3,4,5,6,7,8.";
                return raspuns;
            }

            //daca data nașterii este validă
            string dataCNP;
            string strAnul;
            if (strCNP.Substring(0, 1) == "1" || strCNP.Substring(0, 1) == "2" || strCNP.Substring(0, 1) == "7" || strCNP.Substring(0, 1) == "8")
            {
                strAnul = "19" + strCNP.Substring(1, 2);
            }
            else if (strCNP.Substring(0, 1) == "5" || strCNP.Substring(0, 1) == "6")
            {
                strAnul = "20" + strCNP.Substring(1, 2);
            }
            else
            {
                strAnul = "18" + strCNP.Substring(1, 2);
            }
            dataCNP = strCNP.Substring(5, 2) + "." +
                strCNP.Substring(3, 2) + "." +
                strAnul;
            if (Ajutatoare.IsDateTime(dataCNP) == false)
            {
                raspuns.corect=false;
                raspuns.detalii="CNP:" + strCNP + " eronat. Data nașterii invalidă.";
                return raspuns;
            }
            //dacă cheia cnp-ului este validă
            int c1 = Convert.ToInt16(strCNP.Substring(0, 1));
            int c2 = Convert.ToInt16(strCNP.Substring(1, 1));
            int c3 = Convert.ToInt16(strCNP.Substring(2, 1));
            int c4 = Convert.ToInt16(strCNP.Substring(3, 1));
            int c5 = Convert.ToInt16(strCNP.Substring(4, 1));
            int c6 = Convert.ToInt16(strCNP.Substring(5, 1));
            int c7 = Convert.ToInt16(strCNP.Substring(6, 1));
            int c8 = Convert.ToInt16(strCNP.Substring(7, 1));
            int c9 = Convert.ToInt16(strCNP.Substring(8, 1));
            int c10 = Convert.ToInt16(strCNP.Substring(9, 1));
            int c11 = Convert.ToInt16(strCNP.Substring(10, 1));
            int c12 = Convert.ToInt16(strCNP.Substring(11, 1));

            //1*2 + 2*7 + 3*9 + 4*1 + 5*4 + 6*6 + 7*3 + 8*5 + 9*8 + 10*2 + 11*7 + 12*9
            //  2 - 7 - 9 - 1 - 4 - 6 - 3 - 5 - 8 - 2 - 7 - 9
            int rezultat;
            int valCNP = ((c1 * 2 + c2 * 7 + c3 * 9 + c4 * 1 + c5 * 4 + c6 * 6 + c7 * 3 + c8 * 5 + c9 * 8 + c10 * 2 + c11 * 7 + c12 * 9) % 11);

            if (valCNP == 10)
            {
                rezultat = 1;
            }
            else
            {
                rezultat = valCNP;
            }

            if (Convert.ToInt16(strCNP.Substring(strCNP.Length - 1, 1)) != rezultat)
            {
                raspuns.corect=false;
                raspuns.detalii="CNP:\"" +strCNP + "\" eronat. Cifra de control incorecta.";
                return raspuns;
            }
            //cnp corect
            raspuns.corect=true;
            raspuns.detalii="CNP:\"" + strCNP + "\" este corect.";
            return raspuns;
        }
    }
}