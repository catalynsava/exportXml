using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP9
    {
        public static bool make_CAP9xml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP9\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP9.buc FROM CAP9 LEFT JOIN (SELECT * FROM NOMCAP9) AS ROL ON CAP9.NrCrt = ROL.NrCrt WHERE CAP9.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP9\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_9");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP9");
                xmlWriter.WriteAttributeString("denumire", "Utilaje, instalații pentru agricultură, mijloace de transport cu tracțiune animală și mecanică existente la începutul anului");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                    
                        xmlWriter.WriteStartElement("sistem_tehnic_agricol");         //denumire generica rand
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                             case "1":
                                xmlWriter.WriteAttributeString("denumire", "Tractoare până la 45 CP");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "Tractoare între 46-65 CP");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "Tractoare între 66-100 CP");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "Tractoare între 101-140 CP");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "Tractoare între 141-200 CP");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "Tractoare între 201-280 CP");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Tractoare peste 280 CP");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Motocultoare");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "Motocositoare");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "Pluguri pentru tractor cu două trupițe");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "Pluguri pentru tractor cu trei trupițe");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "Pluguri pentru tractor cu mai mult de trei trupițe");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Pluguri cu tracțiune animală");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "Cultivatoare");
                                break;
                            case "15":
                                xmlWriter.WriteAttributeString("denumire", "Grape cu tracțiune mecanică (toate tipurile)");
                                break;
                            case "16":
                                xmlWriter.WriteAttributeString("denumire", "Grape cu tracțiune animală");
                                break;
                            case "17":
                                xmlWriter.WriteAttributeString("denumire", "Combinatoare");
                                break;
                            case "18":
                                xmlWriter.WriteAttributeString("denumire", "Semănători cu tracțiune mecanică pentru păioase – simple");
                                break;
                            case "19":
                                xmlWriter.WriteAttributeString("denumire", "Semănători cu tracțiune mecanică pentru păioase – multifuncționale");
                                break;
                            case "20":
                                xmlWriter.WriteAttributeString("denumire", "Semănători cu tracțiune mecanică pentru păioase - semănat în teren nelucrat");
                                break;
                            case "21":
                                xmlWriter.WriteAttributeString("denumire", "Semănători cu tracțiune mecanică pentru prășitoare – simple");
                                break;
                            case "22":
                                xmlWriter.WriteAttributeString("denumire", "Semănători cu tracțiune mecanică pentru prășitoare - multifuncționale");
                                break;
                            case "23":
                                xmlWriter.WriteAttributeString("denumire", "Semănători cu tracțiune mecanică pentru prășitoare - semănat în teren nelucrat");
                                break;
                            case "24":
                                xmlWriter.WriteAttributeString("denumire", "Semănători cu tracțiune animală");
                                break;
                            case "25":
                                xmlWriter.WriteAttributeString("denumire", "Mașini pentru plantat cartofi");
                                break;
                            case "26":
                                xmlWriter.WriteAttributeString("denumire", "Mașini pentru plantat răsaduri");
                                break;
                            case "27":
                                xmlWriter.WriteAttributeString("denumire", "Mașini pentru împrăștiat îngrășăminte chimice");
                                break;
                            case "28":
                                xmlWriter.WriteAttributeString("denumire", "Mașini pentru împrăștiat îngrășăminte organice");
                                break;
                            case "29":
                                xmlWriter.WriteAttributeString("denumire", "Mașini de stropit și prăfuit cu tracțiune mecanică – purtate");
                                break;
                            case "30":
                                xmlWriter.WriteAttributeString("denumire", "Mașini de stropit și prăfuit cu tracțiune mecanică – tractate");
                                break;
                            case "31":
                                xmlWriter.WriteAttributeString("denumire", "Mașini pentru erbicidat");
                                break;
                            case "32":
                                xmlWriter.WriteAttributeString("denumire", "Combine autopropulsate pentru recoltat cereale păioase");
                                break;
                            case "33":
                                xmlWriter.WriteAttributeString("denumire", "Combine autopropulsate pentru recoltat porumb");
                                break;
                            case "34":
                                xmlWriter.WriteAttributeString("denumire", "Combine autopropulsate pentru recoltat furaje");
                                break;
                            case "35":
                                xmlWriter.WriteAttributeString("denumire", "Batoze pentru cereale păioase");
                                break;
                            case "36":
                                xmlWriter.WriteAttributeString("denumire", "Combine pentru recoltat sfeclă de zahăr");
                                break;
                            case "37":
                                xmlWriter.WriteAttributeString("denumire", "Dislocatoare pentru sfeclă de zahăr");
                                break;
                            case "38":
                                xmlWriter.WriteAttributeString("denumire", "Mașini de decoletat sfeclă de zahăr");
                                break;
                            case "39":
                                xmlWriter.WriteAttributeString("denumire", "Combine și mașini pentru recoltat cartofi");
                                break;
                            case "40":
                                xmlWriter.WriteAttributeString("denumire", "Cositori cu tracțiune mecanică");
                                break;
                            case "41":
                                xmlWriter.WriteAttributeString("denumire", "Vindrovere autopropulsate pentru recoltat furaje");
                                break;
                            case "42":
                                xmlWriter.WriteAttributeString("denumire", "Prese pentru balotat paie și fân - baloți paralelipipedici");
                                break;
                            case "43":
                                xmlWriter.WriteAttributeString("denumire", "Prese pentru balotat paie și fân - baloți cilindrici");
                                break;
                            case "44":
                                xmlWriter.WriteAttributeString("denumire", "Remorci pentru tractor");
                                break;
                            case "45":
                                xmlWriter.WriteAttributeString("denumire", "Autovehicule pentru transport mărfuri cu capacitate până la 1,5 tone");
                                break;
                            case "46":
                                xmlWriter.WriteAttributeString("denumire", "Autovehicule pentru transport mărfuri cu capacitate peste 1,5 tone");
                                break;
                            case "47":
                                xmlWriter.WriteAttributeString("denumire", "Care și căruțe");
                                break;
                            case "48":
                                xmlWriter.WriteAttributeString("denumire", "Încărcătoare hidraulice");
                                break;
                            case "49":
                                xmlWriter.WriteAttributeString("denumire", "Motopompe pentru irigat");
                                break;
                            case "50":
                                xmlWriter.WriteAttributeString("denumire", "Aripi de ploaie cu deplasare manuală");
                                break;
                            case "51":
                                xmlWriter.WriteAttributeString("denumire", "Instalații de irigat autodeplasabile cu tambur și furtun");
                                break;
                            case "52":
                                xmlWriter.WriteAttributeString("denumire", "Instalații de irigat cu deplasare liniară");
                                break;
                            case "53":
                                xmlWriter.WriteAttributeString("denumire", "Instalații de irigat cu pivot");
                                break;
                            case "54":
                                xmlWriter.WriteAttributeString("denumire", "Instalații pentru muls mecanic");
                                break;
                            case "55":
                                xmlWriter.WriteAttributeString("denumire", "Instalații pentru prepararea furajelor");
                                break;
                            case "56":
                                xmlWriter.WriteAttributeString("denumire", "Instalații pentru evacuarea dejecțiilor");
                                break;
                            case "57":
                                xmlWriter.WriteAttributeString("denumire", "Instalații/cazan pentru fabricat țuică/rachiu");
                                break;
                            case "58":
                                xmlWriter.WriteAttributeString("denumire", "Alte utilaje, instalații pentru agricultură, mijloace de transport cu tracțiune animală și mecanică");
                                break;
                            case "59":
                                xmlWriter.WriteAttributeString("denumire", "Mori cu ciocănele");
                                break;
                            case "60":
                                xmlWriter.WriteAttributeString("denumire", "Mori cu valțuri");
                                break;
                            case "61":
                                xmlWriter.WriteAttributeString("denumire", "Alte mori");
                                break;
                        }
                        xmlWriter.WriteStartElement("nrBucati");               //deschid7
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