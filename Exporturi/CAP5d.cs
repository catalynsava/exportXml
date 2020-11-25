using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP5d
    {
        public static bool make_CAP5dxml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5d\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
                {
                    Ajutatoare.scrielinie("eroriXML.log", " există deja: " + AjutExport.numefisier(strIdRol) + "xml");
                    return false;
                }

                //siruta
                string strSQL = "SELECT * FROM datgen;";
                OleDbCommand cmdDateGenerale = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drDateGenerale = cmdDateGenerale.ExecuteReader();
                if (drDateGenerale.Read() == false)
                {
                    return false;
                }

                Sirute datgenSirute=new Sirute(drDateGenerale["localitate"].ToString(), drDateGenerale["judet"].ToString());

                if (datgenSirute.Siruta == "" | datgenSirute.SirutaJudet == "" | datgenSirute.SirutaSuperioara == "")
                {
                    Console.WriteLine(datgenSirute.SirutaJudet+ " " + datgenSirute.SirutaSuperioara + " " + datgenSirute.Siruta);
                    return false;
                }
                //--------------------------------//

                //datele din baza de date
                strSQL = "SELECT ROL.nrcrt, CAP5d.sup FROM CAP5d LEFT JOIN (SELECT * FROM NOMCAP5d) AS ROL ON CAP5d.NrCrt = ROL.NrCrt WHERE CAP5d.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5d\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
                //header
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("DOCUMENT_RAN");        //deschid1
                xmlWriter.WriteStartElement("HEADER");              //deschid2
                xmlWriter.WriteStartElement("codXml");              //deschid3
                xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
                xmlWriter.WriteEndElement();                        //inchid3
                xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());
                xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");
                xmlWriter.WriteElementString("sirutaUAT", datgenSirute.SirutaSuperioara);
                xmlWriter.WriteEndElement();                        //inchid2
                //body
                xmlWriter.WriteStartElement("BODY");                //deschid2

                xmlWriter.WriteStartElement("gospodarie");         //deschid3
                xmlWriter.WriteAttributeString("identificator", strGosp);

                xmlWriter.WriteStartElement("anRaportare");         //deschid4
                xmlWriter.WriteAttributeString("an", "2020");
                //capitolul
                xmlWriter.WriteStartElement("capitol_5d");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP5d");
                xmlWriter.WriteAttributeString("denumire", "Vii, pepiniere viticole și hameiști situate pe raza localității");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("vii_hamei");         //deschid6
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
                                xmlWriter.WriteAttributeString("denumire", "Vii pe rod pentru struguri de vin - total cod (02+...+05)");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "Vii nobile pentru vinuri DOC");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "Vii nobile pentru vinuri IG");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "Vii nobile pentru alte vinuri");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "Vii hibride pentru vinuri");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "Vii nobile pe rod, pentru struguri de masă");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Plantații de vii tinere");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Pepiniere viticole");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "Plantații portaltoi");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "Teren în pregătire pentru plantații viticole");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "Terenuri cu vii abandonate");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "Hameiști pe rod");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Hameiști tineri neintrați pe rod");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "Teren în pregătire pentru plantații de hamei");
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                        xmlWriter.WriteStartElement("nrARI");               //deschid7
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(drXML["sup"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid7
                        xmlWriter.WriteStartElement("nrHA");               //deschid7
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(drXML["sup"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid7
                        xmlWriter.WriteEndElement();
                    
                }
                xmlWriter.WriteEndElement();                        //inchid5
                xmlWriter.WriteEndElement();                        //inchid4

                xmlWriter.WriteEndElement();                        //inchid3

                xmlWriter.WriteEndElement();                        //inchid2
                xmlWriter.WriteEndElement();                        //inchid1

                xmlWriter.Close();
                drXML.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                Ajutatoare.scrielinie("eroriXML.log",  AjutExport.numefisier(strIdRol) + "xml " + ex.Message);
                return false;
            }
        }
    }
}