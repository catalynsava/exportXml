using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP10a
    {
        public static bool make_CAP10axml(string strIdRol)
        {
            try
            {
                int nrHAvar=0;
                int nrKGvar=0;
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10a\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP10a.sup, CAP10a.can FROM CAP10a LEFT JOIN (SELECT * FROM NOMCAP10a) AS ROL ON CAP10a.NrCrt = ROL.NrCrt WHERE CAP10a.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10a\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_10a");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP10a");
                xmlWriter.WriteAttributeString("denumire", "Aplicarea îngrășămintelor, amendamentelor și a pesticidelor pe suprafețe situate pe raza localității");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("substanta_chimica_agricola");         //denumire generica rand
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
		                        xmlWriter.WriteAttributeString("denumire","Îngrășăminte chimice – total (în echivalent substanță activă)");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire","a) Azotoase");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire","b) Fosfatice");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire","c) Potasice");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire","Îngrășăminte naturale");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire","Amendamente");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire","Insecticide (în echivalent substanță activă)");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire","Fungicide (în echivalent substanță activă)");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire","Erbicide (în echivalent substanță activă) - total, din care pentru:");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire","a) grâu");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire","b) porumb");
                                break;
                        }
                        
                        nrHAvar=Convert.ToInt32(Convert.ToDouble(drXML["sup"].ToString()));
                        nrKGvar=Convert.ToInt32(Convert.ToDouble(drXML["can"].ToString()));
                        xmlWriter.WriteStartElement("nrHA");               //deschid7
                        xmlWriter.WriteAttributeString("value",nrHAvar.ToString());
                        xmlWriter.WriteEndElement();                            //inchid7
                        xmlWriter.WriteStartElement("nrKG");               //deschid7
                        xmlWriter.WriteAttributeString("value", nrKGvar.ToString());
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