using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    internal class CAP2b
    {
        public static bool make_CAP2bxml(string strIdRol, string numeTitular, string initialaTitular, string prenumeTitular, string cnpTitular)
        {
          
            try{
                //###############################################################
                string strGosp = strIdRol;
                strGosp = strGosp.Substring(0, strIdRol.Length - 3);
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2b\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
                {
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

                strSQL = "SELECT * FROM CAP2B WHERE IDROL=\"" + strIdRol + "\";";
                int intCodRand = 1;

                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;


                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2b\\" + AjutExport.numefisier(strIdRol) + "xml", settings);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("DOCUMENT_RAN");        //deschid1

                xmlWriter.WriteStartElement("HEADER");              //deschid2
                xmlWriter.WriteStartElement("codXml");              //deschid3
                xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
                xmlWriter.WriteEndElement();                        //inchid3
                xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());
                xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");
                xmlWriter.WriteElementString("sirutaUAT",datgenSirute.SirutaSuperioara);
                xmlWriter.WriteEndElement();                        //inchid2
                xmlWriter.WriteStartElement("BODY");                //deschid2

                xmlWriter.WriteStartElement("gospodarie");         //deschid3
                xmlWriter.WriteAttributeString("identificator", strGosp);
                //xmlWriter.WriteAttributeString("identificator", strIdRol;

                xmlWriter.WriteStartElement("anRaportare");         //deschid4
                xmlWriter.WriteAttributeString("an", "2020");

                xmlWriter.WriteStartElement("capitol_2b");         //deschid5
                xmlWriter.WriteAttributeString("codCapitol", "CAP2b");
                xmlWriter.WriteAttributeString("denumire", "Identificarea pe parcele a terenurilor aflate în proprietate");

                while (drXML.Read())
                {
                    if (drXML["intra"].ToString() != "0")
                    {
                        xmlWriter.WriteStartElement("identificare_teren");         //deschid6
                        xmlWriter.WriteAttributeString("codRand", intCodRand.ToString());
                        if (drXML["tarlaua"].ToString().Trim() == drXML["parcela"].ToString().Trim())
                        {
                            xmlWriter.WriteAttributeString("denumire", drXML["parcela"].ToString());
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("denumire", drXML["tarlaua"].ToString() + " / " + drXML["parcela"].ToString());
                        }



                        xmlWriter.WriteStartElement("act_detinere");         //deschid7
                        //codTip act
                        if (drXML["tipact"].ToString() != "") { 
                            if (AjutExport.tipact.Contains(drXML["tipact"].ToString()))
                            {
                                xmlWriter.WriteElementString("codTip", drXML["tipact"].ToString());
                            }
                            else
                            {
                                xmlWriter.WriteElementString("codTip", "CP");
                            }
                        }
                        else
                        {
                            xmlWriter.WriteElementString("codTip", "CP");
                        }
                        //dataAct
                        if (drXML["dataact"].ToString() != "")
                        {
                            //Console.WriteLine(drXML["dataact"].ToString().Substring(6,4).PadLeft(4,'0'));
                            //Console.WriteLine(drXML["dataact"].ToString().Substring(3,2).PadLeft(2,'0'));
                            //Console.WriteLine(drXML["dataact"].ToString().Substring(0, 2).PadLeft(2, '0'));
                            xmlWriter.WriteElementString("dataAct", drXML["dataact"].ToString().Substring(6,4).PadLeft(4,'0')+"-"+ drXML["dataact"].ToString().Substring(3,2).PadLeft(2,'0')+"-"+ drXML["dataact"].ToString().Substring(0, 2).PadLeft(2, '0')+ "T00:00:00");
                        }
                        else
                        {
                            xmlWriter.WriteElementString("dataAct", AjutExport.getBlankData());
                        }

                        //emitent act
                        if (drXML["emitentact"].ToString() != "")
                        {
                            xmlWriter.WriteElementString("emitent", drXML["emitentact"].ToString());
                        }
                        else
                        {
                            xmlWriter.WriteElementString("emitent", "-");
                        }

                        //nrAct
                        if (drXML["nrAct"].ToString() != "")
                        {
                            if(drXML["emitentact"].ToString().Length>20){
                                xmlWriter.WriteElementString("nrAct", drXML["emitentact"].ToString().Substring(0,20));
                            }else{
                                xmlWriter.WriteElementString("nrAct", drXML["emitentact"].ToString());
                            }
                            
                        }
                        else
                        {
                            xmlWriter.WriteElementString("nrAct", "-");
                        }
                            
                        xmlWriter.WriteEndElement();                          //inchid7

                        xmlWriter.WriteStartElement("proprietar");         //deschid7

                        //nume
                        
                            xmlWriter.WriteElementString("nume", numeTitular);
                        
                            xmlWriter.WriteElementString("prenume", prenumeTitular);
                        

                       
                            if (initialaTitular == "")
                            {
                                xmlWriter.WriteElementString("initialaTata", "-");
                            }
                            else
                            {
                                xmlWriter.WriteElementString("initialaTata", initialaTitular);
                            }
                        
                            

                        xmlWriter.WriteStartElement("cnp");                 //deschid8
                        
                            RaspunsValidare raspuns=CnpValidare.verificaCNP(cnpTitular);
                            if (raspuns.corect==true)
                            {
                                xmlWriter.WriteAttributeString("value", cnpTitular);
                            }
                            else
                            {
                                xmlWriter.WriteAttributeString("value", AjutExport.genereazaCNP(AjutExport.GetNumeJudetRol(strIdRol), DateTime.Now.AddYears(5)));
                            }
                       

                        xmlWriter.WriteEndElement();                          //inchid8
                        xmlWriter.WriteEndElement();                          //inchid7

                        switch (drXML["util"].ToString())
                        {
                            case "ARABIL":
                                xmlWriter.WriteElementString("codCatFolosinta", "1");
                                break;
                            case "PASUNI":
                                xmlWriter.WriteElementString("codCatFolosinta", "2");
                                break;
                            case "FANETE":
                                xmlWriter.WriteElementString("codCatFolosinta", "3");
                                break;
                            case "VII":
                                xmlWriter.WriteElementString("codCatFolosinta", "5");
                                break;
                            case "HAMEISTI":
                                xmlWriter.WriteElementString("codCatFolosinta", "6");
                                break;
                            case "LIVEZI":
                                xmlWriter.WriteElementString("codCatFolosinta", "8");
                                break;
                            case "PADURI":
                                xmlWriter.WriteElementString("codCatFolosinta", "12");
                                break;
                            case "GRADINI FAMILIALE":
                                xmlWriter.WriteElementString("codCatFolosinta", "9");
                                break;
                            case "DRUMURI SI CAI FERATE":
                                xmlWriter.WriteElementString("codCatFolosinta", "13");
                                break;
                            case "CURTI CONSTRUCTII":
                                xmlWriter.WriteElementString("codCatFolosinta", "14");
                                break;
                            case "DEGRADATE SI NEPRODUCTIVE":
                                xmlWriter.WriteElementString("codCatFolosinta", "15");
                                break;
                            case "APE SI BALTI":
                                xmlWriter.WriteElementString("codCatFolosinta", "16");
                                break;
                            case "ALTE TERENURI":
                                xmlWriter.WriteElementString("codCatFolosinta", "15");
                                break;
                            default:
                                xmlWriter.WriteElementString("codCatFolosinta", "15");
                                break;
                        }

                        xmlWriter.WriteElementString("codModalitateDetinere", "CP");


                        xmlWriter.WriteStartElement("localizare");                 //deschid7
                        xmlWriter.WriteAttributeString("codTip", "1");
                        xmlWriter.WriteAttributeString("denumire", "Nr. topografic al parcelei/tarlalei/solei");

                        if (drXML["TOPO"].ToString() == "")
                        {
                            xmlWriter.WriteAttributeString("valoare", "-");
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("valoare", drXML["TOPO"].ToString());
                        }
                        xmlWriter.WriteEndElement();                                //inchid7

                        xmlWriter.WriteStartElement("localizare");                 //deschid7
                        xmlWriter.WriteAttributeString("codTip", "3");
                        xmlWriter.WriteAttributeString("denumire", "C.F. Nr cadastral");
                        xmlWriter.WriteAttributeString("valoare", "-");
                        xmlWriter.WriteEndElement();                          //inchid7

                        xmlWriter.WriteStartElement("localizare");                 //deschid7
                        xmlWriter.WriteAttributeString("codTip", "5");
                        xmlWriter.WriteAttributeString("denumire", "Nr carte funciara");
                        if (drXML["CF"].ToString() == "")
                        {
                            xmlWriter.WriteAttributeString("valoare", "-");
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("valoare", drXML["CF"].ToString());
                        }
                        xmlWriter.WriteEndElement();                          //inchid7

                        if (drXML["mentiuni"].ToString() == "")
                        {
                            xmlWriter.WriteElementString("mentiuni", "mentiune...nu exista");
                        }
                        else
                        {
                            xmlWriter.WriteElementString("mentiuni", drXML["mentiuni"].ToString());
                        }

                        if (drXML["bloc"].ToString() == "")
                        {
                            xmlWriter.WriteElementString("nrBlocFizic", "-");
                        }
                        else
                        {
                            xmlWriter.WriteElementString("nrBlocFizic", drXML["bloc"].ToString());
                        }


                        if (drXML["intra"].ToString() != "0")
                        {
                            xmlWriter.WriteStartElement("intravilan");                 //deschid7
                            xmlWriter.WriteStartElement("intravilanARI");               //deschid8
                            xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(drXML["intra"].ToString()));
                            xmlWriter.WriteEndElement();                                //inchid8
                            xmlWriter.WriteStartElement("intravilanHA");                //deschid8
                            xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(drXML["intra"].ToString()));
                            xmlWriter.WriteEndElement();                                //inchid8
                            xmlWriter.WriteEndElement();                                //inchid7
                        }


                        xmlWriter.WriteEndElement();                        //inchid6
                        intCodRand += 1;
                    }
                    else if (drXML["extra"].ToString() != "0")
                    {
                        xmlWriter.WriteStartElement("identificare_teren");         //deschid6
                        xmlWriter.WriteAttributeString("codRand", intCodRand.ToString());
                        if (drXML["tarlaua"].ToString().Trim() == drXML["parcela"].ToString().Trim())
                        {
                            xmlWriter.WriteAttributeString("denumire", drXML["parcela"].ToString());
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("denumire", drXML["tarlaua"].ToString() + " / " + drXML["parcela"].ToString());
                        }



                        xmlWriter.WriteStartElement("act_detinere");         //deschid7
                        xmlWriter.WriteElementString("codTip", "CP");
                        //Debug.Print(bazadedate.strDataExport);
                        xmlWriter.WriteElementString("dataAct",AjutExport.getBlankData());
                        xmlWriter.WriteElementString("emitent", "-");
                        xmlWriter.WriteElementString("nrAct", "-");
                        xmlWriter.WriteEndElement();                          //inchid7

                        xmlWriter.WriteStartElement("proprietar");         //deschid7
                        xmlWriter.WriteElementString("nume", numeTitular);
                        xmlWriter.WriteElementString("prenume", prenumeTitular);
                        if (initialaTitular == "")
                        {
                            xmlWriter.WriteElementString("initialaTata", "-");
                        }
                        else
                        {
                            xmlWriter.WriteElementString("initialaTata", initialaTitular);
                        }

                        xmlWriter.WriteStartElement("cnp");                 //deschid8
                        xmlWriter.WriteAttributeString("value", cnpTitular);
                        xmlWriter.WriteEndElement();                          //inchid8
                        xmlWriter.WriteEndElement();                          //inchid7

                        switch (drXML["util"].ToString())
                        {
                            case "ARABIL":
                                xmlWriter.WriteElementString("codCatFolosinta", "1");
                                break;
                            case "PASUNI":
                                xmlWriter.WriteElementString("codCatFolosinta", "2");
                                break;
                            case "FANETE":
                                xmlWriter.WriteElementString("codCatFolosinta", "3");
                                break;
                            case "VII":
                                xmlWriter.WriteElementString("codCatFolosinta", "5");
                                break;
                            case "HAMEISTI":
                                xmlWriter.WriteElementString("codCatFolosinta", "6");
                                break;
                            case "LIVEZI":
                                xmlWriter.WriteElementString("codCatFolosinta", "8");
                                break;
                            case "PADURI":
                                xmlWriter.WriteElementString("codCatFolosinta", "12");
                                break;
                            case "GRADINI FAMILIALE":
                                xmlWriter.WriteElementString("codCatFolosinta", "9");
                                break;
                            case "DRUMURI SI CAI FERATE":
                                xmlWriter.WriteElementString("codCatFolosinta", "13");
                                break;
                            case "CURTI CONSTRUCTII":
                                xmlWriter.WriteElementString("codCatFolosinta", "14");
                                break;
                            case "DEGRADATE SI NEPRODUCTIVE":
                                xmlWriter.WriteElementString("codCatFolosinta", "15");
                                break;
                            case "APE SI BALTI":
                                xmlWriter.WriteElementString("codCatFolosinta", "16");
                                break;
                            case "ALTE TERENURI":
                                xmlWriter.WriteElementString("codCatFolosinta", "15");
                                break;
                            default:
                                xmlWriter.WriteElementString("codCatFolosinta", "15");
                                break;
                        }

                        xmlWriter.WriteElementString("codModalitateDetinere", "CP");


                        xmlWriter.WriteStartElement("localizare");                 //deschid7
                        xmlWriter.WriteAttributeString("codTip", "1");
                        xmlWriter.WriteAttributeString("denumire", "Nr. topografic al parcelei/tarlalei/solei");

                        if (drXML["TOPO"].ToString() == "")
                        {
                            xmlWriter.WriteAttributeString("valoare", "-");
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("valoare", drXML["TOPO"].ToString());
                        }
                        xmlWriter.WriteEndElement();                                //inchid7

                        xmlWriter.WriteStartElement("localizare");                 //deschid7
                        xmlWriter.WriteAttributeString("codTip", "3");
                        xmlWriter.WriteAttributeString("denumire", "C.F. Nr cadastral");
                        xmlWriter.WriteAttributeString("valoare", "-");
                        xmlWriter.WriteEndElement();                          //inchid7

                        xmlWriter.WriteStartElement("localizare");                 //deschid7
                        xmlWriter.WriteAttributeString("codTip", "5");
                        xmlWriter.WriteAttributeString("denumire", "Nr carte funciara");
                        if (drXML["CF"].ToString() == "")
                        {
                            xmlWriter.WriteAttributeString("valoare", "-");
                        }
                        else
                        {
                            xmlWriter.WriteAttributeString("valoare", drXML["CF"].ToString());
                        }
                        xmlWriter.WriteEndElement();                          //inchid7

                        if (drXML["mentiuni"].ToString() == "")
                        {
                            xmlWriter.WriteElementString("mentiuni", "mentiune...nu exista");
                        }
                        else
                        {
                            xmlWriter.WriteElementString("mentiuni", drXML["mentiuni"].ToString());
                        }

                        if (drXML["bloc"].ToString() == "")
                        {
                            xmlWriter.WriteElementString("nrBlocFizic", "-");
                        }
                        else
                        {
                            xmlWriter.WriteElementString("nrBlocFizic", drXML["bloc"].ToString());
                        }

                        if (drXML["extra"].ToString() != "0")
                        {
                            xmlWriter.WriteStartElement("extravilan");                 //deschid7
                            xmlWriter.WriteStartElement("extravilanARI");               //deschid8
                            xmlWriter.WriteAttributeString("value", AjutExport.scoateAri(drXML["extra"].ToString()));
                            xmlWriter.WriteEndElement();                                //inchid8
                            xmlWriter.WriteStartElement("extravilanHA");                //deschid8
                            xmlWriter.WriteAttributeString("value", AjutExport.scoateHa(drXML["extra"].ToString()));
                            xmlWriter.WriteEndElement();                                //inchid8
                            xmlWriter.WriteEndElement();                                //inchid7
                        }


                        xmlWriter.WriteEndElement();                        //inchid6
                        intCodRand += 1;
                    }
                }


                xmlWriter.WriteEndElement();                        //inchid5
                xmlWriter.WriteEndElement();                        //inchid4

                xmlWriter.WriteEndElement();                        //inchid3



                xmlWriter.WriteEndElement();                        //inchid2
                xmlWriter.WriteEndElement();                        //inchid1
                xmlWriter.Close();
                xmlWriter.Dispose();
                drXML.Close();
                //###############################################################
                if(strIdRol=="1.26.42.1.20."){
                    Console.WriteLine("acum");
                }
                return true;
            }
            catch(System.Exception ex){
                Ajutatoare.scrielinie("eroriXML.log",  AjutExport.numefisier(strIdRol) + "xml " + ex.Message);
                return false;
            }
        }
    }
}