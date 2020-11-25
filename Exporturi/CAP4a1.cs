using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP4a1
    {
        public static bool make_CAP4a1xml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\Cap4a1\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP4a1.sup FROM CAP4a1 LEFT JOIN (SELECT * FROM NOMCAP4a1) AS ROL ON CAP4a1.NrCrt = ROL.NrCrt WHERE CAP4a1.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";
                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //--

                //DOCUMENT_RAN
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\Cap4a1\\" + strGosp + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_4a1");                  //capitol
                xmlWriter.WriteAttributeString("codCapitol", "CAP4a1");
                xmlWriter.WriteAttributeString("denumire", "Culturi succesive în câmp, culturi intercalate, culturi modificate genetic pe raza localității");
                //--

                //parcurg--
                while (drXML.Read())
                {
                    xmlWriter.WriteStartElement("cultura_speciala_in_camp");    //cultura_speciala_in_camp 
                    xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                    xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                    switch (drXML["nrcrt"].ToString())
                    {
                        case "1":
                            xmlWriter.WriteAttributeString("denumire", "Culturi succesive în câmp cod (02+...+09)");
                            break;
                        case "2":
                            xmlWriter.WriteAttributeString("denumire", " - porumb boabe");
                            break;
                        case "3":
                            xmlWriter.WriteAttributeString("denumire", " - porumb verde furajer");
                            break;
                        case "4":
                            xmlWriter.WriteAttributeString("denumire", " - tomate");
                            break;
                        case "5":
                            xmlWriter.WriteAttributeString("denumire", " - varză alba");
                            break;
                        case "6":
                            xmlWriter.WriteAttributeString("denumire", " - castraveți");
                            break;
                        case "7":
                            xmlWriter.WriteAttributeString("denumire", " - fasole verde");
                            break;
                        case "8":
                            xmlWriter.WriteAttributeString("denumire", " - alte plante anuale pentru fân și masă verde");
                            break;
                        case "9":
                            xmlWriter.WriteAttributeString("denumire", " - alte culturi succesive");
                            break;
                        case "10":
                            xmlWriter.WriteAttributeString("denumire", "Culturi intercalate – total Cod (11+12+16+...+22)");
                            break;
                        case "11":
                            xmlWriter.WriteAttributeString("denumire", "Fasole boabe");
                            break;
                        case "12":
                            xmlWriter.WriteAttributeString("denumire", "Cartofi total cod (13+14+15)");
                            break;
                        case "13":
                            xmlWriter.WriteAttributeString("denumire", "a) - timpurii și semitimpurii");
                            break;
                        case "14":
                            xmlWriter.WriteAttributeString("denumire", "b) - de vară");
                            break;
                        case "15":
                            xmlWriter.WriteAttributeString("denumire", "c) - de toamnă");
                            break;
                        case "16":
                            xmlWriter.WriteAttributeString("denumire", "Căpșuni");
                            break;
                        case "17":
                            xmlWriter.WriteAttributeString("denumire", "Pepeni verzi");
                            break;
                        case "18":
                            xmlWriter.WriteAttributeString("denumire", "Pepeni galbeni");
                            break;
                        case "19":
                            xmlWriter.WriteAttributeString("denumire", "Dovleci furajeri");
                            break;
                        case "20":
                            xmlWriter.WriteAttributeString("denumire", "Lucernă");
                            break;
                        case "21":
                            xmlWriter.WriteAttributeString("denumire", "Trifoi");
                            break;
                        case "22":
                            xmlWriter.WriteAttributeString("denumire", "Alte culturi intercalate");
                            break;
                        case "23":
                            xmlWriter.WriteAttributeString("denumire", "Culturi modificate genetic cod (24+25)");
                            break;
                        case "24":
                            xmlWriter.WriteAttributeString("denumire", "Porumb");
                            break;
                        case "25":
                            xmlWriter.WriteAttributeString("denumire", "Alte culturi modificate genetic");
                            break;                        
                    }
                        xmlWriter.WriteStartElement("nrARI");                   //nrARI
                        xmlWriter.WriteAttributeString("value",AjutExport.scoateAri(drXML["sup"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid nrARI
                        xmlWriter.WriteStartElement("nrHA");                    //nrHA
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateHa( drXML["sup"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid nrHA
                        xmlWriter.WriteEndElement();                            //inchid cultura_in_camp
                    
                }
                //--

                //--
                xmlWriter.WriteEndElement();                        //capitol_4a1
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