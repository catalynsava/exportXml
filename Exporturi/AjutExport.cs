using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Xml;

namespace exportXml.Exporturi
{
    public static class AjutExport
    {

        public static List<string> tipact = new List<string> { "CP", "CF", "CA", "CD", "CM", "CAD", "CC", "AMD", "AV", "AC", "TP", "DP", "OP", "APV", "ADD", "HJ", "AAC", "CS", "DPR", "CMO", "ALT" };
        public static List<string> formaorganizare=new List<string> {"SNC", "SCS", "SA", "SCA", "SRL", "PFA", "II", "IF"};
        
        public static List<string> cnpmembri =new List<string>();

        public static List<string> cnptitulari =new List<string>();
        public static List<string> blacklistcnptitulari =new List<string>();
       
        
        public static string genereazaGUID()
        {
            return Guid.NewGuid().ToString();
        }
        public static string dataexportxml(){
            string strDataExport;
            DateTime azi = DateTime.Now;
            strDataExport = azi.Year.ToString().PadLeft(4, '0') + "-" + azi.Month.ToString().PadLeft(2, '0') + "-" + azi.Day.ToString().PadLeft(2, '0') + "T" + azi.Hour.ToString().PadLeft(2, '0') + ":" + azi.Minute.ToString().PadLeft(2, '0') + ":" + azi.Second.ToString().PadLeft(2, '0');
            return strDataExport;
        }
        public static string getcodpersoana(string strIdRol, string strcnp, string strnume, string strprenume)
        {
            string strSQL = "SELECT codP from Cap1 WHERE idrol=\"" + strIdRol + "\" AND cnp=\"" + strcnp + "\" AND nume=\"" + strnume + "\" AND prenume=\"" + strprenume + "\";";
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
            if (drTEMP.Read() == false)
            {
                return "";
            }
            else
            {
                return drTEMP["codP"].ToString();
            }
        }
         public static string numefisier(string strIDRol)
        {
            string str = strIDRol;
            return str.Substring(0, str.Length - 3);
        }
         public static void stergeInFolder(string strFolder)
        {
            string[] filePaths = Directory.GetFiles(strFolder);
            foreach (string filePath in filePaths)
            File.Delete(filePath);
        }
        public static string XMLok(string strXMLpath)
        {
            XmlReaderSettings booksSettings = new XmlReaderSettings();
            booksSettings.Schemas.Add("", AppDomain.CurrentDomain.BaseDirectory.ToString() + @"ran_schema_1.1.xsd");
            booksSettings.ValidationType = ValidationType.Schema;

            XmlReader books = XmlReader.Create(strXMLpath, booksSettings);
            try
            {
                while (books.Read()) { }
                books.Close();
                return "ok";
            }
            catch (Exception Err)
            {
                Validari.Ajutatoare.scrielinie("ran_schema_1.1_err.log",strXMLpath + " " + Err.Message);
                books.Close();
                return Err.Message;
                throw;
            }
        }
        public static bool moveWrongXML(string fileFrom, string fileWhere){
            if (File.Exists(fileWhere)){
                File.Delete(fileWhere);
            }
            File.Move(fileFrom, fileWhere);

            return true;
        }
         public static string scoateHa(string strValoare)
        {
            string strReturn = "";
            bool amgasitpunct=false;
            for (int x = 0; x < strValoare.Length; x++)
            {
                if (strValoare.Substring(x, 1) == "." | strValoare.Substring(x, 1) == ",")
                {
                    amgasitpunct=true ;
                    return strReturn;
                }
                else
                {
                    strReturn += strValoare.Substring(x, 1);
                }
            }
            if(amgasitpunct==true ){
                return "0";
            }else{
                return strReturn;
            }
            
        }
        public static string scoateAri(string strValoare)
        {
            string strReturn = "";
            bool boolStart = false;
            for (int x = 0; x < strValoare.Length; x++)
            {
                if (strValoare.Substring(x, 1) == "." | strValoare.Substring(x, 1) == ",")
                {
                    boolStart = true;
                }
                else
                {
                    if (boolStart == true)
                    {
                        strReturn += strValoare.Substring(x, 1);
                    }
                }
            }
            decimal SubHa=Convert.ToDecimal("0," + strReturn);
            //Console.WriteLine(strValoare + " -> " + (SubHa * 100).ToString("0.##").Replace(",", "."));
            return (SubHa * 100).ToString("0.##").Replace(",", ".");
        }

        public static string getBlankData()
        {
            DateTime BlankData = new DateTime();
            string strBlankData = BlankData.Year.ToString().PadLeft(4, '0') + "-" + BlankData.Month.ToString().PadLeft(2, '0') + "-" + BlankData.Day.ToString().PadLeft(2, '0') + "T" + BlankData.Hour.ToString().PadLeft(2, '0') + ":" + BlankData.Minute.ToString().PadLeft(2, '0') + ":" + BlankData.Second.ToString().PadLeft(2, '0');

            return strBlankData;
        }
        public static string genereazaCNP(string strJudet, DateTime dateData)
        {
            string strCodCnpJud = "";
            string CnpGen;
            string strRandom;
            switch (strJudet)
            {
                case "ALBA":
                    strCodCnpJud = "01";
                    break;

                case "ARAD":
                    strCodCnpJud = "02";
                    break;

                case "ARGES":
                    strCodCnpJud = "03";
                    break;

                case "BACAU":
                    strCodCnpJud = "04";
                    break;

                case "BIHOR":
                    strCodCnpJud = "05";
                    break;

                case "BISTRITA-NASAUD":
                    strCodCnpJud = "06";
                    break;

                case "BOTOSANI":
                    strCodCnpJud = "07";
                    break;

                case "BRASOV":
                    strCodCnpJud = "08";
                    break;

                case "BRAILA":
                    strCodCnpJud = "09";
                    break;

                case "BUZAU":
                    strCodCnpJud = "10";
                    break;

                case "CARAS-SEVERIN":
                    strCodCnpJud = "11";
                    break;

                case "CLUJ":
                    strCodCnpJud = "12";
                    break;

                case "CONSTANTA":
                    strCodCnpJud = "13";
                    break;

                case "COVASTA":
                    strCodCnpJud = "14";
                    break;

                case "DAMBOVITA":
                    strCodCnpJud = "15";
                    break;

                case "DOLJ":
                    strCodCnpJud = "16";
                    break;

                case "GALATI":
                    strCodCnpJud = "17";
                    break;

                case "GORJ":
                    strCodCnpJud = "18";
                    break;

                case "HARGHITA":
                    strCodCnpJud = "19";
                    break;

                case "HUNEDOARA":
                    strCodCnpJud = "20";
                    break;

                case "IALOMITA":
                    strCodCnpJud = "21";
                    break;

                case "IASI":
                    strCodCnpJud = "22";
                    break;

                case "ILFOV":
                    strCodCnpJud = "23";
                    break;

                case "MARAMURES":
                    strCodCnpJud = "24";
                    break;

                case "MEHEDINTI":
                    strCodCnpJud = "25";
                    break;

                case "MURES":
                    strCodCnpJud = "26";
                    break;

                case "NEAMT":
                    strCodCnpJud = "27";
                    break;

                case "OLT":
                    strCodCnpJud = "28";
                    break;

                case "PRAHOVA":
                    strCodCnpJud = "29";
                    break;

                case "SATU MARE":
                    strCodCnpJud = "30";
                    break;

                case "SALAJ":
                    strCodCnpJud = "31";
                    break;

                case "SIBIU":
                    strCodCnpJud = "32";
                    break;

                case "SUCEAVA":
                    strCodCnpJud = "33";
                    break;

                case "TELEORMAN":
                    strCodCnpJud = "34";
                    break;

                case "TIMIS":
                    strCodCnpJud = "35";
                    break;

                case "TULCEA":
                    strCodCnpJud = "36";
                    break;

                case "VASLUI":
                    strCodCnpJud = "37";
                    break;

                case "VALCEA":
                    strCodCnpJud = "38";
                    break;

                case "VRANCEA":
                    strCodCnpJud = "39";
                    break;

                case "BUCURESTI":
                    strCodCnpJud = "40";
                    break;

                case "BUCURESTI SECTORUL 1":
                    strCodCnpJud = "41";
                    break;

                case "BUCURESTI SECTORUL 2":
                    strCodCnpJud = "42";
                    break;

                case "BUCURESTI SECTORUL 3":
                    strCodCnpJud = "43";
                    break;

                case "BUCURESTI SECTORUL 4":
                    strCodCnpJud = "44";
                    break;

                case "BUCURESTI SECTORUL 5":
                    strCodCnpJud = "45";
                    break;

                case "BUCURESTI SECTORUL 6":
                    strCodCnpJud = "46";
                    break;

                case "CALARASI":
                    strCodCnpJud = "51";
                    break;

                case "GIURGIU":
                    strCodCnpJud = "52";
                    break;
            }
            CnpGen = "9" + 
                    dateData.Year.ToString().Substring(dateData.Year.ToString().Length - 2, 2) + 
                    dateData.Month.ToString().PadLeft(2, '0') + 
                    dateData.Day.ToString().PadLeft(2, '0') + 
                    strCodCnpJud;
            

            //Debug.Print(dateData.Year.ToString().Substring(length(dateData.Year.ToString())-2,2));
            Random random = new Random();
            int target = random.Next(0, 1000);
            strRandom = target.ToString().PadLeft(3, '0');
            CnpGen += strRandom;

            int c1 = Convert.ToInt16(CnpGen.Substring(0, 1));
            int c2 = Convert.ToInt16(CnpGen.Substring(1, 1));
            int c3 = Convert.ToInt16(CnpGen.Substring(2, 1));
            int c4 = Convert.ToInt16(CnpGen.Substring(3, 1));
            int c5 = Convert.ToInt16(CnpGen.Substring(4, 1));
            int c6 = Convert.ToInt16(CnpGen.Substring(5, 1));
            int c7 = Convert.ToInt16(CnpGen.Substring(6, 1));
            int c8 = Convert.ToInt16(CnpGen.Substring(7, 1));
            int c9 = Convert.ToInt16(CnpGen.Substring(8, 1));
            int c10 = Convert.ToInt16(CnpGen.Substring(9, 1));
            int c11 = Convert.ToInt16(CnpGen.Substring(10, 1));
            int c12 = Convert.ToInt16(CnpGen.Substring(11, 1));



            int valCNP = ((c1 * 2 + c2 * 7 + c3 * 9 + c4 * 1 + c5 * 4 + c6 * 6 + c7 * 3 + c8 * 5 + c9 * 8 + c10 * 2 + c11 * 7 + c12 * 9) % 11);

            if (valCNP == 10)
            {
                CnpGen += "1";
            }
            else
            {
                CnpGen += valCNP.ToString();
                
            }

            return CnpGen;
        }

        public static string GetNumeJudetRol(string idRol)
        {
            string strReturn="";
            
            OleDbCommand cmdTEMP = new OleDbCommand("SELECT judet FROM adrrol WHERE idRol=\"" + idRol + "\"", BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
            if (drTEMP.Read() == true)
            {
                
                strReturn = drTEMP["judet"].ToString();
                drTEMP.Close();
                return strReturn;
            }else{
                return "";
            }
        }
        public static string getCodCladire(string dacaAnexa, string strmaterial, string dacaInstalatii)
        {
            
            
            string strReturn = "";
            string[,] nomenclator = 
            {
                { "NU, DA, DA", "A.1." }, 
                { "NU, DA, NU", "A.1." },
                { "NU, NU, DA", "B.1." },
                { "NU, NU, NU", "B.2." },
                { "DA, DA, DA", "C.1." },
                { "DA, DA, NU", "C.2." },
                { "DA, NU, DA", "D.1." },
                { "DA, NU, NU", "D.2." },
            };

            string strAnexa="";
            if (dacaAnexa == "DA")
            {
                strAnexa = dacaAnexa;
            }
            else
            {
                strAnexa = "NU";
            }

            string strChimic="";
            XmlDocument Doc = new XmlDocument();
            Doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "materiale.xml");
            //XmlNodeList nodeList = Doc.SelectNodes("materiale");
            
            XmlNode root = Doc.FirstChild;
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                if (strmaterial == root.ChildNodes[i].ChildNodes[0].InnerText)
                {
                    strChimic = "DA";
                }
            }
            if (strChimic == "")
            {
                strChimic = "NU";
            }

            string strInstalatii="";
            if(dacaInstalatii=="DA"){
                strInstalatii = "DA";
            }else{
                strInstalatii = "NU";
            }

            string ptNomenclator;
            ptNomenclator = strAnexa + ", " + strChimic + ", " + strInstalatii;


            for (int x = 0; x < nomenclator.Length; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    if (ptNomenclator == nomenclator[x, 0] == true)
                    {
                        strReturn = nomenclator[x, 1];
                        return strReturn;
                    }
                }
            }
            if (strReturn=="")
            {
                return "ER";
            }
            return "ER";
        }
        public static String dataro_dataxml(string dataro)
        {
            if (dataro.Length==10)
            {
                string dataxml = dataro.Substring(dataro.Length - 4, 4) + "-";
                dataxml += dataro.Substring(3, 2) + "-";
                dataxml += dataro.Substring(0, 2) + "T00:00:00";
                return dataxml;
            }
            else
            {
                return "1900-01-01T00:00:00";
            }
        }
        public static int getSuprafataContrArenda(string nrContr, string dataContr)
        {

            string strSQL = "SELECT SUM(suprafata)*10000 as suprtot FROM terenuriA WHERE nrContr=\"" + nrContr + "\" AND dataContr=\"" + dataContr + "\";";
            try
            {
                OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
                if (drTEMP.Read())
                {
                    return Convert.ToInt32(drTEMP["suprtot"]);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to data source " + ex.ToString());
                return 0;
            }
        }
        public static int getSuprafataContrConcesiune(string nrContr, string dataContr)
        {

            string strSQL = "SELECT SUM(suprafata)*10000 as suprtot FROM terenuriB WHERE nrContr=\"" + nrContr + "\" AND dataContr=\"" + dataContr + "\";";
            try
            {
                OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
                if (drTEMP.Read())
                {
                    return Convert.ToInt32(drTEMP["suprtot"]);
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to data source " + ex.ToString());
                return 0;
            }
        }

        public static string getCNPdupaNumeInitialaPrenume(string NumeString, string IString, string PrenumeString)
        {
            string strSQL = "SELECT cnp FROM adrrol where nume like \"*" + NumeString + "*\"";
            strSQL += " AND";
            strSQL += " sirues like\"*" + IString + "*\"";
            strSQL += " AND";
            strSQL += " prenume like\"*" + PrenumeString + "*\"";
            try
            {
                OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune );
                OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
                if (drTEMP.Read())
                {
                    return drTEMP[0].ToString();
                }
                else
                {
                    return "";
                }
                   
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to data source " + ex.ToString());
                return "";
            }
        }
        public static string getCUIdupaNumeFirma(string NumeString)
        {
            string strSQL = "SELECT cnp FROM adrrol where nume like \"%" + NumeString + "%\";";
            try
            {
                OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                //cmdTEMP.CommandType = CommandType.;
                OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

                if (drTEMP.Read())
                {
                    return drTEMP[0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to data source " + ex.ToString() + " " + ex.Message);
                return "";
            }
        }
        public static string getTipExploatatie(string cnp)
        {
            string strSQL = "SELECT tipExploa FROM adrrol where cnp= \"" + cnp + "\"";
            strSQL+=" or cnp=\"" + "RO " + cnp + "\"";

            try
            {
                OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                //cmdTEMP.CommandType = CommandType.;
                OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

                if (drTEMP.Read())
                {
                    return drTEMP[0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to data source " + ex.ToString() + " " + ex.Message);
                return "";
            }
        }
        public static List<string> getProduse(string nrAtest)
        {
            List<string> produse = new List<string>();

            string strSQL = "SELECT produs from produse WHERE nrAtest=\"" + nrAtest + "\";";
            //Debug.Print(strSQL);
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
            while (drTEMP.Read())
            {
                produse.Add(drTEMP["produs"].ToString());
            }

            return produse;
        }
        public static bool EsteDeTipData(string DataParam){
            if(DataParam.Length<10){
                return false;
            }
            string yearString=DataParam.Substring(DataParam.Length -4,4);
            string monthString=DataParam.Substring(4,2);
            string dayString=DataParam.Substring(0,2);
            try
            {
                DateTime dateTime=new DateTime(Convert.ToInt32(yearString), Convert.ToInt32(monthString), Convert.ToInt32(dayString));
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                throw;
            }
        }
        public static string getFormaDeOrganizare(string denumireParam){
            if(denumireParam==""){
                return "SC";
            }
            for (int x = 0; x < 10; x++)
            {
                denumireParam=denumireParam.Replace("  "," ");
            }
            int spatiu=0 ;
            string den1="";
            string den2="";
            string den3="";
            string den4="";
            string den5="";
            
            for (int i = 0; i < denumireParam.Length; i++)
            {
                if(denumireParam.Substring(i,1)==" "){
                    spatiu+=1;
                }else{
                    switch (spatiu)
                    {
                        case 0:
                            den1+=denumireParam.Substring(i,1);
                            break;
                        case 1:
                            den2+=denumireParam.Substring(i,1);
                            break;
                        case 2:
                            den3+=denumireParam.Substring(i,1);
                            break;
                        case 3:
                            den4+=denumireParam.Substring(i,1);
                            break;
                        case 4:
                            den5+=denumireParam.Substring(i,1);
                            break;
                    }
                }
            }
            if(formaorganizare.Contains(den1)){
                return den1;
            }
            if(formaorganizare.Contains(den5)){
                return den5;
            }
            if(formaorganizare.Contains(den2)){
                return den2;
            }
            if(formaorganizare.Contains(den3)){
                return den3;
            }
            if(formaorganizare.Contains(den4)){
                return den4;
            }
            //--
            if(denumireParam.Length>3 && 
                denumireParam.Substring(denumireParam.Length-3, 3)==" SA"){
                    return "SA";
            }
            if(denumireParam.Length>3 && 
                denumireParam.Substring(denumireParam.Length-3, 3)==" II"){
                    return "II";
            }
            if(denumireParam.Length>3 && 
                denumireParam.Substring(denumireParam.Length-3, 3)==" IF"){
                    return "IF";
            }

            if(denumireParam.Length>4 && 
                denumireParam.Substring(denumireParam.Length-4, 4)==" SNC"){
                    return "SNC";
            }

            if(denumireParam.Length>4 && 
                denumireParam.Substring(denumireParam.Length-4, 4)==" SCS"){
                    return "SCS";
            }

            if(denumireParam.Length>4 && 
                denumireParam.Substring(denumireParam.Length-4, 4)==" SCA"){
                    return "SCA";
            }

            if(denumireParam.Length>4 && 
                denumireParam.Substring(denumireParam.Length-4, 4)==" SRL"){
                    return "SRL";
            }

            if(denumireParam.Length>4 && 
                denumireParam.Substring(denumireParam.Length-4, 4)==" PFA"){
                    return "PFA";
            }

            if (denumireParam.Length>3 && 
                denumireParam.Substring(0, 3) == "PFA")
            {
                return "PFA";
            }

            if (denumireParam.Length>6 && 
                denumireParam.Substring(1, 6) == "SCOALA")
            {
                return "SNC";
            }

            if (denumireParam.Length>9 && 
                denumireParam.Substring(0, 9) == "GRADINITA")
            {
                return "SNC";
            }
            if (denumireParam.Length>7 && 
                denumireParam.Substring(0, 7) == "PAROHIA")
            {
                return "SNC";
            }

            if (denumireParam.Length>9 && 
                denumireParam.Substring(0, 9) == "DISPENSAR")
            {
                return "SNC";
            }

            return "SRL";
        }
        
    }
}