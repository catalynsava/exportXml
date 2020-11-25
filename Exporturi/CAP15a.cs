using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP15a
    {
         public static bool make_CAP15a(string strIdRol)
        {
            try
            {
                int nrcrt = 1;
                bool ePJ = false;

                //--
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15a\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
                {
                    Ajutatoare.scrielinie("eroriXML.log", " există deja: " + AjutExport.numefisier(strIdRol) + "xml");
                    return false;
                }
                //--

                //siruta--
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
                //--

                //baza de date--
                strSQL = "SELECT * FROM CAP15 WHERE IDROL=\"" + strIdRol + "\" AND tippers=\"ARENDAS\" ORDER BY nrContr, data;";
                //Console.WriteLine(strSQL) ;
                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                //--

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //-- 
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15a\\" + strGosp + "xml", settings);
                xmlWriter.WriteStartDocument();
                //--

                //--
                xmlWriter.WriteStartElement("DOCUMENT_RAN");        //deschid DOCUMENT_RAN
                //--

                //header--
                xmlWriter.WriteStartElement("HEADER");              //deschid HEADER
                xmlWriter.WriteStartElement("codXml");              //deschide codXml
                xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
                xmlWriter.WriteEndElement();                        //inchid codXml
                xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());
                xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");
                xmlWriter.WriteElementString("sirutaUAT", datgenSirute.SirutaSuperioara);
                xmlWriter.WriteEndElement();                        //inchid HEADER
                //--

                //body--
                xmlWriter.WriteStartElement("BODY");                //deschid BODY
                xmlWriter.WriteStartElement("gospodarie");          //deschid gospodarie
                xmlWriter.WriteAttributeString("identificator", strGosp);
                xmlWriter.WriteStartElement("capitol_15a");         //deschid capitol_15a
                xmlWriter.WriteAttributeString("codCapitol", "CAP15a");
                xmlWriter.WriteAttributeString("denumire", "Inregistrari privind contractele de arendare");

                 while (drXML.Read())
                {
                  

                    

                    string strTipExploa=AjutExport.getTipExploatatie(drXML["cnp"].ToString());
                    if(strTipExploa==""){
                        ePJ=false;

                        if (drXML["nume"].ToString().Substring(0,2).ToUpper()=="SC")
                        {
                            ePJ = true;
                        }
                        else
                        {
                            if (drXML["nume"].ToString().Substring(drXML["nume"].ToString().Length - 3, 3).ToUpper() == "SRL")
                            {
                                ePJ = true;
                            }
                            else
                            {
                                ePJ = false;
                            }
                        }

                    }else{
                        if (strTipExploa.Substring(0,1)=="1"){
                        ePJ=false ;
                    }else{
                        ePJ=true;
                    }
                    }
                    
                   

                    xmlWriter.WriteStartElement("contract_arendare");   //deschid contract_arendare

                    xmlWriter.WriteElementString("codCategFolosinta", "VPVH");
                    xmlWriter.WriteElementString("dataContract", AjutExport.dataro_dataxml(drXML["data"].ToString()));
                    //"1900-01-01T00:00:00"
                    xmlWriter.WriteElementString("dataStart", AjutExport.dataro_dataxml(drXML["data"].ToString()));
                    xmlWriter.WriteElementString("dataStop", AjutExport.dataro_dataxml(drXML["dataExp"].ToString()));
                    xmlWriter.WriteElementString("nrContract", drXML["nrContr"].ToString());
                    xmlWriter.WriteElementString("nrCrt", nrcrt.ToString());
                    nrcrt += 1;
                    xmlWriter.WriteElementString("redeventaLei", drXML["redev"].ToString());
                    xmlWriter.WriteStartElement("suprafataMP");

                    xmlWriter.WriteAttributeString("value", (AjutExport.getSuprafataContrArenda(drXML["nrContr"].ToString(),drXML["data"].ToString())).ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteElementString("arendaInProduse","");
                    

                    if (ePJ==false)
                    {
                        xmlWriter.WriteStartElement("arendasPF");
                        xmlWriter.WriteElementString("nume", drXML["nume"].ToString());
                        if (drXML["prenume"].ToString()!="")
                        {
                            xmlWriter.WriteElementString("prenume", drXML["prenume"].ToString());
                        }
                        else
                        {
                            xmlWriter.WriteElementString("prenume", "-");
                        }
                        
                        if (drXML["i"].ToString()!="")
                        {
                            xmlWriter.WriteElementString("initialaTata", drXML["i"].ToString());
                        }
                        else
                        {
                            xmlWriter.WriteElementString("initialaTata", "-");
                        }
                        
                        xmlWriter.WriteStartElement("cnp");
                        if (drXML["cnp"].ToString()!="")
                        {
                            RaspunsValidare cnpvalid=new RaspunsValidare();
                            cnpvalid=CnpValidare.verificaCNP(drXML["cnp"].ToString());
                            if(cnpvalid.corect==true){
                                xmlWriter.WriteAttributeString("value", drXML["cnp"].ToString());
                            }else{
                                xmlWriter.WriteAttributeString("value", AjutExport.getCNPdupaNumeInitialaPrenume(drXML["nume"].ToString(),drXML["i"].ToString(),drXML["prenume"].ToString()));
                            }
                        }
                        else
                        {
                            string tmpcnp = AjutExport.getCNPdupaNumeInitialaPrenume(drXML["Nume"].ToString(), drXML["i"].ToString(), drXML["prenume"].ToString());
                            if (tmpcnp!="")
                            {
                                xmlWriter.WriteAttributeString("value", tmpcnp);
                            }
                            else
                            {
                                xmlWriter.WriteAttributeString("value", AjutExport.genereazaCNP(AjutExport.GetNumeJudetRol(strIdRol), DateTime.Now));
                            }
                            
                        }
                    
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                    }
                    else
                    {
                        xmlWriter.WriteStartElement("arendasRC");       //START arendasRC
                        xmlWriter.WriteStartElement("cui");             //START cui

                        if (drXML["cnp"].ToString() != "")
                        {
                            xmlWriter.WriteAttributeString("value", drXML["cnp"].ToString());
                        }
                        else
                        {
                            string tmpcui = AjutExport.getCUIdupaNumeFirma(drXML["Nume"].ToString());
                            if (tmpcui != "")
                            {
                                xmlWriter.WriteAttributeString("value", tmpcui);
                                tmpcui="";
                            }
                            else
                            {
                                xmlWriter.WriteAttributeString("value", AjutExport.genereazaCNP(AjutExport.GetNumeJudetRol(strIdRol), DateTime.Now));
                            }
                        }

                        xmlWriter.WriteEndElement();        //inchid cui

                        xmlWriter.WriteElementString("denumire",drXML["nume"].ToString());
                        xmlWriter.WriteElementString("formaOrganizareRC", AjutExport.getFormaDeOrganizare(drXML["nume"].ToString()));
                        xmlWriter.WriteEndElement();        //inchid arendasRC
                    }
                    xmlWriter.WriteEndElement();        //contract_arendare
                }
                
                xmlWriter.WriteEndElement();                        //inchid capitol_16
                xmlWriter.WriteEndElement();                        //inchid gospodarie
                xmlWriter.WriteEndElement();                        //inchid BODY
                //--
                //--
                xmlWriter.WriteEndElement();                        //inchid DOCUMENT_RAN
                //--
                //--
                xmlWriter.Close();
                //--
                return true;
            }
            catch (System.Exception ex)
            {
                Ajutatoare.scrielinie("eroriXML.log",  AjutExport.numefisier(strIdRol) + "xml " + ex.Message);
                return false;
                throw;
            }
        }
    }
}