using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP11
    {
        public static bool make_CAP11xml(string strIdRol)
        {
            string dacanuaman="1600";
            string strdestinatie="";
            string codTipCladire="";
            int suprafata = 0;

            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP11\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT * FROM CAP11 WHERE IDROL=\"" + strIdRol + "\" ORDER BY rand;";
                OleDbCommand cmdXML = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune );
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                //--

                //--
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP11\\" + strGosp + "xml", settings);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("DOCUMENT_RAN");        //deschid1
                xmlWriter.WriteStartElement("HEADER");              //deschid2 HEADER
                xmlWriter.WriteStartElement("codXml");              //deschid3
                xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
                xmlWriter.WriteEndElement();                        //inchid3
                xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());
                xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");
                xmlWriter.WriteElementString("sirutaUAT", datgenSirute.SirutaSuperioara);
                xmlWriter.WriteEndElement();                      //inchid2 HEADER

                xmlWriter.WriteStartElement("BODY");              //deschid2 BODY
                xmlWriter.WriteStartElement("gospodarie");        //deschid3 gospodarie
                xmlWriter.WriteAttributeString("identificator",  strGosp);
                xmlWriter.WriteStartElement("anRaportare");       //deschid4 anRaportare
                xmlWriter.WriteAttributeString("an", "2020");
                xmlWriter.WriteStartElement("capitol_11");        //deschid5 capitol_11
                xmlWriter.WriteAttributeString("codCapitol", "CAP11");
                xmlWriter.WriteAttributeString("denumire", "Clădiri existente la începutul anului pe raza localității");

                while (drXML.Read())
                {
                    Sirute cladireSirute=new Sirute(drXML["sat"].ToString(), drXML["judet"].ToString());
                   
                    xmlWriter.WriteStartElement("cladire");            //deschid6 cladire
                    xmlWriter.WriteStartElement("adresa");             //deschid7 adresa
                    xmlWriter.WriteElementString("apartament", drXML["ap"].ToString());
                    xmlWriter.WriteElementString("bloc", drXML["bloc"].ToString());
                    xmlWriter.WriteElementString("etaj", "");
                    xmlWriter.WriteElementString("numar", drXML["nr"].ToString());
                    xmlWriter.WriteElementString("scara", drXML["scara"].ToString());

                    
                    xmlWriter.WriteElementString("sirutaJudet", cladireSirute.SirutaJudet);
                    xmlWriter.WriteElementString("sirutaLocalitate", cladireSirute.Siruta);
                    xmlWriter.WriteElementString("sirutaUAT", cladireSirute.SirutaSuperioara);
                    xmlWriter.WriteElementString("strada", drXML["strada"].ToString());
                    xmlWriter.WriteEndElement();                       //inchid7  adresa
                    

                    if (drXML["an"].ToString()!= "")
                    {
                        xmlWriter.WriteElementString("anulTerminarii", drXML["an"].ToString());
                        dacanuaman = drXML["an"].ToString();
                    }
                    else
                    {
                        xmlWriter.WriteElementString("anulTerminarii", dacanuaman);
                    }
                    

                    switch(drXML["destinatie"].ToString()){
                        case "a. locuinta":
                            strdestinatie="a";
                            break;
                        case "b. locuinta convenabila":
                            strdestinatie="b";
                            break;
                        case "c. locuinta sociala":
                            strdestinatie="c";
                            break;
                        case "d. locuinta de serviciu":
                            strdestinatie="d";
                            break;
                        case "e. locuinta de interventie":
                            strdestinatie="e";
                            break;
                        case "f. locuinta de necesitate":
                            strdestinatie="f";
                            break;
                        case "g. locuinta de protocol":
                            strdestinatie="g";
                            break;
                        case "h. casa de vacanta":
                            strdestinatie="h";
                            break;
                        case "i. grajd":
                            strdestinatie="i";
                            break;
                        case "j. patul":
                            strdestinatie="j";
                            break;
                        case "k. magazie/hambar pentru cereale":
                            strdestinatie="k";
                            break;
                        case "l. sura/fanarie":
                            strdestinatie="l";
                            break;
                        case "m. remiza/sopron":
                            strdestinatie="m";
                            break;
                        case "n. garaj":
                            strdestinatie="n";
                            break;
                        case "o. cladire sociala, de invatamant, de sanatate, de cultura, de administratie":
                            strdestinatie="o";
                            break;
                        case "p. cladire industriala/socio-economica":
                            strdestinatie = "p";
                            break;
                        case "q. alte cladiri neprevazute la lit. a. - p.":
                            strdestinatie = "q";
                            break;
                        default:
                            strdestinatie = "a";
                            break;
                    }
                    xmlWriter.WriteElementString("codDestinatieCladire", strdestinatie);


                    codTipCladire=AjutExport.getCodCladire(drXML["anexa"].ToString(), drXML["material"].ToString(), drXML["electric"].ToString());


                    xmlWriter.WriteElementString("codTipCladire", codTipCladire);

                    if (drXML["rand"].ToString()!="")
                    {
                        xmlWriter.WriteElementString("identificator", drXML["rand"].ToString());
                    }
                    else
                    {
                        xmlWriter.WriteElementString("identificator", "9");
                    }


                    xmlWriter.WriteElementString("identificatorCadastralElectronic", "");
                    xmlWriter.WriteStartElement("suprafataConstruitaDesfasurataMP");//deschid 7 suprafataConstruitaDesfasurataMP
                    suprafata = Convert.ToInt32(drXML["supr"]);
                    xmlWriter.WriteAttributeString("value", suprafata.ToString());
                    xmlWriter.WriteEndElement();                       //inchid7  suprafataConstruitaDesfasurataMP
                    xmlWriter.WriteStartElement("suprafataConstruitaLaSolMP");//deschid 7 suprafataConstruitaLaSolMP
                    xmlWriter.WriteAttributeString("value", suprafata.ToString());
                    xmlWriter.WriteEndElement();                       //inchid7  suprafataConstruitaLaSolMP
                    if (drXML["zona"].ToString() == "")
                    {
                        xmlWriter.WriteElementString("zona", "Z");
                    }
                    else
                    {
                        xmlWriter.WriteElementString("zona", drXML["zona"].ToString());
                    }
                    
                    xmlWriter.WriteEndElement();                       //inchid6  cladire
                }
                xmlWriter.WriteEndElement();                      //inchid5  capitol_11
                xmlWriter.WriteEndElement();                      //inchid4  anRaportare

                xmlWriter.WriteEndElement();                      //inchid3  gospodarie

                xmlWriter.WriteEndElement();                      //inchid2  BODY

                xmlWriter.WriteEndElement();                      //inchid1 DOCUMENT_RAN

                xmlWriter.Close();
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