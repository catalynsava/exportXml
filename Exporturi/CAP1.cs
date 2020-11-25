using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public static class CAP1
    {
        
        public static bool make_CAP1xml(string strIdRol)
        {
            string strGosp = strIdRol;
            strGosp = strGosp.Substring(0, strIdRol.Length - 3);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
            {
                Ajutatoare.scrielinie("eroriXML.log", " există deja: " + AjutExport.numefisier(strIdRol) + "xml");
                return false;
            }

            int codRand = 1;
            string strSQL = "SELECT * FROM datgen;";
            OleDbCommand cmdDateGenerale = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drDateGenerale = cmdDateGenerale.ExecuteReader();
            if (drDateGenerale.Read() == false)
            {
                return false;
            }

            Sirute dategenSirute=new Sirute(drDateGenerale["localitate"].ToString(), drDateGenerale["judet"].ToString());

            if (dategenSirute.Siruta == "" | dategenSirute.SirutaJudet == "" | dategenSirute.SirutaSuperioara == "")
            {
                Console.WriteLine(dategenSirute.SirutaJudet+ " " + dategenSirute.SirutaSuperioara + " " + dategenSirute.Siruta);
                return false;
            }

            //--------------------------------------------------------------------------
            strSQL = "SELECT * FROM CAP1 WHERE idrol=\"" + strIdRol + "\" ORDER BY rudenie;";
            OleDbCommand cmdEXP = new OleDbCommand(strSQL, BazaDeDate.conexiune );
            OleDbDataReader drEXP = cmdEXP.ExecuteReader();

            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;

            //MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory.ToString());
            XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1\\" + AjutExport.numefisier(strIdRol) + "xml", settings);


            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("DOCUMENT_RAN");    //deschid1
            xmlWriter.WriteStartElement("HEADER");          //deschid2

            xmlWriter.WriteStartElement("codXml");          //deschid3
            xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
            xmlWriter.WriteEndElement();                    //inchid3

            xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml()) ;

            xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");

            xmlWriter.WriteElementString("sirutaUAT", dategenSirute.SirutaSuperioara);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("BODY");
            xmlWriter.WriteStartElement("gospodarie");         //deschid3
            //xmlWriter.WriteAttributeString("identificator", strIdRol);
            xmlWriter.WriteAttributeString("identificator", strGosp);


            xmlWriter.WriteStartElement("capitol_1");          //deschid4
            xmlWriter.WriteAttributeString("codCapitol", "CAP1");
            xmlWriter.WriteAttributeString("denumire", "Componența gospodăriei/exploatației agricole fără personalitate juridică");

            
            while (drEXP.Read())
            {
                //daca apare de mai multe ori, nu-l pun niciodata?
                if(AjutExport.cnpmembri.Contains(drEXP["cnp"].ToString()) && drEXP["rudenie"].ToString()!="1"){
                    Console.WriteLine(drEXP["idrol"] + " " + drEXP["nume"] + " " + drEXP["prenume"] + " " + drEXP["cnp"] + " este prezent de mai multe ori.");
                    Ajutatoare.scrielinie("eroriXML.log", drEXP["idrol"] + " " + drEXP["nume"] + " " + drEXP["prenume"] + " " + drEXP["cnp"] + " este prezent de mai multe ori.");
                    continue;
                }else{
                    AjutExport.cnpmembri.Add(drEXP["cnp"].ToString());
                }
                
                /*if(CnpValidare.uncnpcatimembri(drEXP["cnp"].ToString())>1 && drEXP["rudenie"].ToString()!="1"){
                    Console.WriteLine(drEXP["idrol"] + " " + drEXP["nume"] + " " + drEXP["prenume"] + " " + drEXP["cnp"] + " este prezent de mai multe ori.");
                    Ajutatoare.scrielinie("eroriXML.log", drEXP["idrol"] + " " + drEXP["nume"] + " " + drEXP["prenume"] + " " + drEXP["cnp"] + " este prezent de mai multe ori.");
                    continue;
                }*/
                RaspunsValidare raspuns=CnpValidare.verificaCNP(drEXP["cnp"].ToString());
                if (raspuns.corect == true)
                {
                    xmlWriter.WriteStartElement("membru_gospodarie");   //deschid5
                    xmlWriter.WriteStartElement("dateMembruGospodarie");    //deschid6
                    xmlWriter.WriteElementString("nume", drEXP["nume"].ToString());
                    xmlWriter.WriteElementString("prenume", drEXP["prenume"].ToString());
                    if (drEXP["i"].ToString() == "")
                    {
                        xmlWriter.WriteElementString("initialaTata", "-");
                    }
                    else
                    {
                        xmlWriter.WriteElementString("initialaTata", drEXP["i"].ToString());

                    }

                    xmlWriter.WriteStartElement("cnp");                 //deschid7
                    xmlWriter.WriteAttributeString("value", drEXP["cnp"].ToString());
                    xmlWriter.WriteEndElement();                        //inchid7
                    xmlWriter.WriteEndElement();                        //inchid6

                    xmlWriter.WriteElementString("codLegaturaRudenie", drEXP["rudenie"].ToString());
                    xmlWriter.WriteElementString("codRand", codRand.ToString());
                    switch (drEXP["rudenie"].ToString())
                    {
                        case "1":
                            xmlWriter.WriteElementString("denumireLegaturaRudenie", "Cap de gospodărie");
                            break;
                        case "2":
                            xmlWriter.WriteElementString("denumireLegaturaRudenie", "Soț/Soție");
                            break;
                        case "3":
                            xmlWriter.WriteElementString("denumireLegaturaRudenie", "Fiu/Fiică");
                            break;
                        case "4":
                            xmlWriter.WriteElementString("denumireLegaturaRudenie", "Alte Rude");
                            break;
                        case "5":
                            xmlWriter.WriteElementString("denumireLegaturaRudenie", "Neînrudit");
                            break;
                    }
                    xmlWriter.WriteEndElement();                    //inchid5
                    codRand = codRand + 1;
                }
                else
                {
                    Console.WriteLine(drEXP["cnp"].ToString() + " eronat.");
                    Ajutatoare.scrielinie("eroriXML.log", drEXP["idrol"] + " " + drEXP["nume"] + " " + drEXP["prenume"] + " " + drEXP["cnp"] + "eronat.");
                }

            }

            xmlWriter.WriteEndElement();                    //inchid4
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            drEXP.Close();
            return true;
        }
    }
}