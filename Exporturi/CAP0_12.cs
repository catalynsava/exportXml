using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP0_12
    {
        public static bool make_CAP0_12xml(string strIdRol)
        {

            string strGosp = strIdRol;
            strGosp = strGosp.Substring(0, strIdRol.Length - 3);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
            {
                return false;
            }


            //date generale
            string strSQL = "SELECT * FROM datgen;";
            OleDbCommand cmdDateGenerale = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drDateGenerale = cmdDateGenerale.ExecuteReader();
            if (drDateGenerale.Read() == false)
            {
                return false;
            }

            //inregistrare din adrese rol
            strSQL = "SELECT * FROM adrrol WHERE idRol=\"" + strIdRol + "\";";
            OleDbCommand cmdAdrrol = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drAdrrol = cmdAdrrol.ExecuteReader();
            if (drAdrrol.Read() == false)
            {
                return false;
            }

            if(AjutExport.cnptitulari.Contains(drAdrrol["cnp"].ToString())){
                Console.WriteLine(drAdrrol["idrol"] + " " + drAdrrol["nume"] + " " + drAdrrol["prenume"] + " " + drAdrrol["cnp"] + " este titular și la alt rol de mai multe ori.");
                Ajutatoare.scrielinie("eroriXML.log", drAdrrol["idrol"] + " " + drAdrrol["nume"] + " " + drAdrrol["prenume"] + " " + drAdrrol["cnp"] + " este titular și la alt rol de mai multe ori.");
                AjutExport.blacklistcnptitulari.Add(drAdrrol["idrol"] + " " + drAdrrol["nume"] + " " + drAdrrol["prenume"] + " " + drAdrrol["cnp"]);
                drAdrrol.Close();
                return false;
            }else{
                 AjutExport.cnptitulari.Add(drAdrrol["cnp"].ToString());
            }

            Sirute adrrolSirute=new Sirute(drAdrrol["localitate"].ToString(), drAdrrol["judet"].ToString());
            
            if (adrrolSirute.Siruta == "" | adrrolSirute.SirutaJudet == "" | adrrolSirute.SirutaSuperioara == "")
            {
                //Console.WriteLine(adrrolSirute.SirutaJudet+ " " + adrrolSirute.SirutaSuperioara + " " + adrrolSirute.Siruta);
                return false;
            }





            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;


            XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(strIdRol) + "xml", settings);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("DOCUMENT_RAN");
            xmlWriter.WriteStartElement("HEADER");

            xmlWriter.WriteStartElement("codXml");
            xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
            xmlWriter.WriteEndElement();



            xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());

            xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");

            xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("BODY");
            xmlWriter.WriteStartElement("gospodarie");
            xmlWriter.WriteAttributeString("identificator", strGosp);
            xmlWriter.WriteStartElement("capitol_0_12");
            xmlWriter.WriteAttributeString("codCapitol", "CAP0_12");
            xmlWriter.WriteAttributeString("denumire", "date_identificare_gospodarie_PF");
            xmlWriter.WriteStartElement("date_identificare_gospodarie_PF");

            //adresaGospodarie
            xmlWriter.WriteStartElement("adresaGospodarie");
            xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
            xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
            xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
            xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());

            xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
            xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
            xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
            xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
            xmlWriter.WriteEndElement();


            xmlWriter.WriteElementString("codExploatatie", "1");



            strSQL = "SELECT * FROM adrPF WHERE idRol=\"" + strIdRol + "\" AND codP=\"" + AjutExport.getcodpersoana(strIdRol, drAdrrol["cnp"].ToString(), drAdrrol["nume"].ToString(), drAdrrol["prenume"].ToString()) + "\"";
            OleDbCommand cmdAdresaPF = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drAdresaPF = cmdAdresaPF.ExecuteReader();
            if (drAdresaPF.Read() == true)
            {

                Sirute adrpfSirute=new Sirute(drAdresaPF["localitate"].ToString(), drAdresaPF["judet"].ToString());
                //Console.WriteLine(adrpfSirute.SirutaJudet+ " " + adrpfSirute.SirutaSuperioara + " " + adrpfSirute.Siruta);

                if (adrpfSirute.SirutaJudet== "" || adrpfSirute.SirutaSuperioara == "" || adrpfSirute.Siruta == "")
                {
                    //daca nu gasesc siruta atunci merg pe adresa gospodarie
                    //Console.WriteLine(adrpfSirute.SirutaJudet+ " " + adrpfSirute.SirutaSuperioara + " " + adrpfSirute.Siruta);
                     
                    xmlWriter.WriteStartElement("domiciliuFiscalRo");
                    xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
                    xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
                    xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
                    xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
                    xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
                    xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
                    xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
                    xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
                    xmlWriter.WriteEndElement();
                }
                else
                {
                    //daca gasesc sirutele merg pe adresa de domiciliu
                    xmlWriter.WriteStartElement("domiciliuFiscalRo");
                    xmlWriter.WriteElementString("apartament", drAdresaPF["ap"].ToString());
                    xmlWriter.WriteElementString("bloc", drAdresaPF["bloc"].ToString());
                    xmlWriter.WriteElementString("numar", drAdresaPF["nr"].ToString());
                    xmlWriter.WriteElementString("scara", drAdresaPF["scara"].ToString());
                    xmlWriter.WriteElementString("sirutaJudet", adrpfSirute.SirutaJudet);
                    xmlWriter.WriteElementString("sirutaLocalitate",adrpfSirute.Siruta);
                    xmlWriter.WriteElementString("sirutaUAT", adrpfSirute.SirutaSuperioara);
                    xmlWriter.WriteElementString("strada", drAdresaPF["strada"].ToString());
                    xmlWriter.WriteEndElement();
                }
            }
            else
            {
                //daca nu are deloc adresa de domiciliu, mer pe adresa de gospodarie
                xmlWriter.WriteStartElement("domiciliuFiscalRo");
                xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
                xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
                xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
                xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
                xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
                xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
                xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
                xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
                xmlWriter.WriteEndElement();
            }

            

            xmlWriter.WriteElementString("nrUnicIdentificare", drAdrrol["nrUI"].ToString());

            xmlWriter.WriteStartElement("pozitieGospodarie");
            xmlWriter.WriteElementString("pozitiaAnterioara", "0");
            xmlWriter.WriteElementString("pozitieCurenta", drAdrrol["poz"].ToString());
            xmlWriter.WriteElementString("volumul", drAdrrol["vol"].ToString());

            int x = 0;
            Int32.TryParse(drAdrrol["rolimp"].ToString(), out x);
            xmlWriter.WriteElementString("rolNominalUnic", x.ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("tipDetinator", drAdrrol["tip"].ToString());

            string strNomCodExploatatie;
            switch (drAdrrol["tipExploa"].ToString())
            {
                case "1. Gospodarie/Exploatatie agricola individuala":
                    strNomCodExploatatie = "1";
                    break;
                case "2. Persoana fizica autorizata/ Intreprindere individuala/ Intreprindere familiala":
                    strNomCodExploatatie = "2";
                    break;
                case "3. a) regie autonoma":
                    strNomCodExploatatie = "3";
                    break;
                case "3. b) societate/asociatie agricola (Legea nr. 36/1991)":
                    strNomCodExploatatie = "4";
                    break;
                case "3. c) societate comerciala cu capital majoritar privat (Legea nr. 31/1990)":
                    strNomCodExploatatie = "5";
                    break;
                case "3. d) societate comerciala cu capital majoritar de stat (Legea nr. 31/1990)":
                    strNomCodExploatatie = "6";
                    break;
                case "3. e) institut/ statiune de cercetare":
                    strNomCodExploatatie = "7";
                    break;
                case "3. f) unitate/ subdiviziune administrativ teritoriala":
                    strNomCodExploatatie = "8";
                    break;
                case "3. g) alte institutii publice centrale sau locale":
                    strNomCodExploatatie = "9";
                    break;
                case "3. h) unitate cooperatista":
                    strNomCodExploatatie = "10";
                    break;
                case "3. i) alte tipuri (asociatie, fundatie, asezamant religios, scoala etc.)":
                    strNomCodExploatatie = "11";
                    break;
                default:
                    strNomCodExploatatie = "1";
                    break;
            }
            xmlWriter.WriteElementString("tipExploatatie", strNomCodExploatatie);

            xmlWriter.WriteStartElement("gospodar");
            xmlWriter.WriteElementString("nume", drAdrrol["nume"].ToString());
            xmlWriter.WriteElementString("prenume", drAdrrol["prenume"].ToString());

            if (drAdrrol["sirues"].ToString() == "")
            {
                xmlWriter.WriteElementString("initialaTata", "-");
            }
            else
            {
                xmlWriter.WriteElementString("initialaTata", drAdrrol["sirues"].ToString());
            }


            xmlWriter.WriteStartElement("cnp", "");
            xmlWriter.WriteAttributeString("value", drAdrrol["cnp"].ToString());
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            drAdrrol.Close();
            return true;
        }
    }
}