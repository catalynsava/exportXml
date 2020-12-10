using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP7
    {
        public static bool make_CAP7xml(string strIdRol, int Trimestruparam)
        {
            string FolderTrimestru="XML\\CAP7_" + Trimestruparam + "\\";
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() +FolderTrimestru + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT ROL.nrcrt, CAP7.buc, CAP7.buc2 FROM CAP7 LEFT JOIN (SELECT * FROM NOMCAP7) AS ROL ON CAP7.NrCrt = ROL.NrCrt WHERE CAP7.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //scriu xml
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() +FolderTrimestru + AjutExport.numefisier(strIdRol) + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_7");         //deschid5
                if(Trimestruparam==1){
                    xmlWriter.WriteAttributeString("semestru", "1");
                }else if(Trimestruparam==2){
                    xmlWriter.WriteAttributeString("semestru", "2");
                }
                
                xmlWriter.WriteAttributeString("codCapitol", "CAP7");
                xmlWriter.WriteAttributeString("denumire", "Animale domestice și/sau animale sălbatice crescute în captivitate, în condițiile legii - Situația la începutul semestrului");
                //-----------------------------------------------------------
                //parcurg baza si scriu xml
                while (drXML.Read())
                {
                        xmlWriter.WriteStartElement("categorie_animale");         //deschid6
                        xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                        xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                        switch (drXML["nrcrt"].ToString())
                        {
                            case "1":
                                xmlWriter.WriteAttributeString("denumire", "Bovine-total cod (02+08+13+26)");
                                break;
                            case "2":
                                xmlWriter.WriteAttributeString("denumire", "Bovine sub 1 an - total cod (03+05)");
                                break;
                            case "3":
                                xmlWriter.WriteAttributeString("denumire", "- Bovine femele sub 1 an");
                                break;
                            case "4":
                                xmlWriter.WriteAttributeString("denumire", "din care: sub 6 luni");
                                break;
                            case "5":
                                xmlWriter.WriteAttributeString("denumire", "- Bovine masculi sub 1 an");
                                break;
                            case "6":
                                xmlWriter.WriteAttributeString("denumire", "din care: sub 6 luni");
                                break;
                            case "7":
                                xmlWriter.WriteAttributeString("denumire", "Din 02: Viței pentru sacrificare");
                                break;
                            case "8":
                                xmlWriter.WriteAttributeString("denumire", "Bovine de 1-2 ani – total cod (09+12)");
                                break;
                            case "9":
                                xmlWriter.WriteAttributeString("denumire", "- Bovine femele de 1-2 ani cod (10+11)");
                                break;
                            case "10":
                                xmlWriter.WriteAttributeString("denumire", "- pentru sacrificare");
                                break;
                            case "11":
                                xmlWriter.WriteAttributeString("denumire", "- altele");
                                break;
                            case "12":
                                xmlWriter.WriteAttributeString("denumire", "- Bovine masculi de 1-2 ani");
                                break;
                            case "13":
                                xmlWriter.WriteAttributeString("denumire", "Bovine de 2 ani și peste - total cod (14+19)");
                                break;
                            case "14":
                                xmlWriter.WriteAttributeString("denumire", "- Bovine masculi de 2 ani și peste cod (15+17+18)");
                                break;
                            case "15":
                                xmlWriter.WriteAttributeString("denumire", "- pentru reproductie");
                                break;
                            case "16":
                                xmlWriter.WriteAttributeString("denumire", "din care: Tauri reproducători autorizați");
                                break;
                            case "17":
                                xmlWriter.WriteAttributeString("denumire", "- reformați (pentru sacrificare)");
                                break;
                            case "18":
                                xmlWriter.WriteAttributeString("denumire", "- pentru muncă");
                                break;
                            case "19":
                                xmlWriter.WriteAttributeString("denumire", "- Femele de 2 ani și peste cod (20+23)");
                                break;
                            case "20":
                                xmlWriter.WriteAttributeString("denumire", "- Juninci - cod(21+22)");
                                break;
                            case "21":
                                xmlWriter.WriteAttributeString("denumire", "- pentru sacrificare");
                                break;
                            case "22":
                                xmlWriter.WriteAttributeString("denumire", "- altele");
                                break;
                            case "23":
                                xmlWriter.WriteAttributeString("denumire", "- Vaci cod (24+25)");
                                break;
                            case "24":
                                xmlWriter.WriteAttributeString("denumire", "- vaci pentru lapte");
                                break;
                            case "25":
                                xmlWriter.WriteAttributeString("denumire", "- vaci reformate (pentru sacrificare)");
                                break;
                            case "26":
                                xmlWriter.WriteAttributeString("denumire", "- Bubaline (27+28+29)");
                                break;
                            case "27":
                                xmlWriter.WriteAttributeString("denumire", "- Bivolițe pentru reproducție");
                                break;
                            case "28":
                                xmlWriter.WriteAttributeString("denumire", "- Bivoli autorizați pentru reproducție");
                                break;
                            case "29":
                                xmlWriter.WriteAttributeString("denumire", "- Alte bubaline");
                                break;
                            case "30":
                                xmlWriter.WriteAttributeString("denumire", "Ovine - total cod (31+...+35)");
                                break;
                            case "31":
                                xmlWriter.WriteAttributeString("denumire", "- oi-femele pentru reproducție");
                                break;
                            case "32":
                                xmlWriter.WriteAttributeString("denumire", "- mioare montate");
                                break;
                            case "33":
                                xmlWriter.WriteAttributeString("denumire", "- tineret sub 1 an");
                                break;
                            case "34":
                                xmlWriter.WriteAttributeString("denumire", "- oi-alte categorii");
                                break;
                            case "35":
                                xmlWriter.WriteAttributeString("denumire", "- berbeci pentru reproducție autorizați");
                                break;
                            case "36":
                                xmlWriter.WriteAttributeString("denumire", "Caprine-total cod (37+...+40)");
                                break;
                            case "37":
                                xmlWriter.WriteAttributeString("denumire", "- capre");
                                break;
                            case "38":
                                xmlWriter.WriteAttributeString("denumire", "- țapi");
                                break;
                            case "39":
                                xmlWriter.WriteAttributeString("denumire", "- tineret montat");
                                break;
                            case "40":
                                xmlWriter.WriteAttributeString("denumire", "- alte caprine");
                                break;
                            case "41":
                                xmlWriter.WriteAttributeString("denumire", "Porcine-total cod (42+43+45+49)");
                                break;
                            case "42":
                                xmlWriter.WriteAttributeString("denumire", "Purcei sub 20 kg");
                                break;
                            case "43":
                                xmlWriter.WriteAttributeString("denumire", "Porcine între 20-50 kg");
                                break;
                            case "44":
                                xmlWriter.WriteAttributeString("denumire", "din care: tineret de crescătorii (2-4 luni)");
                                break;
                            case "45":
                                xmlWriter.WriteAttributeString("denumire", "Porcine pentru îngrășat-total cod (46+47+48)");
                                break;
                            case "46":
                                xmlWriter.WriteAttributeString("denumire", "- cu greutate de 50-80 kg");
                                break;
                            case "47":
                                xmlWriter.WriteAttributeString("denumire", "- cu greutate de 81-110 kg");
                                break;
                            case "48":
                                xmlWriter.WriteAttributeString("denumire", "- cu greutate de peste 110 kg");
                                break;
                            case "49":
                                xmlWriter.WriteAttributeString("denumire", "Porcine pentru reproductie de peste 50 kg-total (50+51+53)");
                                break;
                            case "50":
                                xmlWriter.WriteAttributeString("denumire", "- vieri de reproducție autorizați");
                                break;
                            case "51":
                                xmlWriter.WriteAttributeString("denumire", "- scroafe montate");
                                break;
                            case "52":
                                xmlWriter.WriteAttributeString("denumire", "din care scroafe la prima montă");
                                break;
                            case "53":
                                xmlWriter.WriteAttributeString("denumire", "- scroafe nemontate");
                                break;
                            case "54":
                                xmlWriter.WriteAttributeString("denumire", "din care scrofițe");
                                break;
                            case "55":
                                xmlWriter.WriteAttributeString("denumire", "din acestea: tineret femel de prăsilă (6-9 luni)");
                                break;
                            case "56":
                                xmlWriter.WriteAttributeString("denumire", "Cabaline-total din care:");
                                break;
                            case "57":
                                xmlWriter.WriteAttributeString("denumire", "- cabaline de muncă peste 3 ani");
                                break;
                            case "58":
                                xmlWriter.WriteAttributeString("denumire", "- armăsari pentru reproducție autorizați");
                                break;
                            case "59":
                                xmlWriter.WriteAttributeString("denumire", "Măgari");
                                break;
                            case "60":
                                xmlWriter.WriteAttributeString("denumire", "Catâri");
                                break;
                            case "61":
                                xmlWriter.WriteAttributeString("denumire", "Iepuri total din care:");
                                break;
                            case "62":
                                xmlWriter.WriteAttributeString("denumire", "- iepuri femele pentru reproducție");
                                break;
                            case "63":
                                xmlWriter.WriteAttributeString("denumire", "Animale de blană - total cod(64+...+67)");
                                break;
                            case "64":
                                xmlWriter.WriteAttributeString("denumire", "- vulpi");
                                break;
                            case "65":
                                xmlWriter.WriteAttributeString("denumire", "- nurci");
                                break;
                            case "66":
                                xmlWriter.WriteAttributeString("denumire", "- nutrii");
                                break;
                            case "67":
                                xmlWriter.WriteAttributeString("denumire", "- Alte animale de blană");
                                break;
                            case "68":
                                xmlWriter.WriteAttributeString("denumire", "Păsări total cod(69+...+74)");
                                break;
                            case "69":
                                xmlWriter.WriteAttributeString("denumire", "- galinacee");
                                break;
                            case "70":
                                xmlWriter.WriteAttributeString("denumire", "- curci");
                                break;
                            case "71":
                                xmlWriter.WriteAttributeString("denumire", "- rațe");
                                break;
                            case "72":
                                xmlWriter.WriteAttributeString("denumire", "- gâște");
                                break;
                            case "73":
                                xmlWriter.WriteAttributeString("denumire", "- struți");
                                break;
                            case "74":
                                xmlWriter.WriteAttributeString("denumire", "- alte păsări");
                                break;
                            case "75":
                                xmlWriter.WriteAttributeString("denumire", "Din cod 68 (păsări total) - total păsări adulte ouătoare");
                                break;
                            case "76":
                                xmlWriter.WriteAttributeString("denumire", "din care: găini ouătoare");
                                break;
                            case "77":
                                xmlWriter.WriteAttributeString("denumire", "Alte animale domestice și/sau sălbatice crescute în captivitate, în condițiile legii, ce fac obiectul înscrierii în registrul agricol");
                                break;
                            case "78":
                                xmlWriter.WriteAttributeString("denumire", "Familii de albine");
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                        xmlWriter.WriteStartElement("nrCapete");               //deschid7
                        if(Trimestruparam==1){
                            xmlWriter.WriteAttributeString("value", drXML["buc"].ToString());
                        }else if(Trimestruparam==2){
                            xmlWriter.WriteAttributeString("value", drXML["buc2"].ToString());
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