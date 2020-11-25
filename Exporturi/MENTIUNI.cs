using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class MENTIUNI
    {
         public static bool make_MENTIUNIxml(string strIdRol)
        {
            try
            {
                //--
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\MENTIUNI\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
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
                strSQL = "SELECT * FROM MENTIUNI WHERE IDROL=\"" + strIdRol + "\";";
                OleDbCommand cmdXML = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                //--

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //---------------------------------//

                //-- 
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\MENTIUNI\\" + strGosp + "xml", settings);
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
                xmlWriter.WriteStartElement("capitol_16");         //deschid capitol_12
                xmlWriter.WriteAttributeString("codCapitol", "CAP16");
                xmlWriter.WriteAttributeString("denumire", "Mentiuni speciale");
                if(drXML.Read())
                {
                    xmlWriter.WriteElementString("mentiuni_speciale", drXML["Mentiuni"].ToString());
                }
                else
                {
                    xmlWriter.WriteElementString("mentiuni_speciale", "-");
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