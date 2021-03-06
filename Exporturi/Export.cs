using System;
using System.IO;
using System.Data.OleDb;
using exportXml.Validari;
using System.Collections.Generic;

namespace exportXml.Exporturi
{
    public static class Export
    {
        public static void comun(string folderCapitol){
            bool folderExists = Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + folderCapitol);
            if(folderExists==false){
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + folderCapitol);
                Console.WriteLine("am creat: " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" + folderCapitol);
            }
            folderExists = Directory.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong");
            if(folderExists==false ){
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong");
                Console.WriteLine("am creat: " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong");
            }

            AjutExport.stergeInFolder(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" +folderCapitol+ "\\");
            Console.WriteLine("am sters " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\" +folderCapitol+ "\\");

            AjutExport.stergeInFolder(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong\\");
            Console.WriteLine("am sters " + AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\"+folderCapitol+"_Wrong\\");

        }
        public static void cap0_12(){

            string strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE (tip=1 OR tip=2) AND sistat<>\"DA\";";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read())
            {
                RaspunsValidare rsp=new RaspunsValidare();
                rsp=CnpValidare.verificaCNP(drTEMP["cnp"].ToString());
                if (rsp.corect==true)
                {
                    if(CAP0_12.make_CAP0_12xml(drTEMP["idRol"].ToString())==true){
                        string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                        if (strXMLvalid != "ok")
                        {
                            AjutExport.moveWrongXML(
                                AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                                AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                            );
                            Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                        }else{
                            
                            Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + drTEMP["cnp"] + rsp.detalii);
                    Ajutatoare.scrielinie("eroriXML.log", drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + drTEMP["cnp"] + rsp.detalii);
                }
                
            }
            foreach (var item in AjutExport.blacklistcnptitulari)
            {
                Ajutatoare.scrielinie("titulargospodarie.log",item );
            }
            Console.WriteLine("am terminat");
            drTEMP.Close();
            strSQL = "";
        }
        public static void cap0_34(){
            string strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE (tip=3 OR tip=4) AND sistat<>\"DA\";";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();
            while (drTEMP.Read()){
                RaspunsValidare rsp=new RaspunsValidare();
                rsp=CifValidare.verificaCIF(drTEMP["cnp"].ToString());
                if(rsp.corect==true){
                    CAP0_34.make_CAP0_34xml(drTEMP["idRol"].ToString());
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }else{
                    Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + drTEMP["cnp"] + rsp.detalii);
                    Ajutatoare.scrielinie("eroriXML.log", drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + drTEMP["cnp"] + rsp.detalii);
                }
            }
        }
        public static void cap1(){
            string strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE (tip=1 OR tip=2) AND sistat<>\"DA\";";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                RaspunsValidare rsp=new RaspunsValidare();
                rsp=CnpValidare.verificaCNP(drTEMP["cnp"].ToString());

                if(rsp.corect==true && existaInRoluri==true){

                    CAP1.make_CAP1xml(drTEMP["idRol"].ToString());
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP1_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }else{
                    Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + drTEMP["cnp"] + rsp.detalii);
                    Ajutatoare.scrielinie("eroriXML.log", drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + drTEMP["cnp"] + rsp.detalii);
                }
            }
        }
        public static void cap2a(){
            string strSQL="DELETE FROM cap2 WHERE inloc=0 and altloc=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP2) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP2a.make_CAP2axml(drTEMP["idRol"].ToString());

                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2a_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap3(){
            /*string strSQL="DELETE FROM CAP3 WHERE inloc=0 and altloc=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();*/

            string strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP3 WHERE CAP3.NRCRT<=18) AND sistat<>\"DA\";";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP3.make_CAP3xml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP3\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP3\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP3_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap2b(){
            string strSQL="DELETE FROM CAP2b WHERE intra=0 and extra=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, sirues, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP2b) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){
               
                
                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP2b.make_CAP2bxml(drTEMP["idRol"].ToString(),drTEMP["nume"].ToString(),drTEMP["sirues"].ToString(),drTEMP["prenume"].ToString(),drTEMP["cnp"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP2b_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap4a(){
            string strSQL="DELETE FROM CAP4 WHERE inloc=0 and altloc=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM cap4) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP4a.make_CAP4axml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap4a1(){
            string strSQL="DELETE FROM CAP4a1 WHERE sup=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM cap4a1) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP4a1.make_CAP4a1xml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a1\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a1\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4a1_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap4b1(){
            string strSQL="DELETE FROM CAP4b WHERE sup=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM cap4b) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP4b1.make_CAP4b1xml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4b1\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4b1\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4b1_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap4b2(){
            string strSQL="DELETE FROM CAP4b2 WHERE sup=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP4b2) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){
                    CAP4b2.make_CAP4b2xml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4b2\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4b2\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4b2_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap4c(){
            string strSQL="DELETE FROM CAP4c WHERE sup=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP4c) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP4c.make_CAP4cxml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4c\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4c\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP4c_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
         public static void cap5a(){
            string strSQL="DELETE FROM CAP5 WHERE rod=0 and tin=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP5) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP5a.make_CAP5axml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5a_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap5b(){
            string strSQL="DELETE FROM CAP5b WHERE sup=0 and buc=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP5b) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP5b.make_CAP5bxml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5b_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap5c(){
            string strSQL="DELETE FROM CAP5c WHERE sup=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP5c) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                   CAP5c.make_CAP5cxml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5c\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5c\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5c_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap5d(){
            string strSQL="DELETE FROM CAP5d WHERE sup=0;";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP5d) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP5d.make_CAP5dxml(drTEMP["idRol"].ToString());
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5d\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5d\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP5d_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }

            }
        }
        public static void cap6(){
            string strSQL="DELETE FROM CAP6 WHERE inloc=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP6) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                   CAP6.make_CAP6xml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP6\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP6\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP6_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap7(int trimestru){
            string FolderTrimestru="XML\\CAP7_";
            string FolderTrimestruWrong="XML\\CAP7_";
            if(trimestru==1){
                FolderTrimestru+="1\\";
                FolderTrimestruWrong+="1_Wrong\\";
            }else if(trimestru==2){
                FolderTrimestru+="2\\";
                FolderTrimestruWrong+="2_Wrong\\";
            }
            string strSQL="DELETE FROM CAP7 WHERE buc=0 and buc2=0;";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP7) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP7.make_CAP7xml(drTEMP["idRol"].ToString(), trimestru);
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + FolderTrimestru + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + FolderTrimestru + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + FolderTrimestruWrong + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap8(int trimestru){
            string FolderTrimestru="XML\\CAP8_";
            string FolderTrimestruWrong="XML\\CAP8_";
            if(trimestru==1){
                FolderTrimestru+="1\\";
                FolderTrimestruWrong+="1_Wrong\\";
            }else if(trimestru==2){
                FolderTrimestru+="2\\";
                FolderTrimestruWrong+="2_Wrong\\";
            }
            string strSQL="DELETE FROM CAP8 WHERE sem1=0 and sem2=0;";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP8) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP8.make_CAP8xml(drTEMP["idRol"].ToString(), trimestru);
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() +FolderTrimestru + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + FolderTrimestru + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + FolderTrimestruWrong + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap9(){
            string strSQL="DELETE FROM CAP9 WHERE buc=0";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP9) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                   CAP9.make_CAP9xml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP9\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP9\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP9_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap10a(){
            string strSQL="DELETE FROM CAP10a WHERE sup=0 AND can=0";
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP10a) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                   CAP10a.make_CAP10axml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10a_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap10b(){
            string strSQL="DELETE FROM CAP10b WHERE Asup=0 AND Acan=0 AND Fsup=0 AND Fcan=0 AND Psup=0 AND Pcan=0";
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            cmdTEMP.ExecuteNonQuery();

            strSQL = "SELECT idRol, nume, prenume, cnp FROM adrrol WHERE idRol IN (SELECT idRol FROM CAP10b) AND sistat<>\"DA\";";
            cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                   CAP10b.make_CAP10bxml(drTEMP["idRol"].ToString());
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP10b_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap11(){

            string strSQL = "SELECT idRol, nume, sirues, prenume, cnp FROM adrrol WHERE adrrol.idrol in(select idrol from cap11);";
            OleDbCommand cmdTEMP = new System.Data.OleDb.OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP11.make_CAP11xml(drTEMP["idRol"].ToString());
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP11\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP11\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP11_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }

            }
        }
        public static void cap15a(){

            string strSQL = "SELECT idRol, nume, sirues, prenume, cnp FROM adrrol WHERE adrrol.idrol in(select idrol from cap15 WHERE tippers=\"ARENDAS\");";
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP15a.make_CAP15a(drTEMP["idRol"].ToString());
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15a\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15a_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void cap15b(){

            string strSQL = "SELECT idRol, nume, sirues, prenume, cnp FROM adrrol WHERE adrrol.idrol in(select idrol from cap15b WHERE tippers=\"CONCESIONAR\");";
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP15b.make_CAP15b(drTEMP["idRol"].ToString());
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15b\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP15b_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }
            }
        }
        public static void mentiuni(){

            string strSQL = "SELECT idRol, nume, sirues, prenume, cnp FROM adrrol WHERE adrrol.idrol in(select idrol from mentiuni);";
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    MENTIUNI.make_MENTIUNIxml(drTEMP["idRol"].ToString());
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\MENTIUNI\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\MENTIUNI\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\MENTIUNI_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }

            }
        }
        public static void cap12(){

            string strSQL = "SELECT idRol, nume, sirues, prenume, cnp FROM adrrol WHERE adrrol.idrol in(select idrol from cap12);";
            OleDbCommand cmdTEMP = new OleDbCommand(strSQL, BazaDeDate.conexiune);
            OleDbDataReader drTEMP = cmdTEMP.ExecuteReader();

            while (drTEMP.Read()){

                bool existaInRoluri=false;
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true ;
                }
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP0_34\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml")){
                    existaInRoluri=true;
                }

                if(existaInRoluri==true){

                    CAP12.make_CAP12xml(drTEMP["idRol"].ToString());
                    
                    
                    string strXMLvalid = AjutExport.XMLok(AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml");
                    if (strXMLvalid != "ok"){
                        AjutExport.moveWrongXML(
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP12\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml",
                            AppDomain.CurrentDomain.BaseDirectory.ToString() + "XML\\CAP12_Wrong\\" + AjutExport.numefisier(drTEMP["idRol"].ToString()) + "xml"
                        );
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }else{
                        Console.WriteLine(drTEMP["idRol"] + " " + drTEMP["nume"] + " " + drTEMP["prenume"] + " " + strXMLvalid);
                    }
                }

            }
        }
    }
}