using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP12
    {
         public static bool make_CAP12xml(string strIdRol)
        {
            try
            {
                int nr = 1;
                //--
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP12\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT * FROM CAP12 WHERE IDROL=\"" + strIdRol + "\";";
                OleDbCommand cmdXML = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                //--

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //-- 
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP12\\" + strGosp + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_12");         //deschid capitol_12
                xmlWriter.WriteAttributeString("codCapitol", "CAP12");
                xmlWriter.WriteAttributeString("denumire", "Atestatele de producator si carnetele de comercializare eliberate/vizate");
                while (drXML.Read())
                {
                    xmlWriter.WriteStartElement("atestat_producator");         //deschid atestat_producator
                    xmlWriter.WriteElementString("dataAvizConsultativ", AjutExport.dataro_dataxml(drXML["dataAviz"].ToString()));
                    xmlWriter.WriteElementString("dataEliberare",AjutExport.dataro_dataxml(drXML["dataAtest"].ToString()));
                    xmlWriter.WriteElementString("nrAvizConsulativ", AjutExport.dataro_dataxml(drXML["nrAviz"].ToString()));
                    foreach(string cProdus in AjutExport.getProduse(drXML["nrAtest"].ToString()))
                    {
                        xmlWriter.WriteStartElement("produs");         //deschid produs
                        xmlWriter.WriteAttributeString("denumire", cProdus);
                        xmlWriter.WriteEndElement();                    //inchid produs
                    }
                    xmlWriter.WriteElementString("serieNumar", drXML["nrAtest"].ToString());
                    xmlWriter.WriteStartElement("viza");                        //deschid viza
                    if(AjutExport.EsteDeTipData(drXML["dataVizariiTrim"].ToString())==true){
                        xmlWriter.WriteElementString("dataViza", AjutExport.dataro_dataxml(drXML["dataVizariiTrim"].ToString()));
                    }else{
                        xmlWriter.WriteElementString("dataViza", "1900-01-01T00:00:00");
                    }
                    
                    xmlWriter.WriteElementString("numarViza", nr.ToString());
                    xmlWriter.WriteEndElement();                                //inchid viza
                    xmlWriter.WriteEndElement();                                //inchid atestat_producator
                    nr += 1;
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
            }
            

            
        }
    }
}