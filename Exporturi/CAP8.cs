using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP8
    {
        public static bool make_CAP8xml(string strIdRol, int Trimestruparam)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP8\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP8.sem1, CAP8.sem2 FROM CAP8 LEFT JOIN (SELECT * FROM NOMCAP8) AS ROL ON CAP8.NrCrt = ROL.NrCrt WHERE CAP8.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP8\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_8");         //deschid5
                if(Trimestruparam==1){
                    xmlWriter.WriteAttributeString("semestru", "1");
                }else if(Trimestruparam==2){
                    xmlWriter.WriteAttributeString("semestru", "2");
                }
                
                xmlWriter.WriteAttributeString("codCapitol", "CAP8");
                xmlWriter.WriteAttributeString("denumire", "Evoluția efectivelor de animale, în cursul anului, aflate în proprietate");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                        xmlWriter.WriteStartElement("categorie_animale_evolutie_efectiva");         //deschid6
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
                                xmlWriter.WriteAttributeString("denumire", "Bovine total existente la începutul semestrului");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "Viței obținuți");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Animale moarte");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "Bovine total existente la sfârșitul semestrului cod (01+02+03+04-05-06-07-08)");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "din cod 01: Vaci și bivolițe existente la începutul semestrului");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "15":
                                xmlWriter.WriteAttributeString("denumire", "Animale moarte");
                                break;
                            case "16":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "17":
                                xmlWriter.WriteAttributeString("denumire", "Vaci și bivolițe existente la sfârșitul semestrului cod (10+11+12-13-14-15-16)");
                                break;
                            case "18":
                                xmlWriter.WriteAttributeString("denumire", "Ovine existente la începutul semestrului");
                                break;
                            case "19":
                                xmlWriter.WriteAttributeString("denumire", "Miei obținuți");
                                break;
                            case "20":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "21":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "22":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "23":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "24":
                                xmlWriter.WriteAttributeString("denumire", "Animale moarte");
                                break;
                            case "25":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "26":
                                xmlWriter.WriteAttributeString("denumire", "Ovine existente la sfârșitul semestrului cod (18+19+20+21-22-23-24-25)");
                                break;
                            case "27":
                                xmlWriter.WriteAttributeString("denumire", "Caprine existente la începutul semestrului");
                                break;
                            case "28":
                                xmlWriter.WriteAttributeString("denumire", "Iezi obținuți");
                                break;
                            case "29":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "30":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "31":
                                xmlWriter.WriteAttributeString("denumire", "Vănzări");
                                break;
                            case "32":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "33":
                                xmlWriter.WriteAttributeString("denumire", "Animale moarte");
                                break;
                            case "34":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "35":
                                xmlWriter.WriteAttributeString("denumire", "Caprine existente la sfârșitul semestrului cod (27+28+29+30-31-32-33-34)");
                                break;
                            case "36":
                                xmlWriter.WriteAttributeString("denumire", "Porcine existente la începutul semestrului");
                                break;
                            case "37":
                                xmlWriter.WriteAttributeString("denumire", "Purcei obținuți");
                                break;
                            case "38":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "39":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "40":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "41":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "42":
                                xmlWriter.WriteAttributeString("denumire", "Animale moarte");
                                break;
                            case "43":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "44":
                                xmlWriter.WriteAttributeString("denumire", "Porcine existente la sfârșitul semestrului cod (36+37+38+39-40-41-42-43)");
                                break;
                            case "45":
                                xmlWriter.WriteAttributeString("denumire", "Cabaline existente la începutul semestrului");
                                break;
                            case "46":
                                xmlWriter.WriteAttributeString("denumire", "Mânji obtinuți");
                                break;
                            case "47":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "48":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "49":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "50":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "51":
                                xmlWriter.WriteAttributeString("denumire", "Animale moarte");
                                break;
                            case "52":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "53":
                                xmlWriter.WriteAttributeString("denumire", "Cabaline existente la sfârșitul semestrului cod (45+46+47+48-49-50-51-52)");
                                break;
                            case "54":
                                xmlWriter.WriteAttributeString("denumire", "Păsări existente la începutul semestrului");
                                break;
                            case "55":
                                xmlWriter.WriteAttributeString("denumire", "Pui obținuți");
                                break;
                            case "56":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "57":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "58":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "59":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "60":
                                xmlWriter.WriteAttributeString("denumire", "Păsări moarte");
                                break;
                            case "61":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "62":
                                xmlWriter.WriteAttributeString("denumire", "Păsări existente la sfârșitul semestrului cod (54+55+56+57-58-59-60-61)");
                                break;
                            case "63":
                                xmlWriter.WriteAttributeString("denumire", "Alte animale domestice și/sau sălbatice crescute în captivitate, în condițiile legii, ce fac obiectul înscrierii în registrul agricol existente la începutul semestrului");
                                break;
                            case "64":
                                xmlWriter.WriteAttributeString("denumire", "Alte animale obținute");
                                break;
                            case "65":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "66":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "67":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "68":
                                xmlWriter.WriteAttributeString("denumire", "Tăieri");
                                break;
                            case "69":
                                xmlWriter.WriteAttributeString("denumire", "Animale moarte");
                                break;
                            case "70":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "71":
                                xmlWriter.WriteAttributeString("denumire", "Alte animale domestice și/sau sălbatice crescute în captivitate, în condițiile legii, ce fac obiectul înscrierii în registrul agricol, existente la sfârșitul semestrului cod (63+64+65+66-67-68-69-70)");
                                break;
                            case "72":
                                xmlWriter.WriteAttributeString("denumire", "Familii de albine existente la începutul semestrului");
                                break;
                            case "73":
                                xmlWriter.WriteAttributeString("denumire", "Familii noi de albine obținute");
                                break;
                            case "74":
                                xmlWriter.WriteAttributeString("denumire", "Cumpărări");
                                break;
                            case "75":
                                xmlWriter.WriteAttributeString("denumire", "Alte intrări");
                                break;
                            case "76":
                                xmlWriter.WriteAttributeString("denumire", "Vânzări");
                                break;
                            case "77":
                                xmlWriter.WriteAttributeString("denumire", "Familiide albine moarte");
                                break;
                            case "78":
                                xmlWriter.WriteAttributeString("denumire", "Alte ieșiri");
                                break;
                            case "79":
                                xmlWriter.WriteAttributeString("denumire", "Familii de albine existente la sfârșitul semestrului (cod 72+73+74+75-76-77-78)");
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                        xmlWriter.WriteStartElement("nrCapete");               //deschid7
                        if(Trimestruparam==1){
                            xmlWriter.WriteAttributeString("value", drXML["sem1"].ToString());
                        }else if(Trimestruparam==2){
                            xmlWriter.WriteAttributeString("value", drXML["sem2"].ToString());
                        }
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