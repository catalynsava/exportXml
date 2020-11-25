using System;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using exportXml.Validari;

namespace exportXml.Exporturi
{
    public class CAP4a
    {
        public static bool make_CAP4axml(string strIdRol)
        {
            try
            {
                string strGosp = strIdRol.Substring(0, strIdRol.Length - 3);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a\\" + AjutExport.numefisier(strIdRol) + "xml") == true)
                {
                    Ajutatoare.scrielinie("eroriXML.log", " există deja: " + AjutExport.numefisier(strIdRol) + "xml");
                    return false;
                }

                //siruta--
                string strSQL = "SELECT * FROM datgen;";
                OleDbCommand cmdDateGenerale = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drDateGenerale = cmdDateGenerale.ExecuteReader();
                if (drDateGenerale.Read() == false){return false;}
                Sirute datgenSirute=new Sirute(drDateGenerale["localitate"].ToString(), drDateGenerale["judet"].ToString());
                if (datgenSirute.Siruta == "" | datgenSirute.SirutaJudet == "" | datgenSirute.SirutaSuperioara == "")
                {
                    Console.WriteLine(datgenSirute.SirutaJudet+ " " + datgenSirute.SirutaSuperioara + " " + datgenSirute.Siruta);
                    return false;
                }
                //--

                //baza de date--
                strSQL = "SELECT ROL.nrcrt, CAP4.tot FROM CAP4 LEFT JOIN (SELECT * FROM NOMCAP4) AS ROL ON CAP4.NrCrt = ROL.NrCrt WHERE CAP4.IDROL=\"" + strIdRol + "\"  ORDER BY ROL.nrcrt;";
                OleDbCommand cmdXML = new OleDbCommand(strSQL, BazaDeDate.conexiune);
                OleDbDataReader drXML = cmdXML.ExecuteReader();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineOnAttributes = true;
                //--

                //DOCUMENT_RAN
                XmlWriter xmlWriter = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a\\" + strGosp + "xml", settings);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("DOCUMENT_RAN");        //DOCUMENT_RAN
                //--

                //header
                xmlWriter.WriteStartElement("HEADER");              //HEADER
                xmlWriter.WriteStartElement("codXml");              //codXml
                xmlWriter.WriteAttributeString("value", AjutExport.genereazaGUID());
                xmlWriter.WriteEndElement();                        //inchid codXml
                xmlWriter.WriteElementString("dataExport", AjutExport.dataexportxml());
                xmlWriter.WriteElementString("indicativ", "ADAUGA_SI_INLOCUIESTE");
                xmlWriter.WriteElementString("sirutaUAT", datgenSirute.SirutaSuperioara);
                xmlWriter.WriteEndElement();                        //inchid HEADER
                //--

                //BODY
                xmlWriter.WriteStartElement("BODY");                        //BODY
                xmlWriter.WriteStartElement("gospodarie");                  //gospodarie
                xmlWriter.WriteAttributeString("identificator", strGosp);
                xmlWriter.WriteStartElement("anRaportare");                 //anRaportare
                xmlWriter.WriteAttributeString("an", "2020");
                xmlWriter.WriteStartElement("capitol_4a");                  //capitol
                xmlWriter.WriteAttributeString("codCapitol", "CAP4a");
                xmlWriter.WriteAttributeString("denumire", "Suprafața arabilă situată pe raza localității - culturi în câmp");
                //--

                //parcurg--
                while (drXML.Read())
                {
                    xmlWriter.WriteStartElement("cultura_in_camp");                         //cultura_in_camp
                    xmlWriter.WriteAttributeString("codNomenclator", drXML["nrcrt"].ToString());
                    xmlWriter.WriteAttributeString("codRand", drXML["nrcrt"].ToString());
                    switch (drXML["nrcrt"].ToString())
                    {
                        case "1":
                            xmlWriter.WriteAttributeString("denumire", "Cereale pentru boabe – total (exclusiv sămânța) cod (02+...+19)");
                            break;
                        case "2":
                            xmlWriter.WriteAttributeString("denumire", "Grâu comun de toamnă");
                            break;
                        case "3":
                            xmlWriter.WriteAttributeString("denumire", "Grâu comun de primăvară");
                            break;
                        case "4":
                            xmlWriter.WriteAttributeString("denumire", "Grâu dur de toamnă");
                            break;
                        case "5":
                            xmlWriter.WriteAttributeString("denumire", "Grâu dur de primăvară");
                            break;
                        case "6":
                            xmlWriter.WriteAttributeString("denumire", "Secară de toamnă");
                            break;
                        case "7":
                            xmlWriter.WriteAttributeString("denumire", "Secară de primăvară");
                            break;
                        case "8":
                            xmlWriter.WriteAttributeString("denumire", "Orz");
                            break;
                        case "9":
                            xmlWriter.WriteAttributeString("denumire", "Orzoaică de toamnă");
                            break;
                        case "10":
                            xmlWriter.WriteAttributeString("denumire", "Orzoaică de primăvară");
                            break;
                        case "11":
                            xmlWriter.WriteAttributeString("denumire", "Ovăz");
                            break;
                        case "12":
                            xmlWriter.WriteAttributeString("denumire", "Porumb boabe");
                            break;
                        case "13":
                            xmlWriter.WriteAttributeString("denumire", "Sorg pentru boabe");
                            break;
                        case "14":
                            xmlWriter.WriteAttributeString("denumire", "Triticale de toamnă");
                            break;
                        case "15":
                            xmlWriter.WriteAttributeString("denumire", "Triticale de primăvară");
                            break;
                        case "16":
                            xmlWriter.WriteAttributeString("denumire", "Orez");
                            break;
                        case "17":
                            xmlWriter.WriteAttributeString("denumire", "Mei");
                            break;
                        case "18":
                            xmlWriter.WriteAttributeString("denumire", "Hrișcă");
                            break;
                        case "19":
                            xmlWriter.WriteAttributeString("denumire", "Alte cereale");
                            break;
                        case "20":
                            xmlWriter.WriteAttributeString("denumire", "Leguminoase boabe – total (exclusiv sămânța) cod (21+...+26)");
                            break;
                        case "21":
                            xmlWriter.WriteAttributeString("denumire", "Mazăre boabe");
                            break;
                        case "22":
                            xmlWriter.WriteAttributeString("denumire", "Fasole boabe");
                            break;
                        case "23":
                            xmlWriter.WriteAttributeString("denumire", "Linte");
                            break;
                        case "24":
                            xmlWriter.WriteAttributeString("denumire", "Bob");
                            break;
                        case "25":
                            xmlWriter.WriteAttributeString("denumire", "Lupin");
                            break;
                        case "26":
                            xmlWriter.WriteAttributeString("denumire", "Alte leguminoase boabe (năut, măzăriche etc.)");
                            break;
                        case "27":
                            xmlWriter.WriteAttributeString("denumire", "Rădăcinoase – total cod (28+29+30)");
                            break;
                        case "28":
                            xmlWriter.WriteAttributeString("denumire", "Sfeclă de zahăr (exclusiv sămânță)");
                            break;
                        case "29":
                            xmlWriter.WriteAttributeString("denumire", "Sfeclă furajeră (exclusiv sămânță)");
                            break;
                        case "30":
                            xmlWriter.WriteAttributeString("denumire", "Alte rădăcinoase furajere (morcovi furajeri, napi, etc.) (exclusiv sămânță)");
                            break;
                        case "31":
                            xmlWriter.WriteAttributeString("denumire", "Cartofi – total (exclusiv sămânță) cod (32+33+34)");
                            break;
                        case "32":
                            xmlWriter.WriteAttributeString("denumire", "a)timpurii și semitimpurii");
                            break;
                        case "33":
                            xmlWriter.WriteAttributeString("denumire", "b)de vară");
                            break;
                        case "34":
                            xmlWriter.WriteAttributeString("denumire", "c)de toamnă");
                            break;
                        case "35":
                            xmlWriter.WriteAttributeString("denumire", "Plante textile - total (exclusiv sămânța) cod (36+37)");
                            break;
                        case "36":
                            xmlWriter.WriteAttributeString("denumire", "In pentru fibră");
                            break;
                        case "37":
                            xmlWriter.WriteAttributeString("denumire", "Cânepă pentru fibră");
                            break;
                        case "38":
                            xmlWriter.WriteAttributeString("denumire", "Plante oleaginoase - total (exclusiv sămânța) cod (39+...+47)");
                            break;
                        case "39":
                            xmlWriter.WriteAttributeString("denumire", "Floarea-soarelui");
                            break;
                        case "40":
                            xmlWriter.WriteAttributeString("denumire", "Rapiță pentru ulei");
                            break;
                        case "41":
                            xmlWriter.WriteAttributeString("denumire", "Soia boabe");
                            break;
                        case "42":
                            xmlWriter.WriteAttributeString("denumire", "Muștar");
                            break;
                        case "43":
                            xmlWriter.WriteAttributeString("denumire", "In pentru ulei");
                            break;
                        case "44":
                            xmlWriter.WriteAttributeString("denumire", "Mac");
                            break;
                        case "45":
                            xmlWriter.WriteAttributeString("denumire", "Ricin");
                            break;
                        case "46":
                            xmlWriter.WriteAttributeString("denumire", "Șofrănel");
                            break;
                        case "47":
                            xmlWriter.WriteAttributeString("denumire", "Alte plante oleaginoase");
                            break;
                        case "48":
                            xmlWriter.WriteAttributeString("denumire", "Plante pentru alte industrializări – total (exclusiv sămânța) cod (49+...+53)");
                            break;
                        case "49":
                            xmlWriter.WriteAttributeString("denumire", "Tutun");
                            break;
                        case "50":
                            xmlWriter.WriteAttributeString("denumire", "Sorg pentru mături");
                            break;
                        case "51":
                            xmlWriter.WriteAttributeString("denumire", "Arahide");
                            break;
                        case "52":
                            xmlWriter.WriteAttributeString("denumire", "Cicoare");
                            break;
                        case "53":
                            xmlWriter.WriteAttributeString("denumire", "Alte plante pentru alte industrializări");
                            break;
                        case "54":
                            xmlWriter.WriteAttributeString("denumire", "Plante medicinale, aromatice și condimente – total (exclusiv sămânța) Cod (55+...+61)");
                            break;
                        case "55":
                            xmlWriter.WriteAttributeString("denumire", "Coriandru");
                            break;
                        case "56":
                            xmlWriter.WriteAttributeString("denumire", "Chimion");
                            break;
                        case "57":
                            xmlWriter.WriteAttributeString("denumire", "Fenicul");
                            break;
                        case "58":
                            xmlWriter.WriteAttributeString("denumire", "Levănțică");
                            break;
                        case "59":
                            xmlWriter.WriteAttributeString("denumire", "Mentă");
                            break;
                        case "60":
                            xmlWriter.WriteAttributeString("denumire", "Anason");
                            break;
                        case "61":
                            xmlWriter.WriteAttributeString("denumire", "Alte plante medicinale, aromatice și condimente");
                            break;
                        case "62":
                            xmlWriter.WriteAttributeString("denumire", "Legume în câmp - total (exclusiv sămânța) cod (63+...+73+75+...92)");
                            break;
                        case "63":
                            xmlWriter.WriteAttributeString("denumire", "Conopidă și brocoli");
                            break;
                        case "64":
                            xmlWriter.WriteAttributeString("denumire", "Varză albă");
                            break;
                        case "65":
                            xmlWriter.WriteAttributeString("denumire", "Gulii și gulioare");
                            break;
                        case "66":
                            xmlWriter.WriteAttributeString("denumire", "Alte vărzoase (varză roșie, varză de Bruxelles, etc.)");
                            break;
                        case "67":
                            xmlWriter.WriteAttributeString("denumire", "Țelină frunze");
                            break;
                        case "68":
                            xmlWriter.WriteAttributeString("denumire", "Praz");
                            break;
                        case "69":
                            xmlWriter.WriteAttributeString("denumire", "Salată");
                            break;
                        case "70":
                            xmlWriter.WriteAttributeString("denumire", "Spanac");
                            break;
                        case "71":
                            xmlWriter.WriteAttributeString("denumire", "Alte legume cultivate pentru frunze");
                            break;
                        case "72":
                            xmlWriter.WriteAttributeString("denumire", "Tomate total");
                            break;
                        case "73":
                            xmlWriter.WriteAttributeString("denumire", "Castraveți total");
                            break;
                        case "74":
                            xmlWriter.WriteAttributeString("denumire", "din care: cornișon");
                            break;
                        case "75":
                            xmlWriter.WriteAttributeString("denumire", "Ardei total");
                            break;
                        case "76":
                            xmlWriter.WriteAttributeString("denumire", "Vinete");
                            break;
                        case "77":
                            xmlWriter.WriteAttributeString("denumire", "Dovleci");
                            break;
                        case "78":
                            xmlWriter.WriteAttributeString("denumire", "Dovlecei");
                            break;
                        case "79":
                            xmlWriter.WriteAttributeString("denumire", "Alte legume cultivate pentru fruct");
                            break;
                        case "80":
                            xmlWriter.WriteAttributeString("denumire", "Morcovi");
                            break;
                        case "81":
                            xmlWriter.WriteAttributeString("denumire", "Usturoi");
                            break;
                        case "82":
                            xmlWriter.WriteAttributeString("denumire", "Ceapă");
                            break;
                        case "83":
                            xmlWriter.WriteAttributeString("denumire", "Sfeclă roșie");
                            break;
                        case "84":
                            xmlWriter.WriteAttributeString("denumire", "Țelină (rădăcină)");
                            break;
                        case "85":
                            xmlWriter.WriteAttributeString("denumire", "Alte legume rădăcinoase (albitură, hrean, ridichi de lună, ridichi negre, etc.)");
                            break;
                        case "86":
                            xmlWriter.WriteAttributeString("denumire", "Mazăre păstăi");
                            break;
                        case "87":
                            xmlWriter.WriteAttributeString("denumire", "Fasole păstăi");
                            break;
                        case "88":
                            xmlWriter.WriteAttributeString("denumire", "Alte legume păstăi");
                            break;
                        case "89":
                            xmlWriter.WriteAttributeString("denumire", "Porumb zaharat");
                            break;
                        case "90":
                            xmlWriter.WriteAttributeString("denumire", "Ciuperci cultivate");
                            break;
                        case "91":
                            xmlWriter.WriteAttributeString("denumire", "Pepeni verzi");
                            break;
                        case "92":
                            xmlWriter.WriteAttributeString("denumire", "Pepeni galbeni");
                            break;
                        case "93":
                            xmlWriter.WriteAttributeString("denumire", "Plante de nutreț–total (exclusiv sămânța) cod (94+...+98+174)");
                            break;
                        case "94":
                            xmlWriter.WriteAttributeString("denumire", "Porumb verde furajer");
                            break;
                        case "95":
                            xmlWriter.WriteAttributeString("denumire", "Alte plante anuale pentru fân și masă verde");
                            break;
                        case "96":
                            xmlWriter.WriteAttributeString("denumire", "Trifoi și amestec de plante furajere");
                            break;
                        case "97":
                            xmlWriter.WriteAttributeString("denumire", "Lucernă");
                            break;
                        case "98":
                            xmlWriter.WriteAttributeString("denumire", "Alte plante furajere perene");
                            break;
                        case "99":
                            xmlWriter.WriteAttributeString("denumire", "Culturi energetice - total cod (100+...+104)");
                            break;
                        case "100":
                            xmlWriter.WriteAttributeString("denumire", "Salcie energetica");
                            break;
                        case "101":
                            xmlWriter.WriteAttributeString("denumire", "Plop energetic");
                            break;
                        case "102":
                            xmlWriter.WriteAttributeString("denumire", "Miscanthus giganteus (iarba elefantului)");
                            break;
                        case "103":
                            xmlWriter.WriteAttributeString("denumire", "Cynara cardunculus (anghinare)");
                            break;
                        case "104":
                            xmlWriter.WriteAttributeString("denumire", "Alte culturi energetice");
                            break;
                        case "105":
                            xmlWriter.WriteAttributeString("denumire", "Plante pentru producerea de semințe și seminceri, loturi semincere pentru comercializare -total cod (106+123+127+131+132+135+142+145+150+165)");
                            break;
                        case "106":
                            xmlWriter.WriteAttributeString("denumire", "a) Cereale pentru sămânță –total cod (107+...+122)");
                            break;
                        case "107":
                            xmlWriter.WriteAttributeString("denumire", "Grâu comun de toamnă");
                            break;
                        case "108":
                            xmlWriter.WriteAttributeString("denumire", "Grâu comun de primăvară");
                            break;
                        case "109":
                            xmlWriter.WriteAttributeString("denumire", "Grâu dur de toamnă");
                            break;
                        case "110":
                            xmlWriter.WriteAttributeString("denumire", "Grâu dur de primăvară");
                            break;
                        case "111":
                            xmlWriter.WriteAttributeString("denumire", "Secară de toamnă");
                            break;
                        case "112":
                            xmlWriter.WriteAttributeString("denumire", "Secară de primăvară");
                            break;
                        case "113":
                            xmlWriter.WriteAttributeString("denumire", "Orz");
                            break;
                        case "114":
                            xmlWriter.WriteAttributeString("denumire", "Orzoaică de toamnă");
                            break;
                        case "115":
                            xmlWriter.WriteAttributeString("denumire", "Orzoaică de primăvară");
                            break;
                        case "116":
                            xmlWriter.WriteAttributeString("denumire", "Ovăz");
                            break;
                        case "117":
                            xmlWriter.WriteAttributeString("denumire", "Porumb boabe");
                            break;
                        case "118":
                            xmlWriter.WriteAttributeString("denumire", "Sorg pentru boabe");
                            break;
                        case "119":
                            xmlWriter.WriteAttributeString("denumire", "Triticale de toamnă");
                            break;
                        case "120":
                            xmlWriter.WriteAttributeString("denumire", "Triticale de primăvară");
                            break;
                        case "121":
                            xmlWriter.WriteAttributeString("denumire", "Orez");
                            break;
                        case "122":
                            xmlWriter.WriteAttributeString("denumire", "Alte cereale pentru sămânță (mei, hrișcă, etc.)");
                            break;
                        case "123":
                            xmlWriter.WriteAttributeString("denumire", "b) Leguminoase boabe pentru sămânță – total cod (124+125+126)");
                            break;
                        case "124":
                            xmlWriter.WriteAttributeString("denumire", "Mazăre boabe");
                            break;
                        case "125":
                            xmlWriter.WriteAttributeString("denumire", "Fasole boabe");
                            break;
                        case "126":
                            xmlWriter.WriteAttributeString("denumire", "Alte leguminoase boabe pentru sămânță (linte, bob, lupin,etc.)");
                            break;
                        case "127":
                            xmlWriter.WriteAttributeString("denumire", "c) Rădăcinoase pentru sămânță – total cod (128+129+130)");
                            break;
                        case "128":
                            xmlWriter.WriteAttributeString("denumire", "Sfeclă de zahăr");
                            break;
                        case "129":
                            xmlWriter.WriteAttributeString("denumire", "Sfeclă furajeră");
                            break;
                        case "130":
                            xmlWriter.WriteAttributeString("denumire", "Alte rădăcinoase furajere pentru sămânță");
                            break;
                        case "131":
                            xmlWriter.WriteAttributeString("denumire", "d) Cartofi pentru samânță");
                            break;
                        case "132":
                            xmlWriter.WriteAttributeString("denumire", "e) Plante textile pentru sămânță - total cod (133+134)");
                            break;
                        case "133":
                            xmlWriter.WriteAttributeString("denumire", "In pentru fibră");
                            break;
                        case "134":
                            xmlWriter.WriteAttributeString("denumire", "Cânepă pentru fibră");
                            break;
                        case "135":
                            xmlWriter.WriteAttributeString("denumire", "f) Plante oleaginoase pentru sămânță - total cod (136+...+141)");
                            break;
                        case "136":
                            xmlWriter.WriteAttributeString("denumire", "Floarea-soarelui");
                            break;
                        case "137":
                            xmlWriter.WriteAttributeString("denumire", "Rapiță pentru ulei");
                            break;
                        case "138":
                            xmlWriter.WriteAttributeString("denumire", "Soia boabe");
                            break;
                        case "139":
                            xmlWriter.WriteAttributeString("denumire", "Muștar");
                            break;
                        case "140":
                            xmlWriter.WriteAttributeString("denumire", "In pentru ulei");
                            break;
                        case "141":
                            xmlWriter.WriteAttributeString("denumire", "Alte plante oleaginoase pentru sămânță");
                            break;
                        case "142":
                            xmlWriter.WriteAttributeString("denumire", "g) Plante pentru alte industrializări pentru sămânță – total cod (143+144)");
                            break;
                        case "143":
                            xmlWriter.WriteAttributeString("denumire", "Tutun");
                            break;
                        case "144":
                            xmlWriter.WriteAttributeString("denumire", "Sorg pentru mături");
                            break;
                        case "145":
                            xmlWriter.WriteAttributeString("denumire", "h) Plante medicinale, aromatice și condimente pentru sămânță – total cod (146+147+148+149)");
                            break;
                        case "146":
                            xmlWriter.WriteAttributeString("denumire", "Coriandru");
                            break;
                        case "147":
                            xmlWriter.WriteAttributeString("denumire", "Chimion");
                            break;
                        case "148":
                            xmlWriter.WriteAttributeString("denumire", "Fenicul");
                            break;
                        case "149":
                            xmlWriter.WriteAttributeString("denumire", "Alte plante medicinale, aromatice și condimente pentru sămânță");
                            break;
                        case "150":
                            xmlWriter.WriteAttributeString("denumire", "i) Culturi horticole pentru sămânță – total cod (151+...+164)");
                            break;
                        case "151":
                            xmlWriter.WriteAttributeString("denumire", "Tomate");
                            break;
                        case "152":
                            xmlWriter.WriteAttributeString("denumire", "Ceapă sămânță");
                            break;
                        case "153":
                            xmlWriter.WriteAttributeString("denumire", "Arpagic");
                            break;
                        case "154":
                            xmlWriter.WriteAttributeString("denumire", "Usturoi");
                            break;
                        case "155":
                            xmlWriter.WriteAttributeString("denumire", "Varză");
                            break;
                        case "156":
                            xmlWriter.WriteAttributeString("denumire", "Ardei");
                            break;
                        case "157":
                            xmlWriter.WriteAttributeString("denumire", "Castraveți");
                            break;
                        case "158":
                            xmlWriter.WriteAttributeString("denumire", "Rădăcinoase");
                            break;
                        case "159":
                            xmlWriter.WriteAttributeString("denumire", "Mazăre păstăi");
                            break;
                        case "160":
                            xmlWriter.WriteAttributeString("denumire", "Fasole păstăi");
                            break;
                        case "161":
                            xmlWriter.WriteAttributeString("denumire", "Pepeni verzi");
                            break;
                        case "162":
                            xmlWriter.WriteAttributeString("denumire", "Pepeni galbeni");
                            break;
                        case "163":
                            xmlWriter.WriteAttributeString("denumire", "Alte semințe horticole");
                            break;
                        case "164":
                            xmlWriter.WriteAttributeString("denumire", "Seminceri legumicoli (plante mamă)");
                            break;
                        case "165":
                            xmlWriter.WriteAttributeString("denumire", "j) Plante de nutreț pentru sămânță – total cod (166+167+168)");
                            break;
                        case "166":
                            xmlWriter.WriteAttributeString("denumire", "Leguminoase perene");
                            break;
                        case "167":
                            xmlWriter.WriteAttributeString("denumire", "Graminee perene");
                            break;
                        case "168":
                            xmlWriter.WriteAttributeString("denumire", "Anuale și bianuale");
                            break;
                        case "169":
                            xmlWriter.WriteAttributeString("denumire", "Căpșunerii");
                            break;
                        case "170":
                            xmlWriter.WriteAttributeString("denumire", "Flori, plante ornamentale și dendrologice în câmp");
                            break;
                        case "171":
                            xmlWriter.WriteAttributeString("denumire", "Răsadnițe");
                            break;
                        case "172":
                            xmlWriter.WriteAttributeString("denumire", "Suprafața construită a serelor");
                            break;
                        case "173":
                            xmlWriter.WriteAttributeString("denumire", "Suprafața construită a solariilor");
                            break;
                        case "174":
                            xmlWriter.WriteAttributeString("denumire", "Pajiști temporare artificiale (însămânțate pe teren arabil pentru mai puțin de 5 ani)");
                            break;
                        case "175":
                            xmlWriter.WriteAttributeString("denumire", "Câmpuri experimentale");
                            break;
                        case "176":
                            xmlWriter.WriteAttributeString("denumire", "Alte culturi în teren arabil");
                            break;
                        case "177":
                            xmlWriter.WriteAttributeString("denumire", "Total suprafață arabilă cultivată cod (01+20+27+31+35+38+48+54+62+93+99+105+169+170+171+172+173+175+176)");
                            break;
                        case "178":
                            xmlWriter.WriteAttributeString("denumire", "Teren necultivat d.c:");
                            break;
                        case "179":
                            xmlWriter.WriteAttributeString("denumire", "-teren necultivat în sere și solarii");
                            break;
                        case "180":
                            xmlWriter.WriteAttributeString("denumire", "Ogoare");
                            break;
                        case "181":
                            xmlWriter.WriteAttributeString("denumire", "Total teren arabil cod (177+178+180)");
                            break;
                            default:
                                Console.WriteLine("Default case");
                            break;
                        }
                        xmlWriter.WriteStartElement("nrARI");                   //nrARI
                        xmlWriter.WriteAttributeString("value",AjutExport.scoateAri(drXML["tot"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid nrARI
                        xmlWriter.WriteStartElement("nrHA");                    //nrHA
                        xmlWriter.WriteAttributeString("value", AjutExport.scoateHa( drXML["tot"].ToString()));
                        xmlWriter.WriteEndElement();                            //inchid nrHA
                        xmlWriter.WriteEndElement();                            //inchid cultura_in_camp
                    
                }
                //--

                //--
                xmlWriter.WriteEndElement();                        //capitol_4a
                xmlWriter.WriteEndElement();                        //anraportare
                xmlWriter.WriteEndElement();                        //gospodarie
                xmlWriter.WriteEndElement();                        //BODY
                //--

                //DOCUMENT_RAN--
                xmlWriter.WriteEndElement();                        //DOCUMENT_RAN
                xmlWriter.Close();
                drXML.Close();
                //--
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(AjutExport.numefisier(strIdRol) + "xml " + ex.Message);
                Ajutatoare.scrielinie("eroriXML.log",  AjutExport.numefisier(strIdRol) + "xml " + ex.Message);
                return false;
            }
        }
    }
}