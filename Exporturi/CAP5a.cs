using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP5a
    {
        public static bool make_CAP5axml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5a\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP5.rod, CAP5.tin FROM CAP5 LEFT JOIN (SELECT * FROM NOMCAP5) AS ROL ON CAP5.NrCrt = ROL.NrCrt WHERE CAP5.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5a\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_5a");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP5a");
                xmlWriter.WriteAttributeString("denumire", "Numărul pomilor răzleți pe raza localității");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("pom_razlet");         //deschid6
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
                                xmlWriter.WriteAttributeString("denumire", "Pomi fructiferi - total cod (02+03+...+13)");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "Meri");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "Peri");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "Piersici");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "Caiși și zarzări");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "Cireși");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Vișini");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Pruni");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "Nectarini");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "Nuci");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "Aluni");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "Castani");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Alți pomi (gutui, migdali, etc)");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "Duzi");
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                        xmlWriter.WriteStartElement("nrPomiPeRod");               //deschid7
                        xmlWriter.WriteAttributeString("value", drXML["rod"].ToString());
                        xmlWriter.WriteEndElement();                            //inchid7
                        xmlWriter.WriteStartElement("nrPomiTineri");               //deschid7
                        xmlWriter.WriteAttributeString("value", drXML["tin"].ToString());
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