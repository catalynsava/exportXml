using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP2a
    {
        public static bool make_CAP2axml(string strIdRol)
        {
            decimal TempDecimal=0;
            string strGosp = strIdRol;
            strGosp = strGosp.Substring(0, strIdRol.Length - 3);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2a\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
            strSQL = "SELECT ROL.nrcrt, CAP2.inloc, CAP2.altloc, CAP2.tot FROM CAP2 LEFT JOIN (SELECT * FROM NOMCAP2) AS ROL ON CAP2.NrCrt = ROL.NrCrt WHERE CAP2.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";
            //Console.WriteLine(strSQL);

            OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drXML = cmdXML.ExecuteReader();




            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;


            XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2a\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
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

            xmlWriter.WriteStartElement("capitol_2a");         //deschid5
            xmlWriter.WriteAttributeString("codCapitol", "CAP2a");
            xmlWriter.WriteAttributeString("denumire", "Terenuri aflate în proprietate");

            while (drXML.Read())
            {
                if ( drXML["nrcrt"].ToString()!="19" && drXML["nrcrt"].ToString()!="20" )
                {
                    xmlWriter.WriteStartElement("categorie_teren");         //deschid6
                    xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                    xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                    switch (drXML["nrcrt"].ToString())
                    {
                        case "1":
                            xmlWriter.WriteAttributeString("denumire", "Teren arabil(inclusiv sere și solarii)");
                            break;
                        case "2":
                            xmlWriter.WriteAttributeString("denumire", "Pășuni naturale");
                            break;
                        case "3":
                            xmlWriter.WriteAttributeString("denumire", "Fânețe naturale");
                            break;
                        case "4":
                            xmlWriter.WriteAttributeString("denumire", "Vii, pepiniere viticole și hameiști");
                            break;
                        case "5":
                            xmlWriter.WriteAttributeString("denumire", "- vii pe rod");
                            break;
                        case "6":
                            xmlWriter.WriteAttributeString("denumire", "- hameiști (total)");
                            break;
                        case "7":
                            xmlWriter.WriteAttributeString("denumire", "Livezi de pomi, pepiniere pomicole, arbuști fructiferi");
                            break;
                        case "8":
                            xmlWriter.WriteAttributeString("denumire", "- livezi pe rod");
                            break;
                        case "9":
                            xmlWriter.WriteAttributeString("denumire", "Grădini familiale");
                            break;
                        case "10":
                            xmlWriter.WriteAttributeString("denumire", "Teren agricol - total cod (01+02+03 + 04+07+09)");
                            break;
                        case "11":
                            xmlWriter.WriteAttributeString("denumire", "Păduri și alte terenuri cu vegetație forestieră din care:");
                            break;
                        case "12":
                            xmlWriter.WriteAttributeString("denumire", "păduri");
                            break;
                        case "13":
                            xmlWriter.WriteAttributeString("denumire", "Drumuri și căi ferate");
                            break;
                        case "14":
                            xmlWriter.WriteAttributeString("denumire", "Construcții");
                            break;
                        case "15":
                            xmlWriter.WriteAttributeString("denumire", "Terenuri degradate și neproductive");
                            break;
                        case "16":
                            xmlWriter.WriteAttributeString("denumire", "Ape și bălți");
                            break;
                        case "17":
                            xmlWriter.WriteAttributeString("denumire", "Teren neagricol total - cod (11+13+14+15+16)");
                            break;
                        case "18":
                            xmlWriter.WriteAttributeString("denumire", "Suprafață totală - cod (10+17)");
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }

                    TempDecimal=Convert.ToDecimal(drXML["altloc"].ToString());
                    xmlWriter.WriteStartElement("altelocARI");              //deschid7
                    xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(TempDecimal.ToString()));
                    xmlWriter.WriteEndElement();                            //inchid7
                    xmlWriter.WriteStartElement("altelocHA");               //deschid7
                    xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(TempDecimal.ToString()));
                    xmlWriter.WriteEndElement();                            //inchid7
                    TempDecimal=0;

                    TempDecimal=Convert.ToDecimal(drXML["inloc"].ToString());
                    xmlWriter.WriteStartElement("localARI");               //deschid7
                    xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(TempDecimal.ToString()));
                    xmlWriter.WriteEndElement();                            //inchid7
                    xmlWriter.WriteStartElement("localHA");               //deschid7
                    xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(TempDecimal.ToString()));
                    xmlWriter.WriteEndElement();                            //inchid7
                    TempDecimal=0;

                    TempDecimal=Convert.ToDecimal(drXML["tot"].ToString());
                    xmlWriter.WriteStartElement("totalARI");               //deschid7
                    xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(TempDecimal.ToString()));
                    xmlWriter.WriteEndElement();                            //inchid7
                    xmlWriter.WriteStartElement("totalHA");               //deschid7
                    xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(TempDecimal.ToString()));
                    xmlWriter.WriteEndElement();                            //inchid7
                    TempDecimal=0;
                    xmlWriter.WriteEndElement();
                }
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
    }
}