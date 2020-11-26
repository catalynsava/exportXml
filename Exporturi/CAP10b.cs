using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP10b
    {
        public static bool make_CAP10bxml(string strIdRol)
        {
            try
            {
                int nrHaAzotoaseVar=0;
                int nrKgAzotoaseVar=0;
                int nrHaFosfaticeVar=0;
                int nrKgFosfaticeVar=0;
                int nrHaPotasiceVar=0;
                int nrKgPotasiceVar=0;
                int nrHaTotalVar=0;
                int nrKgTotalVar=0;
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10b\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP10b.Asup, CAP10b.Acan, CAP10b.Fsup, CAP10b.Fcan, CAP10b.Psup, CAP10b.Pcan, CAP10b.Tsup, CAP10b.Tcan FROM CAP10b LEFT JOIN (SELECT * FROM NOMCAP10b) AS ROL ON CAP10b.NrCrt = ROL.NrCrt WHERE CAP10b.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10b\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_10b");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP10b");
                xmlWriter.WriteAttributeString("denumire", "Aplicarea îngrășămintelor, amendamentelor și a pesticidelor pe suprafețe situate pe raza localității");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("culturi_ingrasaminte_chimice");         //denumire generica rand
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
                                xmlWriter.WriteAttributeString("denumire", "Îngrășăminte chimice aplicate – total cod rând (02+15)");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "A. Culturi arabile total cod rând (03+…+14)");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "Grâu și secară");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "Orz+Orzoaică de toamnă");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "Orzoaică de primăvară");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "Porumb și sorg boabe");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Floarea-soarelui");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Rapiță");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "Soia boabe");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "Sfeclă de zahăr");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "Cartofi - total");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "Legume – total");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Plante de nutreț – total");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "Alte culturi în arabil");
                                break;
                            case "15":
                                xmlWriter.WriteAttributeString("denumire", "B. Culturi permanente total cod rând (16+…+20)");
                                break;
                            case "16":
                                xmlWriter.WriteAttributeString("denumire", "Pășuni naturale");
                                break;
                            case "17":
                                xmlWriter.WriteAttributeString("denumire", "Fânețe naturale");
                                break;
                            case "18":
                                xmlWriter.WriteAttributeString("denumire", "Vii și hameiști");
                                break;
                            case "19":
                                xmlWriter.WriteAttributeString("denumire", "Livezi");
                                break;
                            case "20":
                                xmlWriter.WriteAttributeString("denumire", "Alte culturi permanente");
                                break;
                        }
                        
                        nrHaAzotoaseVar=Convert.ToInt32(Convert.ToDouble(drXML["Asup"].ToString()));
                        nrHaFosfaticeVar=Convert.ToInt32(Convert.ToDouble(drXML["Fsup"].ToString()));
                        nrHaPotasiceVar=Convert.ToInt32(Convert.ToDouble(drXML["Psup"].ToString()));
                        nrHaTotalVar=nrHaAzotoaseVar+nrHaFosfaticeVar+nrHaPotasiceVar;


                        nrKgAzotoaseVar=Convert.ToInt32(Convert.ToDouble(drXML["Acan"].ToString()));
                        nrKgFosfaticeVar=Convert.ToInt32(Convert.ToDouble(drXML["Fcan"].ToString()));
                        nrKgPotasiceVar=Convert.ToInt32(Convert.ToDouble(drXML["Pcan"].ToString()));
                        nrKgTotalVar=nrKgAzotoaseVar+nrKgFosfaticeVar+nrKgPotasiceVar;
                        
                        xmlWriter.WriteStartElement("nrHAazotoase");         //nrHAazotoase 
                        xmlWriter.WriteAttributeString("value",nrHaAzotoaseVar.ToString());
                        xmlWriter.WriteEndElement();                        //nrHAazotoase 

                        xmlWriter.WriteStartElement("nrHAfosfatice");       //nrHAfosfatice 
                        xmlWriter.WriteAttributeString("value",nrHaFosfaticeVar.ToString());
                        xmlWriter.WriteEndElement();                        //nrHAfosfatice

                        xmlWriter.WriteStartElement("nrHApotasice");        //nrHApotasice  
                        xmlWriter.WriteAttributeString("value",nrHaPotasiceVar.ToString());
                        xmlWriter.WriteEndElement();                        //nrHApotasice 



                        xmlWriter.WriteStartElement("nrKGazotoase");        //nrKGazotoase
                        xmlWriter.WriteAttributeString("value", nrKgAzotoaseVar.ToString());
                        xmlWriter.WriteEndElement();                        //close nrKGazotoase

                        xmlWriter.WriteStartElement("nrKGfosfatice");        //nrKGfosfatice 
                        xmlWriter.WriteAttributeString("value", nrKgFosfaticeVar.ToString());
                        xmlWriter.WriteEndElement();                        //close nrKGfosfatice 

                        xmlWriter.WriteStartElement("nrKGpotasice");        //nrKGpotasice  
                        xmlWriter.WriteAttributeString("value", nrKgPotasiceVar.ToString());
                        xmlWriter.WriteEndElement();                        //close nrKGpotasice  


                        xmlWriter.WriteStartElement("totalHA");        //totalHA  
                        xmlWriter.WriteAttributeString("value",nrHaTotalVar.ToString());
                        xmlWriter.WriteEndElement();                   //close totalHA 

                        xmlWriter.WriteStartElement("totalKG");        //totalKG   
                        xmlWriter.WriteAttributeString("value", nrKgTotalVar.ToString());
                        xmlWriter.WriteEndElement();                   //close totalKG   
                        

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