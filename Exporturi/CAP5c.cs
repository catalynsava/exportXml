using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP5c
    {
        public static bool make_CAP5cxml(string strIdRol)
        {
            try
            {
                int codNomenclator=0;
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5c\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
                {
                    Ajutatoare.scrielinie("eroriXML.log", " există deja: " + AjutExport.numefisier(strIdRol) + "xml");
                    return false;
                }

                //siruta--
                string strSQL = "SELECT * FROM datgen;";
                OleDbCommand cmdDateGenerale = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drDateGenerale = cmdDateGenerale.ExecuteReader();
                if (drDateGenerale.Read() == false){return false;}
                Sirute datgenSirute=new Sirute(drDateGenerale["localitate"].ToString(), drDateGenerale["judet"].ToString());
                if (datgenSirute.Siruta == "" | datgenSirute.SirutaJudet == "" | datgenSirute.SirutaSuperioara == "")
                {
                    Console.WriteLine(datgenSirute.SirutaJudet+ " " + datgenSirute.SirutaSuperioara + " " + datgenSirute.Siruta);
                    return false;
                }
                //--

                //baza de date--
                strSQL = "SELECT ROL.nrcrt, CAP5c.sup FROM CAP5c LEFT JOIN (SELECT * FROM NOMCAP5c) AS ROL ON CAP5c.NrCrt = ROL.NrCrt WHERE CAP5c.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";
                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //--

                //DOCUMENT_RAN
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5c\\" + strGosp + "xml", settings);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("DOCUMENT_RAN");        //DOCUMENT_RAN
                //--

                //header
                xmlWriter.WriteStartElement("HEADER");              //HEADER
                xmlWriter.WriteStartElement("codXml");              //codXml
                xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
                xmlWriter.WriteEndElement();                        //inchid codXml
                xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());
                xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");
                xmlWriter.WriteElementString("sirutaUAT", datgenSirute.SirutaSuperioara);
                xmlWriter.WriteEndElement();                        //inchid HEADER
                //--

                //BODY
                xmlWriter.WriteStartElement("BODY");                        //BODY
                xmlWriter.WriteStartElement("gospodarie");                  //gospodarie
                xmlWriter.WriteAttributeString("identificator", strGosp);
                xmlWriter.WriteStartElement("anRaportare");                 //anRaportare
                xmlWriter.WriteAttributeString("an", "2020");
                xmlWriter.WriteStartElement("capitol_5c");                  //capitol
                xmlWriter.WriteAttributeString("codCapitol", "CAP5c");
                xmlWriter.WriteAttributeString("denumire", "Alte plantații pomicole aflate în teren agricol, pe raza localității");
                //--

                //parcurg--
                while (drXML.Read())
                {
                    xmlWriter.WriteStartElement("pom_alte_plantatii_pomicole");    //rand 

                    xmlWriter.WriteAttributeString("codNomenclator", (codNomenclator+Convert.ToInt32(drXML["nrcrt"].ToString())).ToString() );
                    xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                    switch (drXML["nrcrt"].ToString())
                    {
                        case "1":
                            xmlWriter.WriteAttributeString("denumire", "Arbuști fructiferi total cod (02+03)");
                            break;
                        case "2":
                            xmlWriter.WriteAttributeString("denumire", "- Arbuști fructiferi pe rod");
                            break;
                        case "3":
                            xmlWriter.WriteAttributeString("denumire", "- Arbuști fructiferi tineri");
                            break;
                        case "4":
                            xmlWriter.WriteAttributeString("denumire", "Plantații de duzi în masiv");
                            break;
                        case "5":
                            xmlWriter.WriteAttributeString("denumire", "Alte plantații pomicole aflate în teren agricol (Plantații de pomi de Crăciun înființate în teren agricol)");
                            break;            
                    }
                        xmlWriter.WriteStartElement("nrARI");                   //nrARI
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(drXML["sup"].ToString()));
                        xmlWriter.WriteEndElement();                            // close nrARI
                        xmlWriter.WriteStartElement("nrHA");                    //nrHA
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(drXML["sup"].ToString()));
                        xmlWriter.WriteEndElement();                            //close nrHA  
                        xmlWriter.WriteEndElement();                            //inchid rand
                }
                //--

                //--
                xmlWriter.WriteEndElement();                        //capitol
                xmlWriter.WriteEndElement();                        //anraportare
                xmlWriter.WriteEndElement();                        //gospodarie
                xmlWriter.WriteEndElement();                        //BODY
                //--

                //DOCUMENT_RAN--
                xmlWriter.WriteEndElement();                        //DOCUMENT_RAN
                xmlWriter.Close();
                drXML.Close();
                //--
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(AjutExport.numefisier(strIdRol) + "xml " + ex.Message);
                Ajutatoare.scrielinie("eroriXML.log",  AjutExport.numefisier(strIdRol) + "xml " + ex.Message);
                return false;
            }
        }
    }
}