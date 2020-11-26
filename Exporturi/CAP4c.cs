using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP4c
    {
        public static bool make_CAP4cxml(string strIdRol)
        {
            try
            {
                int codNomenclator=254;
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4c\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP4c.sup FROM CAP4c LEFT JOIN (SELECT * FROM NOMCAP4c) AS ROL ON CAP4c.NrCrt = ROL.NrCrt WHERE CAP4c.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";
                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //--

                //DOCUMENT_RAN
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4c\\" + strGosp + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_4c");                  //capitol
                xmlWriter.WriteAttributeString("codCapitol", "CAP4c");
                xmlWriter.WriteAttributeString("denumire", "Suprafața cultivată cu legume și cartofi în grădinile familiale pe raza localității");
                //--

                //parcurg--
                while (drXML.Read())
                {
                    xmlWriter.WriteStartElement("cultura_in_gradini");    //rand 

                    xmlWriter.WriteAttributeString("codNomenclator", (codNomenclator+Convert.ToInt32(drXML["nrcrt"].ToString())).ToString() );
                    xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                    switch (drXML["nrcrt"].ToString())
                    {
                        case "1":
                            xmlWriter.WriteAttributeString("denumire", "Legume în grădini familiale -total (exclusiv sămânţă) (02+03+...+06+08+...+23)");
                            break;
                        case "2":
                            xmlWriter.WriteAttributeString("denumire", "Varza alba");
                            break;
                        case "3":
                            xmlWriter.WriteAttributeString("denumire", "Salata verde");
                            break;
                        case "4":
                            xmlWriter.WriteAttributeString("denumire", "Spanac");
                            break;
                        case "5":
                            xmlWriter.WriteAttributeString("denumire", "Tomate");
                            break;
                        case "6":
                            xmlWriter.WriteAttributeString("denumire", "Castraveti");
                            break;
                        case "7":
                            xmlWriter.WriteAttributeString("denumire", "din care: cornison");
                            break;
                        case "8":
                            xmlWriter.WriteAttributeString("denumire", "Ardei");
                            break;
                        case "9":
                            xmlWriter.WriteAttributeString("denumire", "Vinete");
                            break;
                        case "10":
                            xmlWriter.WriteAttributeString("denumire", "Dovleci");
                            break;
                        case "11":
                            xmlWriter.WriteAttributeString("denumire", "Dovlecei");
                            break;
                        case "12":
                            xmlWriter.WriteAttributeString("denumire", "Morcovi");
                            break;
                        case "13":
                            xmlWriter.WriteAttributeString("denumire", "Telina (radacina)");
                            break;
                        case "14":
                            xmlWriter.WriteAttributeString("denumire", "Usturoi");
                            break;
                        case "15":
                            xmlWriter.WriteAttributeString("denumire", "Ceapa");
                            break;
                        case "16":
                            xmlWriter.WriteAttributeString("denumire", "Sfecla rosie");
                            break;
                        case "17":
                            xmlWriter.WriteAttributeString("denumire", "Ridichi de luna");
                            break;
                        case "18":
                            xmlWriter.WriteAttributeString("denumire", "Alte legume radacinoase (hrean, ridichi negre, patrunjel, pastârnac etc.) ");
                            break;
                        case "19":
                            xmlWriter.WriteAttributeString("denumire", "Legume pentru frunze (patrunjel, marar, leustean etc.)");
                            break;
                        case "20":
                            xmlWriter.WriteAttributeString("denumire", "Mazare pastai");
                            break;
                        case "21":
                            xmlWriter.WriteAttributeString("denumire", "Fasole pastai");
                            break;
                        case "22":
                            xmlWriter.WriteAttributeString("denumire", "Porumb zaharat");
                            break;
                        case "23":
                            xmlWriter.WriteAttributeString("denumire", "Alte legume");
                            break;
                        case "24":
                            xmlWriter.WriteAttributeString("denumire", "Capsuni");
                            break;
                        case "25":
                            xmlWriter.WriteAttributeString("denumire", "Plante medicinale si aromatice ");
                            break;
                        case "26":
                            xmlWriter.WriteAttributeString("denumire", "Flori, plante ornamentale si dendrologice");
                            break;
                        case "27":
                            xmlWriter.WriteAttributeString("denumire", "Cartofi total (28+29+30)");
                            break;
                        case "28":
                            xmlWriter.WriteAttributeString("denumire", "a) - timpurii si semitimpurii");
                            break;
                        case "29":
                            xmlWriter.WriteAttributeString("denumire", "b) - de vara");
                            break;
                        case "30":
                            xmlWriter.WriteAttributeString("denumire", "c) - de toamna");
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