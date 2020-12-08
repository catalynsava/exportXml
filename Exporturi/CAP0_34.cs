using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP0_34
    {
        public static bool make_CAP0_34xml(string strIdRol)
        {
            
            string strGosp = strIdRol;
            strGosp = strGosp.Substring(0, strIdRol.Length - 3);

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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

            strSQL = "SELECT * FROM adrrol WHERE idRol=\"" + strIdRol + "\";";
            OleDbCommand cmdAdrrol = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drAdrrol = cmdAdrrol.ExecuteReader();
            if (drAdrrol.Read() == false)
            {
                return false;
            }
            
            Sirute adrrolSirute=new Sirute(drAdrrol["localitate"].ToString(), drAdrrol["judet"].ToString());

            if (adrrolSirute.Siruta == "" | adrrolSirute.SirutaJudet == "" | adrrolSirute.SirutaSuperioara == "")
            {
                Console.WriteLine(adrrolSirute.SirutaJudet+ " " + adrrolSirute.SirutaSuperioara + " " + adrrolSirute.Siruta);
                return false;
            }

            
            string formadeorganizarestring=AjutExport.getFormaDeOrganizare(drAdrrol["nume"].ToString());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;


            XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(strIdRol) + "xml", settings);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("DOCUMENT_RAN");
            xmlWriter.WriteStartElement("HEADER");

            xmlWriter.WriteStartElement("codXml");
            xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
            xmlWriter.WriteEndElement();



            xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());

            xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");

            xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("BODY");
            xmlWriter.WriteStartElement("gospodarie");
            xmlWriter.WriteAttributeString("identificator", strGosp);

            xmlWriter.WriteStartElement("capitol_0_34");
            xmlWriter.WriteAttributeString("codCapitol", "CAP0_34");
            xmlWriter.WriteAttributeString("denumire", "Date de identificare a gospodăriei deținute de persoană juridică");
            xmlWriter.WriteStartElement("date_identificare_gospodarie_PJ");

            //<adresaGospodarie>
            xmlWriter.WriteStartElement("adresaGospodarie");
            xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
            xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
            xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
            xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
            xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
            xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
            xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
            xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
            xmlWriter.WriteEndElement();

            //<codExploatatie>
            if (drAdrrol["codexp"].ToString()!="")
            {
                xmlWriter.WriteElementString("codExploatatie", drAdrrol["codexp"].ToString());
            }
            else
            {
                xmlWriter.WriteElementString("codExploatatie", "");
            }

            //<domiciliuFiscalRo>
            string strSQLper = "SELECT * FROM adrPJ WHERE idrol=\"" + strIdRol + "\";";
            OleDbCommand cmdAdresaPJ = new OleDbCommand(strSQLper, BazaDeDate.conexiune );
            OleDbDataReader drAdresaPJ = cmdAdresaPJ.ExecuteReader();
            if(drAdresaPJ.Read()){

                Sirute adrpjSirute=new Sirute(drAdresaPJ["localitate"].ToString(), drAdresaPJ["judet"].ToString());

                if (adrpjSirute.SirutaJudet== "" || adrpjSirute.SirutaSuperioara == "" || adrpjSirute.Siruta == ""){
                    xmlWriter.WriteStartElement("domiciliuFiscalRo");
                    xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
                    xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
                    xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
                    xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
                    xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
                    xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
                    xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
                    xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
                    xmlWriter.WriteEndElement();
                }else{
                    xmlWriter.WriteStartElement("domiciliuFiscalRo");
                    xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
                    xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
                    xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
                    xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
                    xmlWriter.WriteElementString("sirutaJudet", adrpjSirute.SirutaJudet);
                    xmlWriter.WriteElementString("sirutaLocalitate", adrpjSirute.Siruta);
                    xmlWriter.WriteElementString("sirutaUAT", adrpjSirute.SirutaSuperioara);
                    xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
                    xmlWriter.WriteEndElement();
                }  
            }else{
                xmlWriter.WriteStartElement("domiciliuFiscalRo");
                xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
                xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
                xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
                xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
                xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
                xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
                xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
                xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
                xmlWriter.WriteEndElement();
            }
            //--

            //<nrUnicIdentificare>
            xmlWriter.WriteElementString("nrUnicIdentificare", drAdrrol["nrUI"].ToString());
            //--

            //<pozitieGospodarie>
            xmlWriter.WriteStartElement("pozitieGospodarie");
            xmlWriter.WriteElementString("pozitiaAnterioara", "0");
            xmlWriter.WriteElementString("pozitieCurenta", drAdrrol["poz"].ToString());
            xmlWriter.WriteElementString("volumul", drAdrrol["vol"].ToString());
            int x = 0;
            Int32.TryParse(drAdrrol["rolimp"].ToString(), out x);
            xmlWriter.WriteElementString("rolNominalUnic", x.ToString());
            xmlWriter.WriteEndElement();
            //--

            //<tipDetinator>
            xmlWriter.WriteElementString("tipDetinator", drAdrrol["tip"].ToString());
            //--

            //<tipExploatatie>
            string strNomCodExploatatie;
            switch (drAdrrol["tipExploa"].ToString())
            {
                case "1. Gospodarie/Exploatatie agricola individuala":
                    strNomCodExploatatie = "1";
                    break;
                case "2. Persoana fizica autorizata/ Intreprindere individuala/ Intreprindere familiala":
                    strNomCodExploatatie = "2";
                    break;
                case "3. a) regie autonoma":
                    strNomCodExploatatie = "3";
                    break;
                case "3. b) societate/asociatie agricola (Legea nr. 36/1991)":
                    strNomCodExploatatie = "4";
                    break;
                case "3. c) societate comerciala cu capital majoritar privat (Legea nr. 31/1990)":
                    strNomCodExploatatie = "5";
                    break;
                case "3. d) societate comerciala cu capital majoritar de stat (Legea nr. 31/1990)":
                    strNomCodExploatatie = "6";
                    break;
                case "3. e) institut/ statiune de cercetare":
                    strNomCodExploatatie = "7";
                    break;
                case "3. f) unitate/ subdiviziune administrativ teritoriala":
                    strNomCodExploatatie = "8";
                    break;
                case "3. g) alte institutii publice centrale sau locale":
                    strNomCodExploatatie = "9";
                    break;
                case "3. h) unitate cooperatista":
                    strNomCodExploatatie = "10";
                    break;
                case "3. i) alte tipuri (asociatie, fundatie, asezamant religios, scoala etc.)":
                    strNomCodExploatatie = "11";
                    break;
                default:
                    strNomCodExploatatie = "3";
                    break;
            }
            xmlWriter.WriteElementString("tipExploatatie", strNomCodExploatatie);
            //--

            //<adresaReprezentantLegal>
            strSQLper = "SELECT * FROM PerJur WHERE idrol=\"" + strIdRol + "\";";
            OleDbCommand cmdTEMPper = new OleDbCommand(strSQLper, BazaDeDate.conexiune);
            OleDbDataReader drTEMPper = cmdTEMPper.ExecuteReader();

            if (drTEMPper.Read())
            {
                //RaspunsValidare raspunsCnpPerJur=new RaspunsValidare();
                //raspunsCnpPerJur=Validari.CnpValidare.verificaCNP(drTEMPper["cnp"].ToString());
                //if(raspunsCnpPerJur.corect==true){
                
                    Sirute pjSirute=new Sirute(drTEMPper["localitate"].ToString(), drTEMPper["judet"].ToString());
                    Console.WriteLine(pjSirute.SirutaJudet+ " " + pjSirute.SirutaSuperioara + " " + pjSirute.Siruta);

                    if (pjSirute.SirutaJudet== "" || pjSirute.SirutaSuperioara == "" || pjSirute.Siruta == ""){
                        xmlWriter.WriteStartElement("adresaReprezentantLegal");
                        xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
                        xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
                        xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
                        xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
                        xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
                        xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
                        xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
                        xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
                        xmlWriter.WriteEndElement();
                    }else{
                        xmlWriter.WriteStartElement("adresaReprezentantLegal");
                        xmlWriter.WriteElementString("apartament", drTEMPper["ap"].ToString());
                        xmlWriter.WriteElementString("bloc", drTEMPper["bloc"].ToString());
                        xmlWriter.WriteElementString("etaj", "");
                        xmlWriter.WriteElementString("numar", drTEMPper["nr"].ToString());
                        xmlWriter.WriteElementString("scara", "");
                        xmlWriter.WriteElementString("sirutaJudet", pjSirute.SirutaJudet);
                        xmlWriter.WriteElementString("sirutaLocalitate", pjSirute.Siruta);
                        xmlWriter.WriteElementString("sirutaUAT", pjSirute.SirutaSuperioara);
                        xmlWriter.WriteElementString("strada", drTEMPper["strada"].ToString());
                        xmlWriter.WriteEndElement();
                    }
                //}
            }else{
                xmlWriter.WriteStartElement("adresaReprezentantLegal");
                xmlWriter.WriteElementString("apartament", drAdrrol["ap"].ToString());
                xmlWriter.WriteElementString("bloc", drAdrrol["bloc"].ToString());
                xmlWriter.WriteElementString("numar", drAdrrol["nr"].ToString());
                xmlWriter.WriteElementString("scara", drAdrrol["scara"].ToString());
                xmlWriter.WriteElementString("sirutaJudet", adrrolSirute.SirutaJudet);
                xmlWriter.WriteElementString("sirutaLocalitate", adrrolSirute.Siruta);
                xmlWriter.WriteElementString("sirutaUAT", adrrolSirute.SirutaSuperioara);
                xmlWriter.WriteElementString("strada", drAdrrol["strada"].ToString());
                xmlWriter.WriteEndElement();
            }
            //--


            //<persoanaJuridica>
            xmlWriter.WriteStartElement("persoanaJuridica");
            xmlWriter.WriteStartElement("cui");
            xmlWriter.WriteAttributeString("value", drAdrrol["cnp"].ToString());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteElementString("denumire", drAdrrol["nume"].ToString());

            xmlWriter.WriteElementString("formaOrganizareRC", AjutExport.getFormaDeOrganizare(drAdrrol["nume"].ToString()));

                    
            xmlWriter.WriteEndElement();
            //--
            
            //<reprezentantLegal>
            strSQLper = "SELECT * FROM PerJur WHERE idrol=\"" + strIdRol + "\";";
            cmdTEMPper = new OleDbCommand(strSQLper, BazaDeDate.conexiune);
            drTEMPper = cmdTEMPper.ExecuteReader();

            if (drTEMPper.Read())
            {
                
                xmlWriter.WriteStartElement("reprezentantLegal");
                if (drTEMPper["nume"].ToString().Length>0)
                {
                    xmlWriter.WriteElementString("nume", drTEMPper["nume"].ToString());
                }else{
                    xmlWriter.WriteElementString("nume", "-");
                }
                if (drTEMPper["prenume"].ToString().Length > 0)
                {
                    xmlWriter.WriteElementString("prenume", drTEMPper["prenume"].ToString());
                }
                else
                {
                    xmlWriter.WriteElementString("prenume", "-");
                }
                    
                if (drTEMPper["i"].ToString().Length>0)
                {
                    xmlWriter.WriteElementString("initialaTata", drTEMPper["i"].ToString());
                }else{
                    xmlWriter.WriteElementString("initialaTata", "-");
                }
                    
                xmlWriter.WriteStartElement("cnp");
                RaspunsValidare rsp=new RaspunsValidare();
                rsp=CnpValidare.verificaCNP(drTEMPper["cnp"].ToString());
                if (rsp.corect==true)
                {
                    xmlWriter.WriteAttributeString("value", drTEMPper["cnp"].ToString());
                }else{
                    //xmlWriter.WriteAttributeString("value", "");
                    xmlWriter.WriteAttributeString("value", 
                            AjutExport.genereazaCNP
                            (AjutExport.GetNumeJudetRol(strIdRol),DateTime.Now.AddYears(5))
                    );
                }
                    
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                
            }else{
                xmlWriter.WriteStartElement("reprezentantLegal");
                   
                xmlWriter.WriteElementString("nume", "-");
                   
                   
                xmlWriter.WriteElementString("prenume", "-");
                    
                xmlWriter.WriteElementString("initialaTata", "-");
                    
                    
                xmlWriter.WriteStartElement("cnp");
                   
                xmlWriter.WriteAttributeString("value", 
                            AjutExport.genereazaCNP
                            (AjutExport.GetNumeJudetRol(strIdRol),DateTime.Now.AddYears(5))
                );
                    
                    
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            //--
                
                
            
            

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            drAdrrol.Close();
            return true;
            
        }
    }
}