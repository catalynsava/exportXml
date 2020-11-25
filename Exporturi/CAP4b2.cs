using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP4b2
    {
        public static bool make_CAP4b2xml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\Cap4b2\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, Cap4b2.sup FROM Cap4b2 LEFT JOIN (SELECT * FROM NOMCap4b2) AS ROL ON Cap4b2.NrCrt = ROL.NrCrt WHERE Cap4b2.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";
                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //--

                //DOCUMENT_RAN
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\Cap4b2\\" + strGosp + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_4b2");                  //capitol
                xmlWriter.WriteAttributeString("codCapitol", "Cap4b2");
                xmlWriter.WriteAttributeString("denumire", "Suprafața cultivată în solarii și alte spații protejate pe raza localității");
                //--

                //parcurg--
                while (drXML.Read())
                {
                    xmlWriter.WriteStartElement("cultura_in_spatiu_protejat");    //rand 
                    xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                    xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                    switch (drXML["nrcrt"].ToString())
                    {
                        case "1":
                            xmlWriter.WriteAttributeString("denumire", "Legume în sere și/sau solarii – total exclusiv sămânță cod (02+03+...+09+11+...+16)");
                            break;
                        case "2":
                            xmlWriter.WriteAttributeString("denumire", "Varză albă");
                            break;
                        case "3":
                            xmlWriter.WriteAttributeString("denumire", "Varză roșie");
                            break;
                        case "4":
                            xmlWriter.WriteAttributeString("denumire", "Gulii și gulioare");
                            break;
                        case "5":
                            xmlWriter.WriteAttributeString("denumire", "Alte vărzoase");
                            break;
                        case "6":
                            xmlWriter.WriteAttributeString("denumire", "Salatăverde");
                            break;
                        case "7":
                            xmlWriter.WriteAttributeString("denumire", "Spanac");
                            break;
                        case "8":
                            xmlWriter.WriteAttributeString("denumire", "Tomate");
                            break;
                        case "9":
                            xmlWriter.WriteAttributeString("denumire", "Castraveți");
                            break;
                        case "10":
                            xmlWriter.WriteAttributeString("denumire", "din care: cornișon");
                            break;
                        case "11":
                            xmlWriter.WriteAttributeString("denumire", "Ardei");
                            break;
                        case "12":
                            xmlWriter.WriteAttributeString("denumire", "Vinete");
                            break;
                        case "13":
                            xmlWriter.WriteAttributeString("denumire", "Ridichi de lună");
                            break;
                        case "14":
                            xmlWriter.WriteAttributeString("denumire", "Dovlecei");
                            break;
                        case "15":
                            xmlWriter.WriteAttributeString("denumire", "Alte legume");
                            break;
                        case "16":
                            xmlWriter.WriteAttributeString("denumire", "Ciuperci - în sere și/sau solarii");
                            break;
                        case "17":
                            xmlWriter.WriteAttributeString("denumire", "Alte culturi în sere și/sau solarii cod (18+...+22)");
                            break;
                        case "18":
                            xmlWriter.WriteAttributeString("denumire", "Căpșuni");
                            break;
                        case "19":
                            xmlWriter.WriteAttributeString("denumire", "Plante medicinale și aromatice");
                            break;
                        case "20":
                            xmlWriter.WriteAttributeString("denumire", "Flori, plante ornamentale și dendrologice");
                            break;
                        case "21":
                            xmlWriter.WriteAttributeString("denumire", "Răsaduri");
                            break;
                        case "22":
                            xmlWriter.WriteAttributeString("denumire", "Arbuști fructiferi");
                            break;
                        case "23":
                            xmlWriter.WriteAttributeString("denumire", "Suprafața utilizată în sere și/sau solarii cod (01+17)");
                            break;
                        case "24":
                            xmlWriter.WriteAttributeString("denumire", "Ciuperci - în alte spații protejate");
                            break;                    
                    }
                        xmlWriter.WriteStartElement("nrMP");                   //nrMP 
                        xmlWriter.WriteAttributeString("value",drXML["sup"].ToString());
                        xmlWriter.WriteEndElement();                            //inchid suprafata exprimata diferit
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