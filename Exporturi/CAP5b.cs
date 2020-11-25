using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP5b
    {
        public static bool make_CAP5bxml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5b\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP5b.sup, CAP5b.buc FROM CAP5b LEFT JOIN (SELECT * FROM NOMCAP5b) AS ROL ON CAP5b.NrCrt = ROL.NrCrt WHERE CAP5b.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5b\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_5b");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP5b");
                xmlWriter.WriteAttributeString("denumire", "Suprafața plantațiilor pomicole și numărul pomilor pe raza localității");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("pom_plantatii_pomicole");         //deschid6
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
                                xmlWriter.WriteAttributeString("denumire", "Pomi fructiferi - total cod (02+08+13+... ...+22+25+26)");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "Meri cod (03+.....+07)");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "Jonathan");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "Golden Delicios");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "Idared");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "Starkrimson");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Alte soiuri");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Peri cod (09+.....+12)");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "Cure");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "Favorita lui Clapp");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "Williams");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "Alte soiuri");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Piersici");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "Caiși și zarzări");
                                break;
                            case "15":
                                xmlWriter.WriteAttributeString("denumire", "Cireși");
                                break;
                            case "16":
                                xmlWriter.WriteAttributeString("denumire", "Vișini");
                                break;
                            case "17":
                                xmlWriter.WriteAttributeString("denumire", "Pruni");
                                break;
                            case "18":
                                xmlWriter.WriteAttributeString("denumire", "Nectarini");
                                break;
                            case "19":
                                xmlWriter.WriteAttributeString("denumire", "Nuci");
                                break;
                            case "20":
                                xmlWriter.WriteAttributeString("denumire", "Aluni");
                                break;
                            case "21":
                                xmlWriter.WriteAttributeString("denumire", "Castani");
                                break;
                            case "22":
                                xmlWriter.WriteAttributeString("denumire", "Alți pomi (gutui, migdali etc)");
                                break;
                            case "23":
                                xmlWriter.WriteAttributeString("denumire", "Pepiniere pomicole");
                                break;
                            case "24":
                                xmlWriter.WriteAttributeString("denumire", "Suprafețe defrișate");
                                break;
                            case "25":
                                xmlWriter.WriteAttributeString("denumire", "Livezi tinere");
                                break;
                            case "26":
                                xmlWriter.WriteAttributeString("denumire", "Livezi în declin");
                                break;
                            case "27":
                                xmlWriter.WriteAttributeString("denumire", "Teren în pregătire");
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
                        xmlWriter.WriteStartElement("nrPomiPeRod");               //deschid7
                        xmlWriter.WriteAttributeString("value", drXML["buc"].ToString());
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