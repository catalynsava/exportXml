using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP6
    {
        public static bool make_CAP6xml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP6\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP6.inloc FROM CAP6 LEFT JOIN (SELECT * FROM NOMCAP6) AS ROL ON CAP6.NrCrt = ROL.NrCrt WHERE CAP6.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP6\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_6");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP6");
                xmlWriter.WriteAttributeString("denumire", "Suprafețele efectiv irigate în câmp, situate pe raza localității");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("cultura_irigata");         //deschid6
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                             case "1":
                                xmlWriter.WriteAttributeString("denumire", "Teren arabil total cod (02+.... +19)");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "Grâu");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "Secară");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "Orz");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "Orzoaică");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "Porumb boabe");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Orez");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Floarea-soarelui");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "Soia boabe");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "Sfeclă de zahăr");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "Legume în câmp");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "Cartofi total");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Porumb verde furajer");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "Alte plante anuale pentru fân şi masă verde");
                                break;
                            case "15":
                                xmlWriter.WriteAttributeString("denumire", "Trifoi şi amestec de plante furajere ");
                                break;
                            case "16":
                                xmlWriter.WriteAttributeString("denumire", "Lucernă");
                                break;
                            case "17":
                                xmlWriter.WriteAttributeString("denumire", "Alte furaje perene");
                                break;
                            case "18":
                                xmlWriter.WriteAttributeString("denumire", "Plante pentru producerea de semințe şi seminceri");
                                break;
                            case "19":
                                xmlWriter.WriteAttributeString("denumire", "Alte culturi în teren arabil");
                                break;
                            case "20":
                                xmlWriter.WriteAttributeString("denumire", "Păşuni naturale");
                                break;
                            case "21":
                                xmlWriter.WriteAttributeString("denumire", "Fânețe naturale");
                                break;
                            case "22":
                                xmlWriter.WriteAttributeString("denumire", "Vii şi pepiniere viticole");
                                break;
                            case "23":
                                xmlWriter.WriteAttributeString("denumire", "Hameişti");
                                break;
                            case "24":
                                xmlWriter.WriteAttributeString("denumire", "Plantații şi pepiniere pomicole ");
                                break;
                            case "25":
                                xmlWriter.WriteAttributeString("denumire", "Arbuşti fructiferi");
                                break;
                            case "26":
                                xmlWriter.WriteAttributeString("denumire", "Total teren agricol cod (01+20+...+25)");
                                break;
                        }
                        xmlWriter.WriteStartElement("nrARI");               //deschid7
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(drXML["inloc"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid7
                        xmlWriter.WriteStartElement("nrHA");               //deschid7
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(drXML["inloc"].ToString()));
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