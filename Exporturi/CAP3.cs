using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP3
    {
        public static bool make_CAP3xml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP3\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
                {
                    Ajutatoare.scrielinie("eroriXML.log", " există deja: " + AjutExport.numefisier(strIdRol) + "xml");
                    return false;
                }

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

                //--------------------------------------------------------------------------
                strSQL = "SELECT ROL.nrcrt, CAP3.inloc, CAP3.altloc, CAP3.tot FROM CAP3 LEFT JOIN (SELECT * FROM NOMCAP3) AS ROL ON CAP3.NrCrt = ROL.NrCrt WHERE CAP3.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";
                //Console.WriteLine(strSQL);

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;


                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP3\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("BODY");                //deschid2

                xmlWriter.WriteStartElement("gospodarie");         //deschid3
                xmlWriter.WriteAttributeString("identificator", strGosp);
                //xmlWriter.WriteAttributeString("identificator", strIdRol);

                xmlWriter.WriteStartElement("anRaportare");         //deschid4
                xmlWriter.WriteAttributeString("an", "2020");

                xmlWriter.WriteStartElement("capitol_3");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP3");
                xmlWriter.WriteAttributeString("denumire", "Modul de utilizare a suprafețelor agricole situate pe raza localității");

                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("mod_utilizare_suprafete_agricole");         //deschid6
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
                            xmlWriter.WriteAttributeString("denumire", "Suprafața agricolă în proprietate = Capitolul II, punctul a) Terenuri aflate în proprietate, cod 10,coloanele 2,5,8,11,14.");
                            break;
                        case "2":
                            xmlWriter.WriteAttributeString("denumire", "Suprafața agricolă primită (cod 03+ ... +08)");
                            break;
                        case "3":
                            xmlWriter.WriteAttributeString("denumire", "a) în arendă");
                            break;
                        case "4":
                            xmlWriter.WriteAttributeString("denumire", "b) în parte");
                            break;
                        case "5":
                            xmlWriter.WriteAttributeString("denumire", "c) cu titlu gratuit");
                            break;
                        case "6":
                            xmlWriter.WriteAttributeString("denumire", "d) în concesiune");
                            break;
                        case "7":
                            xmlWriter.WriteAttributeString("denumire", "e) în asociere");
                            break;
                        case "8":
                            xmlWriter.WriteAttributeString("denumire", "f) sub alte forme");
                            break;
                        case "9":
                            xmlWriter.WriteAttributeString("denumire", "Suprafața agricolă dată (cod 10+... + 15)");
                            break;
                        case "10":
                            xmlWriter.WriteAttributeString("denumire", "a) în arendă");
                            break;
                        case "11":
                            xmlWriter.WriteAttributeString("denumire", "b) în parte");
                            break;
                        case "12":
                            xmlWriter.WriteAttributeString("denumire", "c) cu titlu gratuit");
                            break;
                        case "13":
                            xmlWriter.WriteAttributeString("denumire", "d) în concesiune");
                            break;
                        case "14":
                            xmlWriter.WriteAttributeString("denumire", "e) în asociere");
                            break;
                        case "15":
                            xmlWriter.WriteAttributeString("denumire", "f) sub alte forme");
                            break;
                        case "16":
                            xmlWriter.WriteAttributeString("denumire", "din rândul 09 - la unități cu personalitate juridică");
                            break;
                        case "17":
                            xmlWriter.WriteAttributeString("denumire", "Suprafața agricolă utilizată cod (01+02-09)");
                            break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                        xmlWriter.WriteStartElement("nrARI");               //deschid7
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(drXML["tot"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid7
                        xmlWriter.WriteStartElement("nrHA");               //deschid7
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(drXML["tot"].ToString()));
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